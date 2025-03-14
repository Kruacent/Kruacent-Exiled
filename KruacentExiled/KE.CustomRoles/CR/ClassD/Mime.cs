using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Usables.Scp330;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.CR.ClassD
{
    [CustomRole(RoleTypeId.ClassD)]
    internal class Mime : Exiled.CustomRoles.API.Features.CustomRole
    {
        public override string Name { get; set; } = "mime";
        public override string Description { get; set; } = "Tu es un <color=#FFC0CB>Mime</color> \nTu ne peux pas parler\nmais tu fais très peu de bruit quand tu marches\net t'es tout plat";
        public override uint Id { get; set; } = 1053;
        public override string CustomInfo { get; set; } = "Mime";
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.ClassD;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = true;
        public override bool IgnoreSpawnSystem { get; set; } = true;

        public override float SpawnChance { get; set; } = 100;
        public override Vector3 Scale { get; set; } = new Vector3(0.1f, 1, 1);

        protected override void RoleAdded(Player player)
        {
            player.EnableEffect(EffectType.SilentWalk, -1, true);
            player.Mute();
        }

        protected override void RoleRemoved(Player player)
        {
            player.UnMute();
            player.DisableEffect(EffectType.SilentWalk);
        }


    }
}
