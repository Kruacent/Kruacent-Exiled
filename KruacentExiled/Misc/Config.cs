using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace KruacentExiled.Misc
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = true;
        [Description("Chance to d-boy doors goes boom")]
        public int ChanceClassDDoorGoesBoom { get; set; } = 2;
        [Description("Enable or disable the auto elevator")]
        public bool AutoElevator { get; set; } = true;
        [Description("Chance to get a pink candy (0-100)")]
        public int ChancePinkCandy { get; set; } = 10;
        public bool SurfaceLight { get; set; } = true;

        public bool ScpPreferences { get; set; } = true;
        public bool Scp035Enabled { get; set; } = true;
        public bool GamblingCoin { get; set; } = true;
        public int GamblingCoinMinUse { get; set; } = 1;
        public int GamblingCoinMaxUse { get; set; } = 2;
        public int GamblingCoinCooldown { get; set; } = 3;

        [Description("health mutiplicator scps)")]
        public float MultSCP049 { get; set; } = 0.8f;
        public float MultSCP939 { get; set; } = 1.2f;
        public float MultSCP106 { get; set; } = 1.1f;
        public int MinPlayerVote { get; set; } = 6;

        public float MultBaseSCP { get; set; } = 1.25f;

        [Description("birthday song for other person")]
        public string DefaultBirthdayMusic { get; set; } = "birthday_default";

        [Description("personalized birthday song for every player here")]
        public Dictionary<string, string> CustomBirthdayMusicPerPlayer { get; set; } = new Dictionary<string, string>()
        {
            { "76561199573165041@steam", "birthday_rotakzz" },
            { "76561198356056056@steam", "birthday_patrique" },
            { "76561199116619876@steam", "birthday_choco" },
            { "76561199151610419@steam", "birthday_jomamouel" },
            { "76561199066936074@steam", "birthday_delecons" },
            { "76561198209599925@steam", "birthday_alex" },
            { "76561198847052333@steam", "birthday_caedit" }
        };

    }
}
