using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.GlobalEventFramework.Examples.API.Feature.CassieGoCrazy
{
    // Turns off the lights in the Heavy Containment zone.
    public class NoLight : ICGCEffect
    {
        public void Effect()
        {
            Exiled.API.Features.Map.TurnOffAllLights(20, Exiled.API.Enums.ZoneType.HeavyContainment);
        }
    }
}
