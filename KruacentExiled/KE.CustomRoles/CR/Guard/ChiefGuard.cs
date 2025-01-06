using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using PlayerRoles;
using System.Collections.Generic;

namespace KE.CustomRoles.CR.Guard
{
    [CustomRole(RoleTypeId.FacilityGuard)]
    internal class ChiefGuard : Exiled.CustomRoles.API.Features.CustomRole
    {
        public override string Name { get; set; } = "ChiefGuard";
        public override string Description { get; set; } = "Tu es un <color=#70C3FF>Chef des gardes du site</color> \nT'as une carte de private \net un crossvec";
        public override uint Id { get; set; } = 1041;
        public override string CustomInfo { get; set; } = "Chef des Gardes";
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.FacilityGuard;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override bool IgnoreSpawnSystem { get; set; } = true;

        public override float SpawnChance { get; set; } = 100;

        public override List<string> Inventory { get; set; } = new List<string>()
       {
          $"{ItemType.Radio}",
          $"{ItemType.ArmorLight}",
          $"{ItemType.GunCrossvec}",
          $"{ItemType.Medkit}",
          $"{ItemType.Flashlight}",
          $"{ItemType.KeycardMTFPrivate}",
          $"{ItemType.None}"
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
       {
          { AmmoType.Nato556, 120}
       };

    }
}
