using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.CustomRoles.API;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.NonAllocLINQ;

namespace KE.CustomRoles.CR.ClassD
{
    [CustomRole(RoleTypeId.ClassD)]
    internal class DBoyInShape : KECustomRole
    {
        public override string Name { get; set; } = "DBoyInShape";
        public override string Description { get; set; } = "";
        public override uint Id { get; set; } = 1058;
        public override string CustomInfo { get; set; } = "DBoyInShape";
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = true;
        public override float SpawnChance { get; set; } = 100;
        public override int MaxHealth { get; set; } = 100;

        private const byte _speedReduction = 15;

        protected override void RoleAdded(Player player)
        {
            player.EnableEffect(EffectType.Slowness, 5, _speedReduction);
        }

        protected override void RoleRemoved(Player player)
        {
            player.DisableEffect(EffectType.Slowness);
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.InteractingDoor += InteractingDoor;
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.InteractingDoor -= InteractingDoor;
        }

        public void InteractingDoor(InteractingDoorEventArgs ev)
        {
            if (ev.IsAllowed) return;
            if (!Check(ev.Player)) return;

            int successRate;
            int damage;

            if (ev.Door.Type.IsGate())
            {
                successRate = 20;
                damage = 20;
            }
            else if (ev.Door.Type.IsCheckpoint())
            {
                successRate = 30;
                damage = 10;
            }
            else
            {
                successRate = 40;
                damage = 5;
            }

            int proba = UnityEngine.Random.Range(0, 101);

            if (proba <= successRate)
            {
                ev.IsAllowed = true;
                Log.Info($"{ev.Player.Nickname} a réussi à ouvrir une {ev.Door.Type} !");
            }
            else
            {
                Log.Info($"{ev.Player.Nickname} a échoué à ouvrir une {ev.Door.Type} et a perdu {damage} HP !");
                ev.Player.Hurt(damage, DamageType.SeveredHands);
            }
        }
    }
}