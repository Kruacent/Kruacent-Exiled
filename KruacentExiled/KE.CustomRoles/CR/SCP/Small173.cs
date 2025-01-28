using Exiled.API.Features.Attributes;
using PlayerRoles;
using UnityEngine;

namespace KE.CustomRoles.CR.SCP
{
    [CustomRole(RoleTypeId.Scp173)]
    internal class Small173 : Exiled.CustomRoles.API.Features.CustomRole
    {
        public override string Name { get; set; } = "Tall173";
        public override string Description { get; set; } = " <color=#F00>Tall NUT</color> \nu tol\n fuck you";
        public override uint Id { get; set; } = 1049;
        public override string CustomInfo { get; set; } = "Small Peanuts";
        public override int MaxHealth { get; set; } = 4500;
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scp173;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override bool IgnoreSpawnSystem { get; set; } = true;
        public override Vector3 Scale { get; set; } = new Vector3(1, 1.15f, 1);
    }
}
