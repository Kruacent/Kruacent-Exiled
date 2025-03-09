using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using KE.BlackoutNDoor.API.Features.RoundEffects;
using KE.BlackoutNDoor.Events.EventArgs.RoundEffect;
using MEC;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Waits;
using YamlDotNet.Core.Events;

namespace KE.BlackoutNDoor.API.Features
{
    
    public class Controller
    {        

        private HashSet<RoundEffect> RoundEffects = [];

        public Controller()
        {
            RoundEffects.Add(new Blackout());
            RoundEffects.Add(new DoorStuck());
        }

        

        public IEnumerator<float> Update()
        {
            int wait = UnityEngine.Random.Range(MainPlugin.Instance.Config.MinInterval, MainPlugin.Instance.Config.MaxInterval);
            if (MainPlugin.Instance.Config.Debug) wait = 20;


            do
            {
                if (Warhead.IsInProgress) continue;

                RoundEffect roundEffect = SelectRoundEffect();
                var zone = roundEffect.SelectZone();
                Log.Debug("zone="+zone);
                Log.Debug($"waiting : {wait}");
                yield return Timing.WaitForSeconds(wait);
                PreRoundEffectEventArgs args = new(zone, roundEffect);
                Events.Handlers.RoundEffect.OnPreRoundEffect(args);
                if (args.IsAllowed)
                {
                    var timeyapping = CassieVoiceLine(zone, roundEffect);
                    yield return Timing.WaitForSeconds(5+ timeyapping);
                    roundEffect.Effect(zone);
                    yield return Timing.WaitForSeconds(MainPlugin.Instance.Config.DurationMalfunction);
                    roundEffect.StopEffect(zone);
                    Events.Handlers.RoundEffect.OnPostRoundEffect(new(zone, roundEffect));
                }
                
            } while (Round.InProgress);


        }

        private RoundEffect SelectRoundEffect()
        {
            return RoundEffects.GetRandomValue();
        }

        private float CassieVoiceLine(ZoneType zone,RoundEffect round)
        {
            string nameEvent = round.EventTranslation;
            string msg = $"Warning {nameEvent} in {GetZoneName(zone)} in 5 seconds";
            string colorzone = GetZoneColor(zone).ToHex();
            string msgTranslated = $"<color=#F00>Warning</color> {nameEvent} In <color={colorzone.Remove(colorzone.Length-2)}>{GetZoneName(zone)}</color> in 5 seconds";

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
