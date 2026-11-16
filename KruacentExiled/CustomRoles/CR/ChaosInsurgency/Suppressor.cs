using Exiled.API.Enums;
using KruacentExiled.CustomRoles.API.Features;
using PlayerRoles;
using System.Collections.Generic;

namespace KruacentExiled.CustomRoles.CR.ChaosInsurgency
{
    public class Suppressor : KECustomRole
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Suppressor",
                    [TranslationKeyDesc] = "Aim for the head for a quick reload",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Supresseur",
                    [TranslationKeyDesc] = "Des fois, des sacrifices sont nécessaires",
                },
                ["legacy"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Supresseur",
                    [TranslationKeyDesc] = "Des fois, des sacrifices sont nécessaires",
                }
            };
        }

        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.ChaosRifleman;

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            $"{ItemType.GunLogicer}", $"{ItemType.ArmorHeavy}", $"{ItemType.GrenadeHE}",
            $"{ItemType.Medkit}", $"{ItemType.Medkit}", $"{ItemType.Adrenaline}",
            $"{ItemType.Radio}", $"{ItemType.KeycardChaosInsurgency}"
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato762, 200 }
        };

        public override HashSet<string> Abilities { get; } = new HashSet<string>()
        {
            "TacticalReload"
        };
    }
}
