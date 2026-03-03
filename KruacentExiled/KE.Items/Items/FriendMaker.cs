using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.Events.EventArgs.Player;
using HintServiceMeow.UI.Utilities;
using KE.Items.API.Features;
using KE.Utils.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Items.Items
{

    public class FriendMaker : KECustomWeapon
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Friend Maker™",
                    [TranslationKeyDesc] = "The number one (1) method to make friends",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Friend Maker™",
                    [TranslationKeyDesc] = "LA méthode pour se faire des amis ! <size=5>Produit non remboursable</size>",
                },
            };
        }
        
        public override ItemType ItemType => ItemType.GunCOM15;
        public override string Name { get; set; } = "FriendMaker";
        public override string Description { get; set; } = "";
        public override float Weight { get; set; } = 1f;
        public override SpawnProperties SpawnProperties { get; set; } = null;


        public override byte ClipSize { get; } = 1;
        
        private Dictionary<Player, DateTime> cooldowns;

        private TimeSpan Cooldown = new(0,1,0);

        protected override void SubscribeEvents()
        {
            cooldowns = new();
            base.SubscribeEvents();
        }
        protected override void UnsubscribeEvents()
        {
            cooldowns = null;
            base.UnsubscribeEvents();
        }


        protected override void OnShooting(ShootingEventArgs ev)
        {

            KELog.Debug("firne dmaker");
            Player player = ev.Player;
            if (!Check(player)) return;
            if (!ev.IsAllowed) return;
            if (!CheckCooldown(player))
            {
                DateTime dateTime = cooldowns[player] + Cooldown;
                KECustomItem.ItemEffectHint(player, "You must wait " + Math.Round((dateTime - DateTime.Now).TotalSeconds) + " seconds before using it again");
                ev.IsAllowed = false;
            }


        }


        protected override void OnHurting(HurtingEventArgs ev)
        {
            Player attacker = ev.Attacker;
            Player player = ev.Player;
            if (!Check(attacker)) return;
            ev.IsAllowed = false;


            if (!CheckCooldown(attacker))
            {
                return;
            }

            

            if(Convert(ev.Player, ev.Attacker))
            {
                cooldowns[ev.Attacker] = DateTime.Now;
            }


        }


        private bool CheckCooldown(Player player)
        {
            if (cooldowns.TryGetValue(player, out DateTime time))
            {
                if (DateTime.Now >= time + Cooldown)
                {
                    return true;
                }
                return false;
            }
            cooldowns[player] = DateTime.Now;
            return true;

        }


        private bool Convert(Player player,Player attacker)
        {
            if (player == null)
            {
                KECustomItem.ItemEffectHint(attacker, "But nobody's here");
                return false;
            }

            if (attacker.Role.Side == player.Role.Side)
            {
                KECustomItem.ItemEffectHint(attacker, "I know you don't like them but they're in your team");
                return false;
            }


            if (player.IsScp && player.Role != RoleTypeId.Scp0492)
            {
                KECustomItem.ItemEffectHint(attacker, "That ain't a zombie");
                return false;
            }

            if (player.IsScp)
            {
                player.Role.Set(attacker.Role, RoleSpawnFlags.AssignInventory);
            }
            else
            {
                player.Role.Set(attacker.Role, RoleSpawnFlags.None);
            }

            KECustomItem.ItemEffectHint(attacker, "New friend acquired!");
            return true;
        }



    }
}
