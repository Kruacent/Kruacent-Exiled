using Exiled.API.Features.Attributes;
using KE.CustomRoles.API.Features;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.CR.SCP
{
    public class Paper : GlobalCustomRole
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Paper",
                    [TranslationKeyDesc] = "uh oh. paper jam",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Paper",
                    [TranslationKeyDesc] = "uh oh. paper jam",
                },
                ["legacy"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Paper",
                    [TranslationKeyDesc] = "uh oh. paper jam",
                },
            };
        }
        public override SideEnum Side { get; set; } = SideEnum.SCP;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;

        public override float SpawnChance { get; set; } = 100;
        public override Vector3 Scale { get; set; } = new Vector3(0.3f, 1, 1);
    }
}
