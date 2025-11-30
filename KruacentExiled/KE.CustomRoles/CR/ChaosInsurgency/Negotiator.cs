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
        public override string Description { get; set; } = "You're immune to friendly fire and you can convert zombie into your team, isn't that nice?";
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


        public override List<string> Inventory { get; set; } = new()
        {
            "KeycardChaosInsurgency",
            "GunAK",
            "Medkit",
            "Painkillers",
            "ArmorCombat",
            "Radio"
        };

        
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
            if (ev.Attacker is null) return;

            if(ev.Attacker.Role.Side == ev.Player.Role.Side)
            {
                ev.IsAllowed = false;
            }

        } 


    }
}
