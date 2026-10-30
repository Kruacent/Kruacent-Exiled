using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp0492;
using Exiled.Events.EventArgs.Scp096;
using Exiled.Events.EventArgs.Scp1509;
using Exiled.Events.EventArgs.Scp173;
using HintServiceMeow.Core.Models.Arguments;
using InventorySystem.Items.Firearms.Attachments;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.API.Interfaces;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using LabApi.Events.Arguments.PlayerEvents;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VoiceChat.Networking;

namespace KruacentExiled.CustomRoles.CR.CustomSCPs
{
    public class SCP035 : CustomSCP, INoPickupItem 
    {
        public override bool IsSupport => false;

        public const string SCPName = "SCP-035";

        public const string CantUseTranslation = "SCP035CantUse";
        public string NoPickupTranslationKey => Name + "_NoPickup";
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = SCPName,
                    [TranslationKeyDesc] = "Kill every humans!\nYou can't pick up the Micro-HID and anything made with it, but you take 3 time less damage by these weapon.",
                    [NoPickupTranslationKey] = "A strange force called 'game balance' \nprevents you from picking up this item.",
                    [CantUseTranslation] = "A strange force called 'game balance' \nprevents you from using this item.",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = SCPName,
                    [TranslationKeyDesc] = "Tue tous les humains!\nTu peux pas prendre d'arme spécial mais tu prends 3 fois moins de dégât de ces armes",
                    [NoPickupTranslationKey] = "Une force étrange qui s'appelle 'équilibre' \nt'empêches de prendre cet objet.",
                    [CantUseTranslation] = "Une force étrange qui s'appelle 'équilibre' \nt'empêches d'utiliser cet objet.",
                },
                ["legacy"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = SCPName,
                    [TranslationKeyDesc] = "You can't pickup the Micro-HID and anything made with it, but you take 3 time less damage by these weapon.\nKill every humans",
                    [NoPickupTranslationKey] = "A strange force called \'game balance\' \nprevents you from picking up this item",
                    [CantUseTranslation] = "A strange force called 'game balance' \nprevents you from using this item.",
                }
            };
        }

        public override int MaxHealth { get; set; } = 1600;
        protected override int SettingId => 10002;

        public override string SCPId => SCPName;

        public override RoleTypeId Role { get; set; } = RoleTypeId.Tutorial;

        // 035 can't pickup these items
        public List<string> BannedItem { get; set; } = new List<string>()
        {
            $"{ItemType.Jailbird}",
            $"{ItemType.ParticleDisruptor}",
            $"{ItemType.MicroHID}",
            $"{ItemType.Painkillers}",
            $"{ItemType.Medkit}",
            $"{ItemType.GunFRMG0}",
            $"{ItemType.GunAK}",
            $"{ItemType.GunShotgun}",
            $"{ItemType.GunSCP127}",
            $"{ItemType.GunE11SR}",

            "ExplosiveProofArmor",
            "FaradayArmor",
            "FlowerStick",
            "FriendMaker",
            "OverseerProtocolsDocument",
            "SCP-1650",
            "SCP-3136",
            "SCP-3136"
        };

        public List<string> WhitelistItem { get; set; } = new List<string>()
        {
            $"{ItemType.SCP330}",
            $"{ItemType.SCP1853}",
            $"{ItemType.SCP1509}"
        };

        public bool BlockScpItems { get; set; } = true;
        public bool BlockRevivingItems { get; set; } = true;
        public bool BlockViolentItems { get; set; } = false;

        public HashSet<ItemType> BlacklistedUsing = new HashSet<ItemType>()
        {
            ItemType.Painkillers,
            ItemType.Medkit,
            ItemType.SCP500,
        };

        // 035 can't be damaged much by these
        public HashSet<DamageType> BlacklistedDamage = new HashSet<DamageType>()
        {
            DamageType.Jailbird,
            DamageType.ParticleDisruptor,
            DamageType.MicroHid
        };

        protected override void SubscribeEvents()
        {
            _intercom = HashSetPool<Player>.Pool.Get();
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Player.Dying += OnDying;
            Exiled.Events.Handlers.Scp1509.Resurrecting += OnResurrecting;
            Exiled.Events.Handlers.Player.UsingItem += OnUsingItem;
            Exiled.Events.Handlers.Player.VoiceChatting += OnVoiceChatting;
            Exiled.Events.Handlers.Scp096.AddingTarget += OnAddingTarget;
            Exiled.Events.Handlers.Scp173.AddingObserver += OnAddingObserver;
            LabApi.Events.Handlers.PlayerEvents.UsingIntercom += OnUsingIntercom;
            LabApi.Events.Handlers.PlayerEvents.UsedIntercom += OnUsedIntercom;
            Exiled.Events.Handlers.Scp0492.TriggeringBloodlust += OnTriggeringBloodlust;
            base.SubscribeEvents();
        }

     

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            Exiled.Events.Handlers.Scp1509.Resurrecting -= OnResurrecting;
            Exiled.Events.Handlers.Player.UsingItem -= OnUsingItem;
            Exiled.Events.Handlers.Scp096.AddingTarget -= OnAddingTarget;
            Exiled.Events.Handlers.Scp173.AddingObserver -= OnAddingObserver;
            LabApi.Events.Handlers.PlayerEvents.UsingIntercom -= OnUsingIntercom;
            LabApi.Events.Handlers.PlayerEvents.UsedIntercom -= OnUsedIntercom;
            Exiled.Events.Handlers.Scp0492.TriggeringBloodlust -= OnTriggeringBloodlust;
            HashSetPool<Player>.Pool.Return(_intercom);
            base.UnsubscribeEvents();
        }


        private void OnTriggeringBloodlust(TriggeringBloodlustEventArgs ev)
        {
            if (ev.IsAllowed && Check(ev.Target))
            {
                ev.IsAllowed = false;
            }
        }

        private static HintPosition position = new RemainingPlayerPosition();
        private static HintPosition logoposition = new LogoPosition();
        private void OnDying(DyingEventArgs ev)
        {
            if (!Check(ev.Player)) return;
            Log.Debug("cassie message");
            Exiled.API.Features.Cassie.CustomScpTermination("SCP 0 3 5", ev.DamageHandler);
            
        }


        private HashSet<Player> _intercom;

        private void OnUsingIntercom(PlayerUsingIntercomEventArgs ev)
        {
            if (Check(ev.Player))
            {
                _intercom.Add(ev.Player);
                Log.Warn("using intercom");
            }
        }
        private void OnUsedIntercom(PlayerUsedIntercomEventArgs ev)
        {
            if (Check(ev.Player))
            {
                _intercom.Remove(ev.Player);
                Log.Warn("used intercom");
            }
        }
        

        private void OnAddingObserver(AddingObserverEventArgs ev)
        {
            if (!ev.IsAllowed) return;
            if (Check(ev.Observer))
            {
                ev.IsAllowed = false;
            }
        }


        private void OnAddingTarget(AddingTargetEventArgs ev)
        {
            if (!ev.IsAllowed) return;
            if (Check(ev.Target))
            {
                ev.IsAllowed = false;
            }
        }

        private void OnVoiceChatting(VoiceChattingEventArgs ev)
        {
            Player player = ev.Player;

            VoiceMessage msg = ev.VoiceMessage;

            //Log.Info("msg.Channel =" + msg.Channel);

            if (msg.Channel == VoiceChat.VoiceChatChannel.ScpChat)
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
                HashSet<ReferenceHub> receiver;
                if (_intercom.Contains(player))
                {
                    msg.Channel = VoiceChat.VoiceChatChannel.Intercom;
                    receiver = ReferenceHub.AllHubs;
                }
                else
                {
                    msg.Channel = VoiceChat.VoiceChatChannel.RoundSummary;
                    receiver = SCPTeam.SCPs.ToHashSet();
                }
                foreach (ReferenceHub hub in receiver)
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
            DisplayHandler.Instance.CreateAuto(player, (args) => GetPlayers(args), position.HintPlacement,HintServiceMeow.Core.Enum.HintSyncSpeed.Normal);
            DisplayHandler.Instance.CreateAuto(player, (args) => GetLogo(args), logoposition.HintPlacement,HintServiceMeow.Core.Enum.HintSyncSpeed.Normal);
            player.Position = RoleTypeId.Scp049.GetRandomSpawnLocation().Position;
            player.EnableEffect<CustomPlayerEffects.NightVision>(100, 0, false);
            base.RoleAdded(player);
        }

        protected override void RoleRemoved(Player player)
        {
            DisplayHandler.Instance.RemoveHint(player, position.HintPlacement);
            DisplayHandler.Instance.RemoveHint(player, logoposition.HintPlacement);

            player.DisableEffect<CustomPlayerEffects.NightVision>();
            base.RoleRemoved(player);
        }
        protected override void GiveInventory(Player player)
        {
            Firearm firearm = player.AddItem(ItemType.GunFSP9) as Firearm;
            uint code = AttachmentsServerHandler.ServerGetReceivedPlayerPreference(firearm.Base);
            firearm.Base.ApplyAttachmentsCode(code, reValidate: true);
            player.AddItem(ItemType.ArmorLight);
            player.AddItem(ItemType.KeycardGuard);

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


                ShowEffectHint(player, GetTranslation(player, CantUseTranslation));
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

                if (ev.Attacker != null && ev.Attacker.Role.Side == Side.Scp)
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
    }
}
