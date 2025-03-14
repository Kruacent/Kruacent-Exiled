using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.Abilities;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.CR.SCP
{
    [CustomRole(RoleTypeId.Scp0492)]
    internal class ZombieDoorman : CustomRole
    {
        public override string Name { get; set; } = "SCP-049-2-Door";
        public override string Description { get; set; } = "U can open lock door with ur ability clapclapclap";
        public override uint Id { get; set; } = 1053;
        public override string CustomInfo { get; set; } = "SCP-049-2-Door";
        public override int MaxHealth { get; set; } = 400;
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scp0492;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override bool IgnoreSpawnSystem { get; set; } = true;

        public override float SpawnChance { get; set; } = 100;
        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>()
        {
            new OpenDoor()
        };
    }
}
