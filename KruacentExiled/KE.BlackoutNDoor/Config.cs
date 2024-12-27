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
        public int MinInterval { get; set; } = 300;
        [Description("the maximum interval between 2 malfunctions")]
        public int MaxInterval { get; set; } = 600;
        [Description("chance of having a Blackout at the start of the game")]
        public double InitialChanceBO { get; set; } = 0.5;
        [Description("the duration of a malfunction")]
        public int DurationMalfunction { get; set; } = 30;

        public float[] ChancePreConta = { .2f, .3f, .3f, .15f, .05f };
        public float[] ChancePostConta = { 0, .4f, .4f, .15f, .05f };
    }
}
