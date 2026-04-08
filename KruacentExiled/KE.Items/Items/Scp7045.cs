namespace KE.Items.Items
{
    //[CustomItem(ItemType.SCP1576)]
    /*[Obsolete("Scrapped - Can't make a good sound quality without external file")]
    public class Scp7045 : KECustomItem
    {
        /*
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
            base.SubscribeEvents();


        }
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsedItem -= OnUsedItem;
            Exiled.Events.Handlers.Player.VoiceChatting -= OnVoiceChatting;
            base.UnsubscribeEvents();
        }

        private bool _isItemActive = false;

        private void OnUsedItem(UsedItemEventArgs ev)
        {
            if (!Check(ev.Item)) return;

            Scp1576 item = (Scp1576)ev.Item;
            item.StopTransmitting();

            if (!_isItemActive)
            {
                _isItemActive = true;
                _recordingBuffer.Clear();
                _isRecording = true;
                _lastVoiceTime = Time.time;
                Log.Info("Recording");

                Timing.RunCoroutine(CheckIfRecordingFinished(item, ev.Player));
            }
            else
            {
                _isItemActive = false;
                _isRecording = false;
                Log.Info("Playing");

                Log.Info("buffer : " + _recordingBuffer.ToArray().Count());
                if (_recordingBuffer.Count > 0)
                {
                    Speaker speaker;
                    byte speakerid = (byte)ev.Player.Id;


                    if (!_speakers.TryGetValue(ev.Player, out Speaker _))
                        _speakers[ev.Player] = Speaker.Create(speakerid, ev.Player.Position);

                    _speakers[ev.Player].Position = ev.Player.Position;

                    Timing.RunCoroutine(PlayVoice([.. _recordingBuffer], (byte)ev.Player.Id, ev.Player));
                }

                item.StopTransmitting();

            }
        }



        public const int sampleSize = 480;

        private List<byte> _recordingBuffer = new();
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
                _recordingBuffer.AddRange(message.Data);

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
                    Log.Info($"Final recorded voice message length: {_recordingBuffer.Count} samples.");

                    float[] finalRecording = _recordingBuffer.ToArray();
                    Log.Info($"Final recorded voice message length: {finalRecording.Length} samples.");

                    //Timing.RunCoroutine(PlayVoice(finalRecording, speaker.Base.NetworkControllerId));
                }
            }

            TeleportItem(item, p);
        }

        private IEnumerator<float> PlayVoice(byte[] data, byte speakerid, Player player)
        {
            Log.Info("message for " +player.Nickname);
            foreach (var chunk in SplitData(data, 512))
            {
                var message = new AudioMessage(speakerid, chunk, chunk.Length);
                player.ReferenceHub.connectionToClient.Send(message);
                yield return Timing.WaitForSeconds(0.1f);
            }

            Log.Info("end message");

        }
        private IEnumerable<byte[]> SplitData(byte[] data, int chunkSize)
        {
            for (int i = 0; i < data.Length; i += chunkSize)
            {
                int size = Math.Min(chunkSize, data.Length - i);
                byte[] chunk = new byte[size];
                Array.Copy(data, i, chunk, 0, size);
                yield return chunk;
            }
            //78336
        }


        private void TeleportItem(Scp1576 item, Player p)
        {
            Player nextPlayer = Player.List.Where(x => x.IsHuman && x != p).GetRandomValue();

            Log.Info("NextPlayer  : " + nextPlayer);

            Room closeRoom = FindNearbyRoom(nextPlayer, 2);

            Log.Info("Room  : " + closeRoom);



            Spawn(closeRoom.Position, p);
            p.RemoveItem(item, false);
        }

        private Room FindNearbyRoom(Player p, int depth)
        {
            if (p == null) return Room.Random();

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
        */
}