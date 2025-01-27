using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using PlayerRoles;
using System.Collections.Generic;

namespace KE.CustomRoles.CR.Guard
{
    [CustomRole(RoleTypeId.FacilityGuard)]
    internal class Guard914 : Exiled.CustomRoles.API.Features.CustomRole
    {
        public override string Name { get; set; } = "guard914";
        public override string Description { get; set; } = "Tu es <b>Le</b> <color=#6B6B38>garde de SCP-914</color> \nTu commences à 914 \nmais on a volé ta carte \net ntm aussi";
        public override uint Id { get; set; } = 1405;
        public override string CustomInfo { get; set; } = "Garde de 914";
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.FacilityGuard;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override bool IgnoreSpawnSystem { get; set; } = true;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            DynamicSpawnPoints = new List<DynamicSpawnPoint>()
            {
                new DynamicSpawnPoint()
                {
                    Location = Exiled.API.Enums.SpawnLocationType.Inside914,
                    Chance = 100,
                }
            }
        };

        public override float SpawnChance { get; set; } = 100;

        public override List<string> Inventory { get; set; } = new List<string>()
       {
          $"{ItemType.Radio}",
          $"{ItemType.ArmorLight}",
          $"{ItemType.GunFSP9}",
          $"{ItemType.Medkit}",
          $"{ItemType.Flashlight}"
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
       {
          { AmmoType.Nato556, 60}
       };

    }
}
