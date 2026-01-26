using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.Events.EventArgs.Player;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using MEC;
using Mirror;
using PlayerRoles;
using System;
using System.Collections.Generic;
using UnityEngine;
using VoiceChat;
using VoiceChat.Codec;
using VoiceChat.Codec.Enums;
using VoiceChat.Networking;

namespace KE.CustomRoles.CR.ChaosInsurgency
{
    public class Russe : KECustomRole, IColor
    {
        public override string Description { get; set; } = "Tu dois faire la roulette russe avec les autres joueurs";
        public override string InternalName => "Russe";
        public override string PublicName { get; set; } = "Le Russe";
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.ChaosRifleman;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public Color32 Color => new(255, 0, 0, 0);

        public override float SpawnChance { get; set; } = 100;
        public override Vector3 Scale { get; set; } = new Vector3(1.1f, 1f, 1.1f);

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            $"{ItemType.GunA7}",
            $"{ItemType.ArmorCombat}",
            $"{ItemType.GunRevolver}",
            $"{ItemType.Radio}",
            $"{ItemType.Adrenaline}",
            $"{ItemType.KeycardChaosInsurgency}",
            $"{ItemType.GrenadeHE}",
            $"{ItemType.GrenadeHE}",
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Ammo44Cal, 18},
            { AmmoType.Nato762, 120}
        };

        private OpusDecoder _decoder = new OpusDecoder();
        private OpusEncoder _encoder = new OpusEncoder(OpusApplicationType.Voip);

        private bool DebugMode = false;
        private float _filterMem = 0f;

        private Dictionary<Player, Npc> ActiveDummies = new Dictionary<Player, Npc>();

        protected override void RoleAdded(Player player)
        {
            //Exiled.Events.Handlers.Player.VoiceChatting += OnVoiceChatting;
            if (DebugMode) CreateDummy(player);
        }

        protected override void RoleRemoved(Player player)
        {
            //Exiled.Events.Handlers.Player.VoiceChatting -= OnVoiceChatting;

            if (DebugMode) RemoveDummy(player);
        }

        private void OnVoiceChatting(VoiceChattingEventArgs ev)
        {
            if (!Check(ev.Player)) return;
            ev.IsAllowed = false;

            if (!ActiveDummies.ContainsKey(ev.Player)) return;

            float[] pcmBuffer = new float[1920];
            int decodedLen = _decoder.Decode(ev.VoiceMessage.Data, ev.VoiceMessage.DataLength, pcmBuffer);
            ProcessRusseEffect(pcmBuffer, decodedLen);

            byte[] encodedBytes = new byte[4000];
            int encodedLen = _encoder.Encode(pcmBuffer, encodedBytes);

            byte[] finalPacket = new byte[encodedLen];
            Array.Copy(encodedBytes, finalPacket, encodedLen);

            Npc dummy = ActiveDummies[ev.Player];
            dummy.Position = ev.Player.Position;

            var msg = new VoiceMessage(dummy.ReferenceHub, VoiceChatChannel.Proximity, finalPacket, encodedLen, false);

            foreach (Player hub in Player.List)
            {
                if (!DebugMode && hub == ev.Player) continue;

                if (Vector3.Distance(hub.Position, ev.Player.Position) < 20f)
                {
                    hub.Connection.Send(msg);
                }
            }
        }
        private void ProcessRusseEffect(float[] buffer, int length)
        {
            float gateThreshold = 0.02f;

            float muffle = 0.10f;

            for (int i = 0; i < length; i++)
            {
                float input = buffer[i];

                if (Mathf.Abs(input) < gateThreshold)
                {
                    _filterMem *= 0.9f;
                    buffer[i] = _filterMem;
                    continue;
                }

                _filterMem += (input - _filterMem) * muffle;

                float output = _filterMem * 6.0f;

                if (output > 1.0f) output = 1.0f;
                else if (output < -1.0f) output = -1.0f;

                buffer[i] = output;
            }
        }

        private void CreateDummy(Player p)
        {
            if (ActiveDummies.ContainsKey(p)) return;
            Npc npc = Npc.Spawn($"Russe-{p.Nickname}", RoleTypeId.Tutorial, false, p.Position);
            npc.Scale = Vector3.zero;

            Timing.CallDelayed(0.5f, () =>
            {
                if (npc != null && npc.GameObject != null)
                {
                    try { npc.IsGodModeEnabled = true; } catch { }
                }
            });
            ActiveDummies[p] = npc;
        }

        private void RemoveDummy(Player p)
        {
            if (ActiveDummies.ContainsKey(p))
            {
                if (ActiveDummies[p] != null) NetworkServer.Destroy(ActiveDummies[p].GameObject);
                ActiveDummies.Remove(p);
            }
        }
    }
}
