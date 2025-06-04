using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoiceChat.Codec;
using VoiceChat.Codec.Enums;
using VoiceChat.Networking;

namespace KE.Items.Items
{
    [CustomItem(ItemType.SCP1576)]
    public class Scp7045 : KECustomItem
    {

        private static readonly OpusDecoder _decoder = new();
        private static readonly OpusEncoder _encoder = new(OpusApplicationType.Voip);
        private static readonly Dictionary<Player, Speaker> _speakers = new();
        public override uint Id { get; set; } = 1800;
        public override string Name { get; set; } = "SCP-7045";
        public override string Description { get; set; } = "A weird looking radio";
        public override float Weight { get; set; } = 0.65f;

        private AudioMessage _message;
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
            LockerSpawnPoints = new List<LockerSpawnPoint>()
            {
                new LockerSpawnPoint()
                {
                    Type = LockerType.Scp1576Pedestal,
                    UseChamber = true,
                    Chance = .5f
                }
            }
        };

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsedItem += OnUsedItem;
            Exiled.Events.Handlers.Player.VoiceChatting += OnVoiceChatting;


        }
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsedItem -= OnUsedItem;
            Exiled.Events.Handlers.Player.VoiceChatting -= OnVoiceChatting;
        }

        private bool _isItemActive = false;

        private void OnUsedItem(UsedItemEventArgs ev)
        {
            if (!Check(ev.Item)) return;

            Scp1576 item = (Scp1576)ev.Item;

            if (!_isItemActive)
            {
                _isItemActive = true;
                _recordingBuffer.Clear();
                _isRecording = true;
                _lastVoiceTime = Time.time;
                Log.Info("Enregistrement activé...");

                Timing.RunCoroutine(CheckIfRecordingFinished(item, ev.Player));
            }
            else
            {
                _isItemActive = false;
                _isRecording = false;
                Log.Info("Enregistrement terminé.");

                Log.Info("Recording buffer : " + _recordingBuffer.ToArray().Count());
                if (_recordingBuffer.Count > 0)
                {
                    Speaker speaker;
                    byte speakerid = (byte)ev.Player.Id;

                    if (!_speakers.TryGetValue(ev.Player, out speaker))
                        _speakers[ev.Player] = Speaker.Create(speakerid, ev.Player.Position);

                    float[] finalRecording = _recordingBuffer.ToArray();
                    Timing.RunCoroutine(PlayVoice(finalRecording, (byte)ev.Player.Id, item, ev.Player));
                }

                item.StopTransmitting();

            }
        }



        public const int sampleSize = 480;

        private List<float> _recordingBuffer = new();
        private bool _isRecording = false;
        private float _lastVoiceTime = 0f; // Timestamp of last voice packet
        private void OnVoiceChatting(VoiceChattingEventArgs ev)
        {
            if (!_isItemActive || !Check(ev.Player.CurrentItem)) return;

            Speaker speaker;
            byte speakerid = (byte)ev.Player.Id;
            if (!_speakers.TryGetValue(ev.Player, out speaker))
            {
                _speakers[ev.Player] = Speaker.Create(speakerid, ev.Player.Position);
            }
            speaker = _speakers[ev.Player];

            VoiceMessage message = ev.VoiceMessage;

            // Si on est en mode enregistrement et que l'item est activé, on enregistre la voix
            if (_isRecording)
            {
                float[] decodedBuffer = new float[sampleSize];
                _decoder.Decode(message.Data, message.DataLength, decodedBuffer);
                _recordingBuffer.AddRange(decodedBuffer);

                // Mettre à jour le timestamp de la dernière voix reçue
                _lastVoiceTime = Time.time;
            }
        }

        private IEnumerator<float> CheckIfRecordingFinished(Scp1576 item, Player p)
        {
            while (_isRecording)
            {
                // Wait a short time before checking again
                yield return Timing.WaitForSeconds(0.2f);

                // If no voice data has been received for 0.5s, stop recording
                if (Time.time - _lastVoiceTime >= 0.5f)
                {
                    _isRecording = false;

                    float[] finalRecording = _recordingBuffer.ToArray();
                    Log.Info($"Final recorded voice message length: {finalRecording.Length} samples.");

                    //Timing.RunCoroutine(PlayVoice(finalRecording, speaker.Base.NetworkControllerId));
                }
            }

            TeleportItem(item, p);
        }

        private IEnumerator<float> PlayVoice(float[] data, byte speakerid, Scp1576 item, Player p)
        {
            for (int i = 0; i < data.Length; i += sampleSize)
            {
                byte[] encodedData = new byte[512];
                float[] decodedBuffer = data.Skip(i).Take(sampleSize).ToArray();
                int dataLen = _encoder.Encode(decodedBuffer, encodedData);
                _message = new AudioMessage(speakerid, encodedData, dataLen);
                foreach (Player player in Player.List)
                    player.ReferenceHub.connectionToClient.Send(_message);
                yield return Timing.WaitForOneFrame;
            }

            item.CreatePickup(Room.Random().Position, null, true);
            p.RemoveItem(item, false);
        }

        private void TeleportItem(Scp1576 item, Player p)
        {
            Player nextPlayer = Player.List.Where(x => x.IsHuman && x != p).GetRandomValue();

            Log.Info("NextPlayer  : " + nextPlayer);

            Room closeRoom = findNearbyRoom(nextPlayer, 2);

            Log.Info("Room  : " + closeRoom);

            item.CreatePickup(closeRoom.Position, null, true);

            Log.Info("PickupCreated");
            p.RemoveItem(item, false);
        }

        private Room findNearbyRoom(Player p, int depth)
        {
            HashSet<Room> rooms = new HashSet<Room>(p.CurrentRoom.NearestRooms);
            for (int i = 0; i < depth; i++)
            {
                // Copier les nouvelles rooms avant de les ajouter
                HashSet<Room> newRooms = new HashSet<Room>(rooms.SelectMany(r => r.NearestRooms));
                rooms.UnionWith(newRooms);
            }

            // Si rooms est vide, on retourne une salle aléatoire
            return rooms.Count > 0 ? rooms.GetRandomValue() : Room.Random();
        }
    }
}