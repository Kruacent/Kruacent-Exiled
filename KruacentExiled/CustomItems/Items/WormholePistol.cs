using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Spawn;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Features;
using KruacentExiled.CustomItems.API.Enum;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Interface;
using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KruacentExiled.CustomItems.Items
{
    public class WormholePistol : KECustomWeapon, IViolentItem, ILumosItem
    {
        public const string TranslationCooldown = "WormholePistolCooldown";
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Wormhole Pistol",
                    [TranslationKeyDesc] = "Change position with the target you shot at, 'MERICA!",
                    [TranslationCooldown] = "You must wait %second% seconds before using it again",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Pistolet TP",
                    [TranslationKeyDesc] = "Tire et POUF, tu change de position avec la cible",
                    [TranslationCooldown] = "Tu dois attendre %second% secondes avant de pouvoir l'utiliser à nouveau",
                },
            };
        }

        public override ItemRank Rank => ItemRank.Epic;
        public override ItemType ItemType => ItemType.GunCOM18;
        public override string Name { get; set; } = "WormholePistol";
        public override float Weight { get; set; } = 1f;
        public override float Damage { get; set; } = 1f;
        public override byte ClipSize { get; } = 2;
        private TimeSpan Cooldown = new TimeSpan(0, 1, 0);
        private bool _isTimeSet = false;
        private DateTime time;
        public bool IsViolent => true;
        protected int NbShot = 0;
        public UnityEngine.Color Color { get; set; } = UnityEngine.Color.magenta;
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties
        {
            Limit = 1,
            RoomSpawnPoints = new List<RoomSpawnPoint>
            {
                new RoomSpawnPoint()
                {
                    Chance = 25,
                    Room = RoomType.LczPlants,
                },
                new RoomSpawnPoint()
                {
                    Chance = 25,
                    Room = RoomType.HczNuke,
                },
                new RoomSpawnPoint()
                {
                    Chance = 25,
                    Room = RoomType.Hcz939,
                },
                new RoomSpawnPoint()
                {
                    Chance = 25,
                    Room = RoomType.EzShelter,
                }
            }
        };

        protected override void OnHurting(HurtingEventArgs ev)
        {
            if (!ev.IsAllowed) return;
            Player attacker = ev.Attacker;
            Player target = ev.Player;

            if (attacker.Role is FpcRole attRole && attRole.IsGrounded)
            {
                Vector3 attackerPos = attacker.Position;

                Vector3 targetPos = target.Position;

                if ((attacker != null && target != null) && (attacker.IsAlive && target.IsAlive))
                {
                    Timing.CallDelayed(0.1f, () => attacker.Position = targetPos);

                    Timing.CallDelayed(0.1f, () => target.Position = attackerPos);
                }
                
            }

            base.OnHurting(ev);
            
        }

        protected override void OnShooting(ShootingEventArgs ev)
        {
            if (!ev.IsAllowed) return;


            KELog.Debug("shot =" + NbShot);
            KELog.Debug("now =" + DateTime.Now);
            KELog.Debug("next posible =" + (time + Cooldown));

            if (CheckCooldown())
            {


                if (NbShot < 2)
                {
                    NbShot++;
                }
                else
                {
                    time = DateTime.Now;
                    ev.IsAllowed = false;
                    KELog.Debug("no shot");
                }
            }
            else
            {
                ev.IsAllowed = false;
                string msg = GetTranslation(ev.Player, TranslationCooldown).Replace("%second%", Math.Round((Cooldown - (DateTime.Now-time)).TotalSeconds).ToString());
                ItemEffectHint(ev.Player, msg);
            }
            
            
        }

        private bool CheckCooldown()
        {
            bool canShot = false;

            if (!_isTimeSet)
            {
                time = DateTime.Now;
                canShot = true;
                _isTimeSet = true;
            }
            else if(NbShot < 2)
            {
                canShot = true;
            }
            else if (DateTime.Now >= time + Cooldown)
            {
                canShot = true;
                NbShot = 0;
                time = DateTime.Now;
            }
            return canShot;
        }

    }
}
