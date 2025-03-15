using Exiled.API.Features.Attributes;
using KE.CustomRoles.API;
using PlayerRoles;
using UnityEngine;

namespace KE.CustomRoles.CR.SCP
{
    [CustomRole(RoleTypeId.None)]
    public class Tall : GlobalCustomRole
    {
        public override string Name { get; set; } = "Tall";
        public override string Description { get; set; } = "u tall";
        public override uint Id { get; set; } = 1049;
        public override string CustomInfo { get; set; } = "Tall";
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override float MaxHealthMultiplicator { get; set; } = 1.1f;
        public override float SpawnChance { get; set; } = 100;
        public override SideEnum Side { get; set; } = SideEnum.SCP;
        public override Vector3 Scale { get; set; } = new Vector3(1, 1.15f, 1);
    }
}
