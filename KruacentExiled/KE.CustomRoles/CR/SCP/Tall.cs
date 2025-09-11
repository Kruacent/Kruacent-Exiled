using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using KE.CustomRoles.API.Features;
using PlayerRoles;
using System.Linq;
using UnityEngine;

namespace KE.CustomRoles.CR.SCP
{
    [CustomRole(RoleTypeId.None)]
    public class Tall : GlobalCustomRole
    {
        public override string Description { get; set; } = "u tall";
        public override uint Id { get; set; } = 1049;
        public override string PublicName { get; set; } = "Tall";
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override float MaxHealthMultiplicator { get; set; } = 1.1f;
        public override float SpawnChance { get; set; } = 100;
        public override SideEnum Side { get; set; } = SideEnum.SCP;
        public new Vector3 Scale { get; set; } = new(1, 1.3f, 1);
        public Vector3 BaseScale => Vector3.one;
        protected override void RoleAdded(Player player)
        {
            player.SetFakeScale(Scale, Player.List.Where(p => p != player));

            
        }

        protected override void RoleRemoved(Player player)
        {
            player.SetFakeScale(BaseScale, Player.List.Where(p => p != player));
        }
    }
}
