using Exiled.API.Features.Attributes;
using KE.CustomRoles.API.Features;
using PlayerRoles;
using UnityEngine;

namespace KE.CustomRoles.CR.SCP
{
    [CustomRole(RoleTypeId.None)]
    public class Paper : GlobalCustomRole
    {
        public override string Description { get; set; } = "u are a paper";
        public override uint Id { get; set; } = 1047;
        public override SideEnum Side { get; set; } = SideEnum.SCP;
        public override string PublicName { get; set; } = "Paper";
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;

        public override float SpawnChance { get; set; } = 100;
        public override Vector3 Scale { get; set; } = new Vector3(0.1f, 1, 1);
    }
}
