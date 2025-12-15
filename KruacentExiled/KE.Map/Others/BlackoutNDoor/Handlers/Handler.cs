using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using KE.Map.Others.BlackoutNDoor.Events.EventArgs;
using KE.Utils.API.Interfaces;
using KE.Utils.Extensions;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EventHandle = KE.Map.Others.BlackoutNDoor.Events.Handlers.BlackoutNDoor;

namespace KE.Map.Others.BlackoutNDoor.Handlers
{
    public class Handler : IUsingEvents
    {
        public static float MinInterval = 60 * 3;
        public static float MaxInterval = 60 * 6;

        public Pattern ChosenPattern = null;

        private float time = 0;
        public float TimeBeforeNextEvent => time;

        public static readonly HashSet<ZoneType> Zones = new()
        {
            ZoneType.LightContainment,ZoneType.HeavyContainment,ZoneType.Entrance
        };

        private ZoneType currentScpZone = ZoneType.Unspecified;
        private int weightPerSeconds = 2;
        private int weight = defaultWeight;
        private const int defaultWeight = 330;

        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        }

        private void OnRoundStarted()
        {
            ChosenPattern = Pattern.AllPatterns.GetRandomValue();
            time = GetRandomTime();

            if (MainPlugin.Instance.Config.Debug)
            {
                time = 30;
                ChosenPattern = new Pattern
                ([
                    new Blackout(),new DoorStuck()
                ]);
            }

            
            Timing.RunCoroutine(Timer());
        }

        private IEnumerator<float> Timer()
        {
            int timeRefresh = 10;
            while (Round.InProgress)
            {
                yield return Timing.WaitForSeconds(timeRefresh);
                if (Warhead.IsInProgress) continue;
                Player scp = Player.List.Where(p => p.IsScp && p.Role != RoleTypeId.Scp0492).FirstOrDefault();

                if(scp == null)
                {
                    scp = Player.List.Where(p => p.IsScp).FirstOrDefault();
                }


                if (scp != null)
                {
                    if(scp.Zone == currentScpZone)
                    {
                        weight += timeRefresh* weightPerSeconds;
                    }
                    else
                    {
                        weight = defaultWeight;
                    }
                    currentScpZone = scp.Zone;
                }
                else
                {
                    currentScpZone = ZoneType.Unspecified;
                }
                
                Log.Debug("weight=" + weight + " at " + currentScpZone);


                time-= timeRefresh;
                Log.Debug(time +"s");
                if (time <= 0)
                {
                    LaunchEvent();
                    time = GetRandomTime();
                }
                

            }
        }
        private float GetRandomTime()
        {
            return UnityEngine.Random.Range(MinInterval, MaxInterval);
        }


        private void LaunchEvent()
        {


            MapEvent mapEvent = ChosenPattern.GetNext();

            ChoseMapEventEventArgs choseMapEv = new ChoseMapEventEventArgs(mapEvent);
            EventHandle.OnChoseMapEvent(choseMapEv);
            mapEvent = choseMapEv.MapEvent;
            

            ZoneType zone = GetZone();
            ChoseZoneEventArgs choseZoneEv = new(zone);
            EventHandle.OnChoseZoneEvent(choseZoneEv);

            if (Zones.Contains(choseZoneEv.Zone))
            {
                zone = choseZoneEv.Zone;
            }



            Log.Info(mapEvent.GetType().Name + " at " + zone);



            PreEventEventArgs preEv = new PreEventEventArgs(mapEvent, true);
            EventHandle.OnPreEvent(preEv);


            if (preEv.IsAllowed && Zones.Contains(zone))
            {

                string message = mapEvent.Cassie + " " + ZoneTypeToCassie(zone) + " " + MainPlugin.Translations.End;
                string translate = mapEvent.CassieTranslated + " " + ZoneTypeToCassieTranslated(zone) + " " + MainPlugin.Translations.End;
                float yap = Exiled.API.Features.Cassie.CalculateDuration(message, false, 0);
                Exiled.API.Features.Cassie.MessageTranslated(message, translate,false,false,true);


                Timing.CallDelayed(5 + yap, delegate
                {
                    
                    mapEvent.StartEvent(zone,TimeBeforeNextEvent);

                });
            }
        }


        private string ZoneTypeToCassie(ZoneType zone)
        {
            return zone switch
            {
                ZoneType.LightContainment => MainPlugin.Translations.LightContainment,
                ZoneType.HeavyContainment => MainPlugin.Translations.HeavyContainment,
                ZoneType.Entrance => MainPlugin.Translations.EntranceZone,
                _ => string.Empty
            };
        }

        private string ZoneTypeToCassieTranslated(ZoneType zone)
        {
            return zone switch
            {
                ZoneType.LightContainment => MainPlugin.Translations.LightContainmentTranslation,
                ZoneType.HeavyContainment => MainPlugin.Translations.HeavyContainmentTranslation,
                ZoneType.Entrance => MainPlugin.Translations.EntranceZoneTranslation,
                _ => string.Empty
            };
        }


        private ZoneType GetZone()
        {
            return RandomZoneByWeight();
        }

        private ZoneType RandomZoneByWeight()
        {
            List<ZoneType> result = new();

            List<ZoneType> weightedPool = new();
            foreach (ZoneType zone in Zones.Where(z => z.IsSafe()))
            {
                if(zone != currentScpZone)
                {
                    for (int i = 0; i < defaultWeight; i++)
                    {
                        weightedPool.Add(zone);
                    }
                }
                else
                {
                    for (int i = 0; i < weight; i++)
                    {
                        weightedPool.Add(zone);
                    }
                }


                
            }
            return weightedPool.GetRandomValue();
        }
    }
}
