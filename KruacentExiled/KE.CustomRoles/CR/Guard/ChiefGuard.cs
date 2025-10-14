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
        public override string Description { get; set; } = "T'as une carte de private \net un crossvec";
        public override uint Id { get; set; } = 1046;
        public override string PublicName { get; set; } = "Chef des Gardes";
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
