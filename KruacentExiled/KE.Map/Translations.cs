using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Map
{
    public class Translations : ITranslation
    {

        public string Blackout { get; set; } = "Warning Failure Of All Lights In";
        public string BlackoutTranslation { get; set; } = "<color=#F00>Warning</color> Failure Of All Lights In";
        public string Doorstuck { get; set; } = "Warning Failure Of All Doors In";
        public string DoorstuckTranslation { get; set; } = "<color=#F00>Warning</color> Failure Of All Doors In";
        public string Both { get; set; } = "Warning Failure Of All Systems In";
        public string BothTranslation { get; set; } = "<color=#F00>Warning</color> Failure Of All Systems In";


        public string LightContainment { get; set; } = "Light Containment Zone";
        public string LightContainmentTranslation { get; set; } = "<color=#1BBB9B>Light Containment Zone</color>";
        public string HeavyContainment { get; set; } = "Heavy Containment Zone";
        public string HeavyContainmentTranslation { get; set; } = "<color=#431919>Heavy Containment Zone</color>";
        public string EntranceZone { get; set; } = "Entrance Zone";
        public string EntranceZoneTranslation { get; set; } = "<color=#FFFF00>Entrance Zone</color>";
        public string SurfaceZone { get; set; } = "Surface";
        public string SurfaceZoneTranslation { get; set; } = "<color=#FF0000>Surface</color>";

        public string End { get; set; } = "In 5 seconds";



    }
}
