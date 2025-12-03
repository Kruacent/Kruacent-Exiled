using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp1509;
using Exiled.Events.EventArgs.Server;
using KE.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.CR.SCP
{
    public class SCP035 : CustomSCP
    {
        public override bool IsSupport => false;

        public override string PublicName { get; set; } = "SCP-035";
        public override int MaxHealth { get; set; } = 800;
        public override string Description { get; set; } = "KILL EVERYONE";
        protected override int SettingId => 10002;

        public override RoleTypeId Role { get; set; } = RoleTypeId.Tutorial;

        // 035 can't pickup these items
        public HashSet<ItemType> BlacklistedPickup = new()
        {
            ItemType.Jailbird,
            ItemType.ParticleDisruptor,
            ItemType.MicroHID,
        };
        // 035 can't be damaged by these
        public HashSet<DamageType> BlacklistedDamage = new()
        {
            DamageType.Jailbird,
            DamageType.ParticleDisruptor,
            DamageType.MicroHid
        };

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Player.SearchingPickup += OnSearchingPickup;
            Exiled.Events.Handlers.Server.EndingRound += OnEndingRound;
            Exiled.Events.Handlers.Scp1509.Resurrecting += OnResurrecting;

            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Player.SearchingPickup -= OnSearchingPickup;
            Exiled.Events.Handlers.Server.EndingRound -= OnEndingRound;
            Exiled.Events.Handlers.Scp1509.Resurrecting += OnResurrecting;
            base.UnsubscribeEvents();
        }

        protected override void RoleAdded(Player player)
        {

            player.VoiceChannel = VoiceChat.VoiceChatChannel.ScpChat;
            base.RoleAdded(player);
        }

        private void OnSearchingPickup(SearchingPickupEventArgs ev)
        {
            if (BlacklistedPickup.Contains(ev.Pickup.Type))
            {
                ShowEffectHint(ev.Player, "This item can't be picked up");
                ev.IsAllowed = false;
                return;
            }
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (Check(ev.Player))
            {
                if (BlacklistedDamage.Contains(ev.DamageHandler.Type))
                {
                    if (ev.IsAllowed)
                    {
                        ev.DamageHandler.Damage /= 2;
                    }
                    
                    return;
                }

                if (ev.Attacker is not null && ev.Attacker.Role.Side == Side.Scp)
                {
                    ev.IsAllowed = false;
                    return;
                }
            }

            if (Check(ev.Attacker))
            {
                if (ev.Player.Role.Side == Side.Scp)
                {
                    ev.IsAllowed = Server.FriendlyFire || ev.Attacker.IsFriendlyFireEnabled;
                    return;
                }
            }



        }

        private void OnResurrecting(ResurrectingEventArgs ev)
        {
            if (!Check(ev.Player)) return;

            ev.NewRole = RoleTypeId.Scp0492;
        }

        public void OnEndingRound(EndingRoundEventArgs ev)
        {
            if (TrackedPlayers.Count <= 0) return;

            if (ev.ClassList.mtf_and_guards != 0 || ev.ClassList.scientists != 0) ev.IsAllowed = false;
            else if (ev.ClassList.class_ds != 0 || ev.ClassList.chaos_insurgents != 0) ev.IsAllowed = false;
            else if (ev.ClassList.scps_except_zombies + ev.ClassList.zombies > 0) ev.IsAllowed = true;
            else ev.IsAllowed = true;
        }
    }
}
