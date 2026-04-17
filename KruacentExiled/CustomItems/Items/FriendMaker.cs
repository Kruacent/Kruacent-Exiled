using Exiled.API.Features;
using Exiled.API.Features.Spawn;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Features;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Interface;
using PlayerRoles;
using System;
using System.Collections.Generic;

namespace KruacentExiled.CustomItems.Items
{

    public class FriendMaker : KECustomWeapon, IViolentItem
    {

        public const string TranslationCooldown = "FriendMakerCooldown";
        public const string TranslationNobody = "FriendMakerNobody";
        public const string TranslationSameTeam = "FriendMakerSameTeam";
        public const string TranslationNonZombie = "FriendMakerNonZombie";
        public const string TranslationSuccess = "FriendMakerSuccess";
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Friend Maker™",
                    [TranslationKeyDesc] = "The number one (1) method to make friends",
                    [TranslationCooldown] = "You must wait %second% seconds before using it again",
                    [TranslationNobody] = "But nobody came",
                    [TranslationSameTeam] = "I know you don't like them but they're in your team",
                    [TranslationNonZombie] = "That ain't a zombie",
                    [TranslationSuccess] = "New friend acquired!",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Friend Maker™",
                    [TranslationKeyDesc] = "LA méthode pour se faire des amis ! <size=5>Produit non remboursable</size>",
                    [TranslationCooldown] = "Tu dois attendre %second% secondes avant de pouvoir l'utiliser à nouveau",
                    [TranslationNobody] = "Mais personne n'est venu",
                    [TranslationSameTeam] = "Je sais que tu l'aime pas mais il est bien avec toi",
                    [TranslationNonZombie] = "C'est pas un zombie ça",
                    [TranslationSuccess] = "Nouvel ami obtenu!",
                },
            };
        }
        
        public override ItemType ItemType => ItemType.GunCOM15;
        public override string Name { get; set; } = "FriendMaker";
        public override float Weight { get; set; } = 1f;
        public override SpawnProperties SpawnProperties { get; set; } = null;


        public override byte ClipSize { get; } = 1;

        public bool IsViolent => false;

        private Dictionary<Player, DateTime> cooldowns;

        private TimeSpan Cooldown = new TimeSpan(0,1,0);

        protected override void SubscribeEvents()
        {
            cooldowns = new Dictionary<Player, DateTime>();
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


                string msg = GetTranslation(player, TranslationCooldown).Replace("%second%", Math.Round((dateTime - DateTime.Now).TotalSeconds).ToString());
                ItemEffectHint(player, msg);
                ev.IsAllowed = false;
            }


        }


        protected override void OnHurting(HurtingEventArgs ev)
        {
            Player attacker = ev.Attacker;
            Player player = ev.Player;
            if (!Check(attacker)) return;
            ev.IsAllowed = false;


            

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
                TranslationHint(attacker, TranslationNobody);
                return false;
            }

            if (attacker.Role.Side == player.Role.Side)
            {
                TranslationHint(attacker, TranslationSameTeam);
                return false;
            }


            if (player.IsScp && player.Role != RoleTypeId.Scp0492)
            {
                TranslationHint(attacker, TranslationNonZombie);
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
            TranslationHint(attacker, TranslationSuccess);
            return true;
        }



    }
}
