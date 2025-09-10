using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using KE.BlackoutNDoor.Events.EventArgs.RoundEffect;
using KE.Utils.API.Interfaces;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.BlackoutNDoor.API.Features.RoundEffects
{
    public abstract class RoundEffect
    {
        /// <summary>
        /// How Cassie will announce the message
        /// </summary>
        public string EventTranslation { get; protected set; }
        public IReadOnlyDictionary<ZoneType,int> Chances { get; protected set; }
        public abstract void Effect(ZoneType zone);
        public abstract void StopEffect(ZoneType zone);


        internal ZoneType SelectZone()
        {
            int totalWeight = 0;
            foreach (var weight in Chances.Values)
            {
                totalWeight += weight;
            }

            if (totalWeight == 0)
            {
                Log.Error("Total probability must be greater than zero.");
                return ZoneType.Unspecified;
            }

            int randValue = UnityEngine.Random.Range(0, totalWeight);
            int cumulativeSum = 0;

            foreach (var entry in Chances)
            {
                cumulativeSum += entry.Value;
                if (randValue < cumulativeSum)
                    return entry.Key;
            }

            Log.Error("Zone selection failed");
            return ZoneType.Unspecified;
        }


        public static readonly Controller Controller = new();
        public static HashSet<RoundEffect> AllEffect { get; set; }





        public static void SubscribeEvents()
        {
            Controller.SubscribeEvents();
        }

        public static void UnsubscribeEvents()
        {
            Controller.UnsubscribeEvents();
        }




    }
    public class Controller : IUsingEvents
    {

        private HashSet<RoundEffect> RoundEffects = [];
        private static CoroutineHandle Handle;

        public Controller()
        {
            RoundEffect.AllEffect = new()
            {
                new Blackout(),
                new DoorStuck()
            };
        }


        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
        }

        private void OnRoundStarted()
        {
            Log.Debug($"handle = {Handle}");
            Timing.KillCoroutines(Handle);
            Handle = Timing.RunCoroutine(Update());
        }

        private void OnWaitingForPlayers()
        {
            Timing.KillCoroutines(Handle);
        }



        public IEnumerator<float> Update()
        {
            int wait = UnityEngine.Random.Range(MainPlugin.Instance.Config.MinInterval, MainPlugin.Instance.Config.MaxInterval);
            if (MainPlugin.Instance.Config.Debug) wait = 20;


            while (Round.InProgress)
            {
                if (Warhead.IsInProgress) continue;

                RoundEffect roundEffect = SelectRoundEffect();
                var zone = roundEffect.SelectZone();
                Log.Debug("zone=" + zone);
                Log.Debug($"waiting : {wait}");
                yield return Timing.WaitForSeconds(wait);
                PreRoundEffectEventArgs args = new(zone, roundEffect);
                Events.Handlers.RoundEffect.OnPreRoundEffect(args);
                if (args.IsAllowed)
                {
                    var timeyapping = CassieVoiceLine(zone, roundEffect);
                    yield return Timing.WaitForSeconds(5 + timeyapping);
                    roundEffect.Effect(zone);
                    yield return Timing.WaitForSeconds(MainPlugin.Instance.Config.DurationMalfunction);
                    roundEffect.StopEffect(zone);
                    Events.Handlers.RoundEffect.OnPostRoundEffect(new(zone, roundEffect));
                }

            } 


        }

        private RoundEffect SelectRoundEffect()
        {
            return RoundEffects.GetRandomValue();
        }

        private float CassieVoiceLine(ZoneType zone, RoundEffect round)
        {
            string nameEvent = round.EventTranslation;
            string msg = $"Warning {nameEvent} in {GetZoneName(zone)} in 5 seconds";
            string colorzone = GetZoneColor(zone).ToHex();
            string msgTranslated = $"<color=#F00>Warning</color> {nameEvent} In <color={colorzone.Remove(colorzone.Length - 2)}>{GetZoneName(zone)}</color> in 5 seconds";

            Cassie.MessageTranslated(msg, msgTranslated, true, false, true);
            return Cassie.CalculateDuration(msg);

        }

        private Color GetZoneColor(ZoneType zone)
        {
            return zone switch
            {
                ZoneType.LightContainment => new Color(0.1058f, 0.7333f, 0.6078f),
                ZoneType.HeavyContainment => new Color(0.2627f, 0.0980f, 0.0980f),
                ZoneType.Entrance => new Color(1, 1, 0),
                _ => new Color(1, 0, 0)
            };
        }


        private string GetZoneName(ZoneType zone)
        {
            return zone switch
            {
                ZoneType.LightContainment => "Light Containment Zone",
                ZoneType.HeavyContainment => "Heavy Containment Zone",
                ZoneType.Entrance => "Entrance Zone",
                ZoneType.Surface => "Surface",
                ZoneType.Unspecified => "All of the facility",
                _ => "somewhere"
            };
        }



    }
}
