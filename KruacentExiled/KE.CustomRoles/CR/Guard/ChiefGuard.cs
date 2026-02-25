using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.CR.Guard
{
    public class ChiefGuard : KECustomRole, IColor
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Chief Guard",
                    [TranslationKeyDesc] = "you got a private card and a crossvec",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Chef des gardes",
                    [TranslationKeyDesc] = "T'as une carte de private \net un crossvec",
                }
                ,["legacy"] = new()
                {
                    [TranslationKeyName] = "Chef des gardes",
                    [TranslationKeyDesc] = "T'as une carte de private \net un crossvec",
                }
            };
        }
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.FacilityGuard;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;

        public override float SpawnChance { get; set; } = 100;

        public override List<string> Inventory { get; set; } = new List<string>()
       {
          $"{ItemType.Radio}",
          $"{ItemType.ArmorLight}",
          $"{ItemType.GunCrossvec}",
          $"{ItemType.Medkit}",
          $"{ItemType.Flashlight}",
          $"{ItemType.KeycardMTFPrivate}"
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
       {
          { AmmoType.Nato556, 120}
       };

        public Color32 Color => new Color32(112, 195, 255, 0);
    }
}
