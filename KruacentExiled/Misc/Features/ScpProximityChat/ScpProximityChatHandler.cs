using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using KE.Utils.API.Settings.SettingsCategories;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using KruacentExiled.CustomRoles.Settings;
using PlayerRoles;
using PlayerRoles.Voice;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UserSettings.ServerSpecific;
using VoiceChat;
using VoiceChat.Codec;
using VoiceChat.Codec.Enums;
using VoiceChat.Networking;

namespace KruacentExiled.Misc.Features.ScpProximityChat
{
    internal class ScpProximityChatHandler : MiscFeature
    {
        private const int HeaderId = 8000;
        private const int KeybindId = 8001;

        public static ScpProximityChatHandler Instance { get; private set; }

        private readonly HashSet<Player> _enabled = new HashSet<Player>();
        public static HintPosition TextPosition = new ScpProximityChatPosition();

        private readonly Dictionary<Player, ProximityVoice> _voices = new Dictionary<Player, ProximityVoice>();
        private readonly Dictionary<Player, float> _lastToggleTime = new Dictionary<Player, float>();
        private const float _MinimumTimeBetweenTwoToggle = 0.3f;

        private readonly SettingsCategory _settingsCategory;

        private class ProximityVoice
        {
            public AudioPlayer AudioPlayer;
            public OpusDecoder Decoder = new OpusDecoder();
            public OpusEncoder Encoder = new OpusEncoder(OpusApplicationType.Voip);
        }

        public ScpProximityChatHandler()
        {
            Instance = this;

            HeaderSetting header = new HeaderSetting(HeaderId, "SCP Proximity Chat");
            KeybindSetting keybind = new KeybindSetting(KeybindId, "Toggle SCP proximity chat", KeyCode.None, hintDescription: "Lets humans near you hear your voice", header: header);
            _settingsCategory = new SettingsCategory(header, 1006, new List<SettingBase>() { keybind });
        }

        public static bool IsEnabled(Player player)
        {
            return Instance != null && Instance._enabled.Contains(player);
        }

        public override void SubscribeEvents()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += OnKeyPressed;
            Exiled.Events.Handlers.Player.TogglingNoClip += OnTogglingNoClip;
            Exiled.Events.Handlers.Player.VoiceChatting += OnVoiceChatting;
            Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;
            Exiled.Events.Handlers.Player.Left += OnLeft;
            Exiled.Events.Handlers.Server.RestartingRound += DisableAll;
        }

        public override void UnsubscribeEvents()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= OnKeyPressed;
            Exiled.Events.Handlers.Player.TogglingNoClip -= OnTogglingNoClip;
            Exiled.Events.Handlers.Player.VoiceChatting -= OnVoiceChatting;
            Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;
            Exiled.Events.Handlers.Player.Left -= OnLeft;
            Exiled.Events.Handlers.Server.RestartingRound -= DisableAll;
            DisableAll();
        }

        private void OnKeyPressed(ReferenceHub hub, ServerSpecificSettingBase setting)
        {
            try
            {
                if (!SettingHandler.CheckPressed(setting, KeybindId)) return;

                Player player = Player.Get(hub);
                if (!CanUseProximity(player)) return;

                Toggle(player);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private void OnTogglingNoClip(TogglingNoClipEventArgs ev)
        {
            if (!CanUseProximity(ev.Player)) return;

            Toggle(ev.Player);
        }

        private static bool CanUseProximity(Player player)
        {
            if (player == null) return false;
            if (!SCPTeam.IsSCP(player.ReferenceHub)) return false;
            if (player.Role.Type == RoleTypeId.Scp079) return false;

            return true;
        }

        private void Toggle(Player player)
        {
            _lastToggleTime.TryGetValue(player, out float lastTime);

            if (Time.time - lastTime > _MinimumTimeBetweenTwoToggle)
            {
                if (_enabled.Contains(player))
                {
                    Disable(player);
                    ShowHint(player, ScpProximityChatTranslations.Disabled);
                }
                else
                {
                    Enable(player);
                    ShowHint(player, ScpProximityChatTranslations.Enabled);
                }

                _lastToggleTime[player] = Time.time;
            }
        }

        private void Enable(Player player)
        {
            _enabled.Add(player);

            // Not using SCPTeam because that cause a porblem with 035 that is Tutorial
            if (!player.IsScp) return;

            AudioPlayer audioPlayer = AudioPlayer.CreateOrGet($"ScpProximity {player.Id}");
            Speaker speaker = audioPlayer.AddSpeaker("voice",
                isSpatial: true,
                minDistance: MainPlugin.Configs.ScpProximityChatMinDistance,
                maxDistance: MainPlugin.Configs.ScpProximityChatMaxDistance);

            speaker.transform.parent = player.Transform;
            speaker.transform.localPosition = Vector3.zero;

            _voices[player] = new ProximityVoice() { AudioPlayer = audioPlayer };
        }

        private void Disable(Player player)
        {
            _enabled.Remove(player);

            if (_voices.TryGetValue(player, out ProximityVoice voice))
            {
                if (voice.AudioPlayer != null)
                {
                    voice.AudioPlayer.RemoveSpeaker("voice");
                    voice.AudioPlayer.Destroy();
                }

                voice.Decoder.Dispose();
                voice.Encoder.Dispose();
                _voices.Remove(player);
            }
        }

        private void DisableAll()
        {
            foreach (Player player in _enabled.ToList())
            {
                Disable(player);
            }
        }

        private void OnVoiceChatting(VoiceChattingEventArgs ev)
        {
            if (ev.VoiceMessage.Channel != VoiceChatChannel.ScpChat) return;
            if (!_voices.TryGetValue(ev.Player, out ProximityVoice voice)) return;

            float[] samples = new float[480];
            voice.Decoder.Decode(ev.VoiceMessage.Data, ev.VoiceMessage.DataLength, samples);

            for (int i = 0; i < samples.Length; i++)
            {
                samples[i] = Mathf.Clamp(samples[i] * MainPlugin.Configs.ScpProximityChatVolume, -1f, 1f);
            }

            byte[] data = new byte[512];
            int length = voice.Encoder.Encode(samples, data);

            AudioMessage audio = new AudioMessage(voice.AudioPlayer.ControllerID, data, length);

            foreach (Player target in Player.Enumerable)
            {
                if (SCPTeam.IsSCP(target.ReferenceHub)) continue;
                if (!CanHearProximity(target, ev.Player)) continue;

                target.Connection.Send(audio);
            }
        }

        private void OnChangingRole(ChangingRoleEventArgs ev)
        {
            Disable(ev.Player);
            _lastToggleTime.Remove(ev.Player);
        }

        private void OnLeft(LeftEventArgs ev)
        {
            Disable(ev.Player);
            _lastToggleTime.Remove(ev.Player);
        }

        private static bool CanHearProximity(Player listener, Player speaker)
        {
            if (!(listener.ReferenceHub.roleManager.CurrentRole is IVoiceRole voiceRole))
            {
                return false;
            }

            VoiceChatChannel channel = voiceRole.VoiceModule.ValidateReceive(speaker.ReferenceHub, VoiceChatChannel.Proximity);

            if (channel == VoiceChatChannel.None)
            {
                return false;
            }

            return true;
        }

        private static void ShowHint(Player player, string key)
        {
            DisplayHandler.Instance.AddHint(TextPosition.HintPlacement, player, MainPlugin.GetTranslation(player, key), 5);
        }
    }
}
