using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.CustomRoles.API.Features;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using Utils.NonAllocLINQ;

namespace KE.CustomRoles.CR.ClassD
{
    public class DBoyInShape : KECustomRole
    {
        public override string Description { get; set; } = "Dammmmnnnnnnn les gates";
        public override uint Id { get; set; } = 1058;
        public override string PublicName { get; set; } = "DBoyInShape";
        public override RoleTypeId Role { get; set; } = RoleTypeId.ClassD;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = true;
        public override float SpawnChance { get; set; } = 100;
        public override int MaxHealth { get; set; } = 100;

        public const byte SpeedReduction = 15;

        public override HashSet<int> Abilities => new()
        {
            2009
        };

        protected override void RoleAdded(Player player)
        {
            player.EnableEffect(EffectType.Slowness, SpeedReduction,999999 );
        }

        protected override void RoleRemoved(Player player)
        {
            player.DisableEffect(EffectType.Slowness);
        }
    }
}