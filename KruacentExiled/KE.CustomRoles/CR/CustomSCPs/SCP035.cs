using Cassie;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Pools;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp1509;
using Exiled.Events.EventArgs.Server;
using HintServiceMeow.Core.Models.Arguments;
using HintServiceMeow.Core.Utilities;
using KE.CustomRoles.API.Features;
using KE.Items.API.Features;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using KE.Utils.API.Features.SCPs;
using MEC;
using PlayerRoles;
using PlayerRoles.FirstPersonControl.Thirdperson;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VoiceChat.Networking;

namespace KE.CustomRoles.CR.CustomSCPs
{
    public class SCP035 : CustomSCP
    {
        public override bool IsSupport => false;
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "SCP-035",
                    [TranslationKeyDesc] = "Kill every humans!\nYou can't pick up the Micro-HID and anything made with it, but you take 3 time less damage by these weapon.",
                    ["SCP035CantPickup"] = "A strange force called 'game balance' \nprevents you from picking up this item.",
                    ["SCP035CantUse"] = "A strange force called 'game balance' \nprevents you from using this item.",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "SCP-035",
                    [TranslationKeyDesc] = "Tue tous les humains!\nTu peux pas prendre d'arme spécial mais tu prends 3 fois moins de dégât de ces armes",
                    ["SCP035CantPickup"] = "Une force étrange qui s'appelle 'équilibre' \nt'empêches de prendre cet objet.",
                    ["SCP035CantUse"] = "Une force étrange qui s'appelle 'équilibre' \nt'empêches d'utiliser cet objet.",
                },
                ["legacy"] = new()
                {
                    [TranslationKeyName] = "SCP-035",
                    [TranslationKeyDesc] = "You can't pickup the Micro-HID and anything made with it, but you take 3 time less damage by these weapon.\nKill every humans",
                    ["SCP035CantPickup"] = "A strange force called \'game balance\' \nprevents you from picking up this item",
                    ["SCP035CantUse"] = "A strange force called 'game balance' \nprevents you from using this item.",
                }
            };
        }

        public override int MaxHealth { get; set; } = 1200;
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
        public HashSet<ItemType> WhitelistUsing = new()
        {
            ItemType.SCP330,
            ItemType.SCP1853,
            ItemType.SCP1509,
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
            Exiled.Events.Handlers.Player.VoiceChatting += OnVoiceChatting;

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
        private static HintPosition position = new RemainingPlayerPosition();
        private static HintPosition logoposition = new LogoPosition();
        private void OnDying(DyingEventArgs ev)
        {
            if (!Check(ev.Player)) return;
            Log.Debug("cassie message");

            Exiled.API.Features.Cassie.CustomScpTermination("SCP 0 3 5", ev.DamageHandler);
            
        }

        private void OnVoiceChatting(VoiceChattingEventArgs ev)
        {
            Player player = ev.Player;

            
            VoiceMessage msg = ev.VoiceMessage;

            if(msg.Channel == VoiceChat.VoiceChatChannel.ScpChat)
            {
                msg.Channel = VoiceChat.VoiceChatChannel.RoundSummary;

                foreach (Player scp035 in Player.List)
                {
                    if (scp035 != player && Check(scp035))
                    {
                        scp035.Connection.Send(msg);
                    }
                }
            }



            if (Check(player))
            {
                msg.Channel = VoiceChat.VoiceChatChannel.RoundSummary;

                foreach (ReferenceHub hub in SCPTeam.SCPs)
                {
                    if (hub != player.ReferenceHub)
                    {
                        hub.connectionToClient.Send(msg);
                    }
                    
                }
                ev.IsAllowed = false;
            }
        }

        protected override void RoleAdded(Player player)
        {
            PlayerDisplay dis = PlayerDisplay.Get(player);
            DisplayHandler.Instance.CreateAuto(player, (args) => GetPlayers(args), position.HintPlacement,HintServiceMeow.Core.Enum.HintSyncSpeed.Normal);
            DisplayHandler.Instance.CreateAuto(player, (args) => GetLogo(args), logoposition.HintPlacement,HintServiceMeow.Core.Enum.HintSyncSpeed.Normal);





            player.Position = RoleTypeId.Scp049.GetRandomSpawnLocation().Position;
            player.EnableEffect<NightVision>(100, 0, false);
            base.RoleAdded(player);
        }

        protected override void RoleRemoved(Player player)
        {
            DisplayHandler.Instance.RemoveHint(player, position.HintPlacement);
            DisplayHandler.Instance.RemoveHint(player, logoposition.HintPlacement);

            player.DisableEffect<NightVision>();
            base.RoleRemoved(player);
        }


        


        private string GetPlayers(AutoContentUpdateArg arg)
        {

            if (!Check(Player.Get(arg.PlayerDisplay.ReferenceHub))) 
                return string.Empty;
            return "<size=50><b>" + Mathf.Clamp(RoundSummary.singleton.TargetCount,0,9).ToString() + "</b></size>";
            //<size=50><b>👤8</b></size>
        }

        private string GetLogo(AutoContentUpdateArg arg)
        {

            if (!Check(Player.Get(arg.PlayerDisplay.ReferenceHub)))
                return string.Empty;
            return "<size=50><b>👤</b></size>";
        }





        private void OnUsingItem(UsingItemEventArgs ev)
        {
            Player player = ev.Player;
            if (!Check(player)) return;

            if (BlacklistedUsing.Contains(ev.Item.Type))
            {


                ShowEffectHint(player, GetTranslation(player, "SCP035CantUse"));
                ev.IsAllowed = false;
                return;
            }
        }

        private void OnSearchingPickup(SearchingPickupEventArgs ev)
        {
            Player player = ev.Player;
            Pickup pickup = ev.Pickup;
            if (!Check(ev.Player)) return;



            CustomItem item = null;

            CustomItem.TryGet(pickup, out item);


            if(item is not null)
            {
                if(item.Id == 1050 || item.Id == 1047)
                {
                    ShowEffectHint(player, GetTranslation(player, "SCP035CantPickup"));
                    ev.IsAllowed = false;
                    return;
                }
            }


            if (pickup.Type.IsScp() && !WhitelistUsing.Contains(pickup.Type))
            {
                ShowEffectHint(player, GetTranslation(player, "SCP035CantPickup"));
                ev.IsAllowed = false;
                return;
            }

            if (pickup.Type == ItemType.GunSCP127)
            {
                ShowEffectHint(player, GetTranslation(player, "SCP035CantPickup"));
                ev.IsAllowed = false;
                return;
            }





            if (BlacklistedPickup.Contains(pickup.Type))
            {
                ShowEffectHint(player, GetTranslation(player, "SCP035CantPickup"));
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
                        ev.DamageHandler.Damage /= 3;
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
            Timing.CallDelayed(1, () => ev.Target.Health = 100);
        }

        public void OnEndingRound(EndingRoundEventArgs ev)
        {
            /*if (TrackedPlayers.Count <= 0) return;

            if (ev.ClassList.mtf_and_guards != 0 || ev.ClassList.scientists != 0) ev.IsAllowed = false;
            else if (ev.ClassList.class_ds != 0 || ev.ClassList.chaos_insurgents != 0) ev.IsAllowed = false;
            else if (ev.ClassList.scps_except_zombies + ev.ClassList.zombies > 0) ev.IsAllowed = true;
            else ev.IsAllowed = true;*/
        }
    }
}
