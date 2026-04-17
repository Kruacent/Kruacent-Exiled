using Exiled.API.Enums;
using KruacentExiled.CustomRoles.API.Features;
using PlayerRoles;
using System.Collections.Generic;

namespace KruacentExiled.CustomRoles.CR.MTF
{
    public class Pilot : KECustomRole
    {
        public override int MaxHealth { get; set; } = 90;
        public override RoleTypeId Role { get; set; } = RoleTypeId.NtfPrivate;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;

        public override float SpawnChance { get; set; } = 100;
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Pilot",
                    [TranslationKeyDesc] = "So I haveth a Laser Pointere",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Pilote",
                    [TranslationKeyDesc] = "Je suis pilote!",
                },
                ["legacy"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Pilot",
                    [TranslationKeyDesc] = "So I haveth a Laser Pointere",
                },
            };
        }

        public override List<string> Inventory { get; set; } = new List<string>()
       {
          $"{ItemType.GunCrossvec}",
          $"{ItemType.KeycardMTFOperative}",
          $"{ItemType.Radio}",
          $"{ItemType.ArmorCombat}",
          $"{ItemType.Medkit}"
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
          { AmmoType.Nato9, 100}
        };

        public override HashSet<string> Abilities { get; } = new HashSet<string>()
        {
            "SetPosition",
            "AirStrike"
        };
    }
}
