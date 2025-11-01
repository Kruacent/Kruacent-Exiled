using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.API.Features;
using PlayerRoles;
using UnityEngine;

namespace KE.CustomRoles.CR.SCP
{
    public class Small : GlobalCustomRole
    {
        public override string Description { get; set; } = "u smoll";
        public override SideEnum Side { get; set; } = SideEnum.SCP;
        public override string PublicName { get; set; } = "Small";
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;

        public override float SpawnChance { get; set; } = 100;
        public override Vector3 Scale { get; set; } = new Vector3(1, .75f, 1);
    }
}
