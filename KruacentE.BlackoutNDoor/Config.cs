using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackoutKruacent
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        [Description("the minimum interval between 2 malfunctions")]
        public static int MinInterval { get; set; } = 300;
        [Description("the maximum interval between 2 malfunctions")]
        public static int MaxInterval { get; set; } = 600;
        [Description("chance of having a Blackout at the start of the game")]
        public static double InitialChanceBO { get; set; } = 0.5;
        [Description("the duration of a malfunction")]
        public static int DurationMalfunction { get; set; } = 30;
    }
}
