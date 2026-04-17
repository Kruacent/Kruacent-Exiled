using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.API.Features;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KruacentExiled.CustomRoles.CR.SCP
{
    public class Small : GlobalCustomRole
    {
        public override SideEnum Side { get; set; } = SideEnum.SCP;
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Small",
                    [TranslationKeyDesc] = "u smoll",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Petit",
                    [TranslationKeyDesc] = "t poti",
                },
                ["legacy"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Small",
                    [TranslationKeyDesc] = "u smoll",
                },
            };
        }
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;

        public override float SpawnChance { get; set; } = 100;
        public override Vector3 Scale { get; set; } = new Vector3(1, .75f, 1);
    }
}
