using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using KE.CustomRoles.API.Features;
using PlayerRoles;
using System.Collections.Generic;

namespace KE.CustomRoles.CR.Guard
{
    internal class Introvert : KECustomRole
    {
        public override string Description { get; set; } = "Tu n'aimes pas trop les humains";
        public override string PublicName { get; set; } = "Introvert";
        public override string InternalName => PublicName;
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.FacilityGuard;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;

        public override float SpawnChance { get; set; } = 100;

        public override List<string> Inventory { get; set; } = new List<string>()
        {
          $"{ItemType.KeycardGuard}",
          $"{ItemType.GunFSP9}",
          $"{ItemType.Medkit}",
          $"{ItemType.GrenadeFlash}",
          $"{ItemType.GrenadeFlash}",
          $"{ItemType.ArmorLight}",
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
          { AmmoType.Nato556, 60}
        };
    }
}
