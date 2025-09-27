using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.API.Features;
using MEC;
using PlayerRoles;
using Exiled.API.Features;
using UnityEngine;
using Exiled.API.Enums;

namespace KE.CustomRoles.CR.Human
{
    [CustomRole(RoleTypeId.None)]
    public class Inverted : GlobalCustomRole
    {
        public override SideEnum Side { get; set; } = SideEnum.Human;
        public override string Description { get; set; } = "Tu as un talent assez exceptionnel !";
        public override uint Id { get; set; } = 1066;
        public override string PublicName { get; set; } = "Inverted";
        public override bool KeepRoleOnDeath { get; set; } = true;
        public override bool KeepRoleOnChangingRole { get; set; } = true;

        public override float SpawnChance { get; set; } = 100;
        public override Vector3 Scale { get; set; } = new Vector3(1, 1, 1);

        protected override void RoleAdded(Player player)
        {
            player.Scale = new Vector3(1f, -1f, 1f);
            player.EnableEffect(EffectType.Slowness, 200, 99999);
        }

        protected override void RoleRemoved(Player player)
        {
            player.Scale = new Vector3(1f, 1f, 1f);
            player.DisableEffect(EffectType.Slowness);
        }
    }
}