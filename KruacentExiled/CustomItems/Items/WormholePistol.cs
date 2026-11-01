using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Spawn;
using Exiled.Events.EventArgs.Player;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Interface;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.CustomItems.Items
{
    public class WormholePistol : KECustomWeapon, IViolentItem
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Wormhole Pistol",
                    [TranslationKeyDesc] = "Change position with the target you shot at, 'MERICA!",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Pistolet TP",
                    [TranslationKeyDesc] = "Tire et POUF, tu change de position avec la cible",
                },
            };
        }

        public override ItemType ItemType => ItemType.GunCOM18;
        public override string Name { get; set; } = "WormholePistol";
        public override float Weight { get; set; } = 1f;
        public override float Damage { get; set; } = 1f;
        public override byte ClipSize { get; } = 2;
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties
        {
            Limit = 1,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new DynamicSpawnPoint()
                {
                    Chance = 25,
                    Location = SpawnLocationType.InsideLczWc,
                },
                new DynamicSpawnPoint()
                {
                    Chance = 25,
                    Location = SpawnLocationType.Inside939Cryo,
                },
                new DynamicSpawnPoint()
                {
                    Chance = 25,
                    Location = SpawnLocationType.InsideHidLab,
                },
                new DynamicSpawnPoint()
                {
                    Chance = 25,
                    Location = SpawnLocationType.InsideIntercom,
                }
            }
        };

        public bool IsViolent => true;

        protected override void OnHurting(HurtingEventArgs ev)
        {
            Player attacker = ev.Attacker;

            Player target = ev.Player;

            if (attacker.Role is FpcRole attRole && attRole.IsGrounded)
            {
                Vector3 attackerPos = attacker.Position;

                Vector3 targetPos = target.Position;

                Timing.CallDelayed(0.1f, () => attacker.Position = targetPos); 

                Timing.CallDelayed(0.1f, () => target.Position = attackerPos);
                
            }
            
        }

    }
}
