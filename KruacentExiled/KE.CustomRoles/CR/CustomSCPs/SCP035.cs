using Cassie;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp1509;
using Exiled.Events.EventArgs.Server;
using KE.CustomRoles.API.Features;
using PlayerRoles;
using System.Collections.Generic;

namespace KE.CustomRoles.CR.CustomSCPs
{
    public class SCP035 : CustomSCP
    {
        public override bool IsSupport => false;

        public override string PublicName { get; set; } = "SCP-035";
        public override int MaxHealth { get; set; } = 1300;
        public override string Description { get; set; } = "You can't pickup the Micro-HID and anything made with it, but you take 2 time less damage by these weapon";
        protected override int SettingId => 10002;

        public override RoleTypeId Role { get; set; } = RoleTypeId.Tutorial;

        // 035 can't pickup these items
        public HashSet<ItemType> BlacklistedPickup = new()
        {
            ItemType.Jailbird,
            ItemType.ParticleDisruptor,
            ItemType.MicroHID,
            ItemType.Painkillers,
            ItemType.Medkit,
            ItemType.SCP500,
        };
        public HashSet<ItemType> BlacklistedUsing = new()
        {
            ItemType.Painkillers,
            ItemType.Medkit,
            ItemType.SCP500,
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
            Exiled.Events.Handlers.Player.Dying += OnDying;
            Exiled.Events.Handlers.Player.SearchingPickup += OnSearchingPickup;
            Exiled.Events.Handlers.Server.EndingRound += OnEndingRound;
            Exiled.Events.Handlers.Scp1509.Resurrecting += OnResurrecting;
            Exiled.Events.Handlers.Player.UsingItem += OnUsingItem;

            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            Exiled.Events.Handlers.Player.SearchingPickup -= OnSearchingPickup;
            Exiled.Events.Handlers.Server.EndingRound -= OnEndingRound;
            Exiled.Events.Handlers.Scp1509.Resurrecting -= OnResurrecting;
            Exiled.Events.Handlers.Player.UsingItem -= OnUsingItem;
            base.UnsubscribeEvents();
        }

        private void OnDying(DyingEventArgs ev)
        {
            if (!Check(ev.Player)) return;
            Log.Debug("cassie message");

            Exiled.API.Features.Cassie.CustomScpTermination("SCP 0 3 5", ev.DamageHandler);
            
        }

        protected override void RoleAdded(Player player)
        {

            player.VoiceChannel = VoiceChat.VoiceChatChannel.ScpChat;
            player.Position = RoleTypeId.Scp049.GetRandomSpawnLocation().Position;
            base.RoleAdded(player);
        }


        public static readonly string CantPickup = "A strange force called \'game balance\' \nprevents you from picking up this item";
        public static readonly string CantUse = "A strange force called \'game balance\' \nprevents you from using this item";

        private void OnUsingItem(UsingItemEventArgs ev)
        {
            if (!Check(ev.Player)) return;

            if (BlacklistedUsing.Contains(ev.Item.Type))
            {
                ShowEffectHint(ev.Player, CantUse);
                ev.IsAllowed = false;
                return;
            }
        }

        private void OnSearchingPickup(SearchingPickupEventArgs ev)
        {
            if (!Check(ev.Player)) return;

            CustomItem item = null;

            CustomItem.TryGet(ev.Pickup, out item);


            if(item is not null)
            {
                if(item.Id == 1050 || item.Id == 1047)
                {
                    ShowEffectHint(ev.Player, CantPickup);
                    ev.IsAllowed = false;
                    return;
                }

                if (ev.Pickup.Type.IsScp())
                {
                    ShowEffectHint(ev.Player, CantPickup);
                    ev.IsAllowed = false;
                    return;
                }

                if (ev.Pickup.Type == ItemType.GunSCP127)
                {
                    ShowEffectHint(ev.Player, CantPickup);
                    ev.IsAllowed = false;
                    return;
                }
            }








            if (BlacklistedPickup.Contains(ev.Pickup.Type))
            {
                ShowEffectHint(ev.Player, CantPickup);
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
