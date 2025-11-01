using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Usables.Scp330;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using PlayerRoles;
using PlayerRoles.Spectating;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.CR.ClassD
{
    public class Mime : KECustomRole, IColor
    {
        public override string Description { get; set; } = "Tu ne peux pas parler\nmais tu fais très peu de bruit quand tu marches\net t'es tout plat";
        public override string PublicName { get; set; } = "Mime";
        public override string InternalName => PublicName;
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.ClassD;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = true;
        public override float SpawnChance { get; set; } = 100;
        public override Vector3 Scale { get; set; } = new Vector3(0.1f, 1, 1);
        public Color32 Color => new(255, 74, 74, 0);

        protected override void RoleAdded(Player player)
        {
            player.EnableEffect(EffectType.SilentWalk, -1, true); //doesn't work with 939 i think
            player.IsMuted = true;
        }

        protected override void RoleRemoved(Player player)
        {
            player.IsMuted = false;
            player.DisableEffect(EffectType.SilentWalk);
        }

        public override HashSet<string> Abilities { get; } = new()
        {
            "SimulateDeath"
        };
    }
}
