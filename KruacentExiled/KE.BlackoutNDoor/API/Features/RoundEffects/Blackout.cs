using Exiled.API.Enums;
using Exiled.API.Features;
using System.Collections.Generic;

namespace KE.BlackoutNDoor.API.Features.RoundEffects
{
    public class Blackout : RoundEffect
    {
        
        public Blackout()
        {
            base.EventTranslation = "Failure Of All Lights";
            base.Chances = new Dictionary<ZoneType,int>()
            {
                { ZoneType.LightContainment, 33 },
                { ZoneType.HeavyContainment, 33 },
                { ZoneType.Entrance, 34 },
            };
        }


        public override void Effect(ZoneType zone)
        {
            Map.TurnOffAllLights(999, zone);
        }

        public override void StopEffect(ZoneType zone)
        {
            Map.TurnOffAllLights(0f, zone);
        }




    }



}
