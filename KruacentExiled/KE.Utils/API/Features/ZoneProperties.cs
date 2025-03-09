using Exiled.API.Enums;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;


namespace KE.Utils.API.Features
{
    public class ZoneProperties
    {
        private ZoneType _zone;
        public ZoneProperties(ZoneType zone)
        {
            _zone = zone;
        }

        public ZoneType Zone
        {
            get
            {
                /*if (_zone == ZoneType.Unspecified)
                    return Enum.GetValues(typeof(ZoneType)).ToArray<ZoneType>().ToHashSet();
                return [_zone];*/
                return _zone;
            }
        }


        public override string ToString()
        {
            return _zone switch
            {
                ZoneType.Unspecified => "All of the facility",
                ZoneType.LightContainment => "Light Containment Zone",
                ZoneType.HeavyContainment => "Heavy Containment Zone",
                ZoneType.Entrance => "Entrance Zone",
                ZoneType.Surface => "Surface",
                ZoneType.Pocket => "Pocket Dimension",
                _ => string.Empty,
            };
        }
    }
}
