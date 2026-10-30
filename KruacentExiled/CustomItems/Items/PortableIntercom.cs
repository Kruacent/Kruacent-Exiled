using Exiled.API.Features;
using Exiled.API.Features.Spawn;
using Exiled.Events.EventArgs.Player;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Interface;
using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace KruacentExiled.CustomItems.Items
{
    public class PortableIntercom : KECustomItem, IViolentItem, ILumosItem
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Portable Intercom",
                    [TranslationKeyDesc] = "You can talk at the intercom",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Intercom Portable",
                    [TranslationKeyDesc] = "Tu peux parler à Intercom",
                },
            };
        }

        public override ItemType ItemType => ItemType.Radio;
        public Color Color { get; set; } = UnityEngine.Color.green;
        public override string Name { get; set; } = "PortableIntercom";
        public override float Weight { get; set; } = 0.5f;
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
            RoomSpawnPoints = new List<RoomSpawnPoint>
            {
                new RoomSpawnPoint()
                {
                    Room = Exiled.API.Enums.RoomType.LczCafe,
                    Offset = new UnityEngine.Vector3(1f, 0f, 1f),
                    Chance = 100
                }
            },
        };

        public bool IsViolent => false;

        private float SpeachDuration = 10f;
        private float InternalCooldown = 90f;
        private Dictionary<Player, float> _cooldowns = new Dictionary<Player, float>();
        private Dictionary<Player, CoroutineHandle> _talkingCoroutines = new Dictionary<Player, CoroutineHandle>();
        private Dictionary<Player, float> _startUsingItemTime = new Dictionary<Player, float>();

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            Exiled.Events.Handlers.Player.VoiceChatting += OnTalking;
            Exiled.Events.Handlers.Player.TogglingRadio += OnTogglingRadio;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            Exiled.Events.Handlers.Player.VoiceChatting -= OnTalking;
            Exiled.Events.Handlers.Player.TogglingRadio -= OnTogglingRadio;
        }

        private void OnTogglingRadio(TogglingRadioEventArgs ev)
        {
            if (!Check(ev.Player)) return;
            ev.IsAllowed = false;
            KECustomItem.ItemEffectHint(ev.Player, "Ceci n'est pas une radio mais un intercom portable");
        }

        protected void OnTalking(VoiceChattingEventArgs ev)
        {
            if (!Check(ev.Player)) return;

            if (_talkingCoroutines.ContainsKey(ev.Player))
            {
                if (Time.time - _startUsingItemTime[ev.Player] >= SpeachDuration)
                {
                    StopIntercom(ev.Player);
                    KECustomItem.ItemEffectHint(ev.Player, "Ohhh tu parles trop là");
                    return;
                }

                Timing.KillCoroutines(_talkingCoroutines[ev.Player]);
                _talkingCoroutines[ev.Player] = Timing.CallDelayed(0.35f, () => StopIntercom(ev.Player));
                return;
            }

            if (_cooldowns.TryGetValue(ev.Player, out float lastUsedTime))
            {
                float elapsed = Time.time - lastUsedTime;
                if (elapsed < InternalCooldown)
                {
                    float remaining = Mathf.Ceil(InternalCooldown - elapsed);
                    KECustomItem.ItemEffectHint(ev.Player, $"Il reste {remaining}s avant de pouvoir réutiliser l'intercom portable");
                    return;
                }
            }

            if (Intercom.InUse)
            {
                KECustomItem.ItemEffectHint(ev.Player, "Intercom est déjà en cours d'utilisation");
                return;
            }

            _startUsingItemTime[ev.Player] = Time.time;

            Intercom.PlaySound(true);
            Intercom.TrySetOverride(ev.Player, true);

            _talkingCoroutines[ev.Player] = Timing.CallDelayed(InternalCooldown, () => StopIntercom(ev.Player));
        }

        private void StopIntercom(Player player)
        {
            if (player != null && player.IsConnected)
            {
                Intercom.TrySetOverride(player, false);
                _cooldowns[player] = Time.time;
            }

            if (_talkingCoroutines.TryGetValue(player, out CoroutineHandle handle))
            {
                Timing.KillCoroutines(handle);
                _talkingCoroutines.Remove(player);
            }

            if (_startUsingItemTime.ContainsKey(player))
            {
                _startUsingItemTime.Remove(player);
            }
        }
    }
}