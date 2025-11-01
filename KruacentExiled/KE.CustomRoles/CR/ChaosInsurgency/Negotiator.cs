using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.CustomRoles.API.Features;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.CR.ChaosInsurgency
{
    public class Negotiator : KECustomRole
    {
        public override string Description { get; set; } = "Who knew zombie could be so great listeners";
        public override string InternalName => PublicName;
        public override string PublicName { get; set; } = "Negotiator";
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.ChaosRifleman;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;

        public override float SpawnChance { get; set; } = 100;

        public override HashSet<string> Abilities { get; } = new()
        {
            "Convert"
        };

        

        protected override void RoleAdded(Player player)
        {
            Timing.CallDelayed(.1f, delegate
            {
                player.AddItem(ItemType.Radio);
            });
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            base.UnsubscribeEvents();
        }


        private void OnHurting(HurtingEventArgs ev)
        {
            if (!Check(ev.Player)) return;
            if (!ev.IsAllowed) return;

            if(ev.Attacker.Role.Side == ev.Player.Role.Side)
            {
                ev.IsAllowed = false;
            }

        } 


    }
}
