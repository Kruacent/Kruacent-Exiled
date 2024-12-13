using Exiled.API.Interfaces;
using System.ComponentModel;

namespace KE.Misc
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = true;
        [Description("Chance that the friendly fire is enabled at the start of the round (set 0 to disable)")]
        public int ChanceFF { get; set; } = 50;
        [Description("Enable or disable the auto-nuke annoucement")]
        public int ChanceClassDDoorGoesBoom { get; set; } = 2;
        [Description("Chance to d-boy doors goes boom")]
        public bool AutoNukeAnnoucement {  get; set; } = true;
        [Description("Enable or disable the lockdown of SCP-173")]
        public bool PeanutLockDown { get; set; } = true;
        [Description("Enable or disable the auto elevator")]
        public bool AutoElevator { get; set; } = true;
    }
}
