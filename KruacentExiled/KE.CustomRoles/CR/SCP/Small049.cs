using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using UnityEngine;

namespace KE.CustomRoles.CR.SCP
{
    [CustomRole(RoleTypeId.Scp049)]
    internal class Small049 : CustomRole
    {
        public override string Name { get; set; } = "Small049";
        public override string Description { get; set; } = "u are a smoll doctor";
        public override uint Id { get; set; } = 1048;
        public override string CustomInfo { get; set; } = "Small Doctor";
        public override int MaxHealth { get; set; } = 2300;
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scp049;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override bool IgnoreSpawnSystem { get; set; } = true;

        public override float SpawnChance { get; set; } = 100;
        public override Vector3 Scale { get; set; } = new Vector3(1, 0.75f, 1);
    }
}
