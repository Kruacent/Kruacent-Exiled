using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.API.Interfaces;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KruacentExiled.CustomRoles.CR.Scientist
{
    public class GambleAddict : KECustomRole, IColor
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Gamble Addict",
                    [TranslationKeyDesc] = "you got 2 coins\ngood luck",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Accro du casino",
                    [TranslationKeyDesc] = "T'as trade ton kit et ta carte contre 2 pièces \nfais en bon usage",
                },
                ["legacy"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Gamble Addict",
                    [TranslationKeyDesc] = "T'as trade ton kit et ta carte contre 2 pièces \nfais en bon usage",
                },
            };
        }
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scientist;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;

        public override float SpawnChance { get; set; } = 100;

        public override List<string> Inventory { get; set; } = new List<string>()
        {
          $"{ItemType.Coin}",
          $"{ItemType.Coin}",
        };

        public override HashSet<string> Abilities { get; } = new HashSet<string>()
        {
            "Trade"
        };

        public Color32 Color => new Color32(0, 105, 59,0);
    }
}
