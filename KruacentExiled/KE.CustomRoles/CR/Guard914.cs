using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.CR
{
    [CustomRole(RoleTypeId.FacilityGuard)]
    internal class Guard914 : Exiled.CustomRoles.API.Features.CustomRole
    {
        public override string Name { get; set; } = "guard914";
        public override string Description { get; set; } = "Tu es <b>Le</b> <color=#6B6B38>garde de SCP-914</color> \nTu commences à 914 \nmais on a volé ta carte \net ntm aussi";
        public override uint Id { get; set; } = 1040;
        public override string CustomInfo { get; set; } = "Garde de 914";
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.FacilityGuard;
        public override bool KeepRoleOnDeath { get ; set ; } = false;
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


    }
}
