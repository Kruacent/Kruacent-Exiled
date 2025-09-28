using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.CR.MTF
{
    public class Terroriste : KECustomRole, IColor
    {
        public override string Description { get; set; } = "Ne fait pas exploser la facilité \ntu commences avec des grenades";
        public override uint Id { get; set; } = 1052;
        public override string PublicName { get; set; } = "Terroriste";
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.NtfSergeant;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public Color32 Color => new(105, 52, 22, 0);

        public override float SpawnChance { get; set; } = 100;
        public override List<string> Inventory { get; set; } = new List<string>()
       {
          $"{ItemType.GrenadeHE}",
          $"{ItemType.GrenadeHE}",
          $"{ItemType.GrenadeHE}",
          $"{ItemType.GrenadeHE}",
          $"{ItemType.GunE11SR}",
          $"{ItemType.Adrenaline}",
          $"{ItemType.KeycardMTFOperative}",
          $"{ItemType.Radio}"
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
          { AmmoType.Nato556, 100}
        };

        public override HashSet<int> Abilities { get; } = new()
        {
            2007
        };
    }
}
