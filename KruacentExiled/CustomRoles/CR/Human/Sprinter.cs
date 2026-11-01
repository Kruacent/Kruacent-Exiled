using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.CustomRoles.CR.Human
{
    public class Sprinter : GlobalCustomRole, IColor
    {
        public override SideEnum Side { get; set; } = SideEnum.Human;

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Sprinter",
                    [TranslationKeyDesc] = "Run forest, Run!",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Sprinter",
                    [TranslationKeyDesc] = "Tu as des chaussures qui courent vites",
                },
                ["legacy"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Sprinter",
                    [TranslationKeyDesc] = "Tu as des chaussures qui courent vites",
                }

            };
        }
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = true;
        public override float SpawnChance { get; set; } = 100;
        public Color32 Color => new Color32(221, 17, 17, 0);

        public override HashSet<string> Abilities { get; } = new HashSet<string>()
        {
            "Sprint"
        };
    }
}
