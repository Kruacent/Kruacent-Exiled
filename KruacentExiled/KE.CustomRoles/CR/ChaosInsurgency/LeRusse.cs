using AdminToys;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.CustomRoles.API.Features;
using MEC;
using Mirror;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VoiceChat.Codec;
using VoiceChat.Networking;

namespace KE.CustomRoles.CR.ChaosInsurgency
{
    public class Russe : KECustomRole
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Russian",
                    [TranslationKeyDesc] = "RUSH B or A, i dont rember sooo good luck",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Russe",
                    [TranslationKeyDesc] = "RUSH B or A, i dont rember sooo good luck",
                },
                ["legacy"] = new()
                {
                    [TranslationKeyName] = "Russe",
                    [TranslationKeyDesc] = "RUSH B or A, i dont rember sooo good luck",
                }
            };
        }

        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.ChaosRifleman;

        public float DamageToLootbox { get; set; } = 500f;

        private readonly Dictionary<Player, float> _playerDamage = new();

        private static object[] _lootPool = null;

        public override List<string> Inventory { get; set; } = new()
        {
            $"{ItemType.GunRevolver}", $"{ItemType.GunA7}", $"{ItemType.ArmorHeavy}",
            $"{ItemType.GrenadeHE}", $"{ItemType.GrenadeFlash}", $"{ItemType.Radio}",
            $"{ItemType.KeycardChaosInsurgency}"
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new()
        {
            { AmmoType.Ammo44Cal, 19 }, { AmmoType.Nato762, 60 }
        };


        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting += OnDealingDamage;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnDealingDamage;
            base.UnsubscribeEvents();
        }


        protected override void RoleAdded(Player player)
        {
            _playerDamage[player] = 0f;
            
        }

        protected override void RoleRemoved(Player player)
        {

            
            _playerDamage.Remove(player);
        }

        private void OnDealingDamage(HurtingEventArgs ev)
        {
            if (ev.Attacker == null || !Check(ev.Attacker) || ev.Player == ev.Attacker) return;

            if (!_playerDamage.ContainsKey(ev.Attacker)) _playerDamage[ev.Attacker] = 0f;
            _playerDamage[ev.Attacker] += ev.Amount;

            if (_playerDamage[ev.Attacker] >= DamageToLootbox)
            {
                _playerDamage[ev.Attacker] = 0f;
                SpawnLootBox(ev.Player.Position, ev.Attacker);
            }
        }

        private void SpawnLootBox(Vector3 pos, Player owner)
        {
            Primitive box = Primitive.Create(PrimitiveType.Cube, pos + Vector3.up * 0.5f, Vector3.zero, new Vector3(0.5f, 0.5f, 0.5f), true);
            box.Color = Color.black;
            Timing.RunCoroutine(LootBoxAnimation(box, owner));
        }

        private IEnumerator<float> LootBoxAnimation(Primitive box, Player p)
        {
            Exiled.API.Features.Toys.Light boxLight = Exiled.API.Features.Toys.Light.Create(box.Position + Vector3.up * 0.5f, Vector3.zero, Vector3.one, true);
            boxLight.Intensity = 40f;
            boxLight.Range = 8f;

            Color[] csRarities = { Color.blue, Color.magenta, Color.red };

            try
            {
                for (int i = 0; i < 10; i++)
                {
                    if (box == null) yield break;

                    Color color = csRarities[Random.Range(0, csRarities.Length)];
                    box.Color = color;
                    boxLight.Color = color;

                    yield return Timing.WaitForSeconds(0.2f);
                }

                object reward = GetRandomGambleReward();

                if (reward is CustomItem custom)
                {
                    custom.Spawn(box.Position);
                }
                else if (reward is ItemType baseItem)
                {
                    Item.Create(baseItem).CreatePickup(box.Position);
                }
            }
            finally
            {
                if (box != null) box.Destroy();
                Timing.CallDelayed(0.5f, () => { if (boxLight != null) boxLight.Destroy(); });
            }
        }

        
        private object GetRandomGambleReward()
        {
            if (_lootPool == null)
            {
                List<object> tempPool = new List<object>();

                foreach (ItemType item in System.Enum.GetValues(typeof(ItemType)))
                {
                    if (item == ItemType.None || item == ItemType.MicroHID || item == ItemType.Jailbird ||
                        item == ItemType.ParticleDisruptor || item == ItemType.GunSCP127) continue;

                    if (item.ToString().Contains("Ammo")) continue;

                    tempPool.Add(item);
                }

                if (CustomItem.Registered != null)
                {
                    foreach (var custom in CustomItem.Registered)
                    {
                        tempPool.Add(custom);
                    }
                }

                _lootPool = tempPool.ToArray();
            }

            return _lootPool[Random.Range(0, _lootPool.Length)];
        }
    }
}