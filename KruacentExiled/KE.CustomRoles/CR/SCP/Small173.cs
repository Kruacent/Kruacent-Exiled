using Exiled.API.Features.Attributes;
using PlayerRoles;
using UnityEngine;

namespace KE.CustomRoles.CR.SCP
{
    [CustomRole(RoleTypeId.Scp173)]
    internal class Small173 : Exiled.CustomRoles.API.Features.CustomRole
    {
        public override string Name { get; set; } = "Small173";
        public override string Description { get; set; } = "u are a small peanuts";
        public override uint Id { get; set; } = 1046;
        public override string CustomInfo { get; set; } = "Small Peanuts";
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scp173;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override bool IgnoreSpawnSystem { get; set; } = true;

        public override float SpawnChance { get; set; } = 100;
        public override Vector3 Scale { get; set; } = new Vector3(1, 0.75f, 1);
    }
}
