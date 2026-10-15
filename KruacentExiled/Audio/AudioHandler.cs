using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Features;
using KE.Utils.API.Interfaces;
using KruacentExiled.Interfaces;
using ProjectMER.Commands.Modifying.Position;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using YamlDotNet.Core.Tokens;

namespace KruacentExiled.Audio
{
    public class AudioHandler : IHandler, IUsingEvents
    {
        public bool Debug { get; set; }

        private SettingsHandler settingsHandler;
        public static AudioHandler Instance { get; private set; }
        public AudioHandler(bool debug)
        {
            Debug = debug;
            settingsHandler = new SettingsHandler(this);
            Instance = this;
        }

        public void SubscribeEvents()
        {
            settingsHandler.SubscribeEvents();

        }
        public void UnsubscribeEvents()
        {
            settingsHandler.UnsubscribeEvents();
        }


        private float GetVolume(Player player, SoundType soundType)
        {
            return settingsHandler.GetVolume(player,soundType);
        }



        public Dictionary<Player, AudioCollection> audioPlayers = new Dictionary<Player, AudioCollection>();


        /// <summary>
        /// Change the volume of all <see cref="AudioPlayer"/> for this <see cref="Player"/>
        /// </summary>
        /// <param name="player"></param>
        /// <param name="newVolume"></param>
        /// <param name="type"></param>
        public void ChangeVolume(Player player, float newVolume, SoundType type)
        {
            if (audioPlayers.ContainsKey(player))
            {
                audioPlayers[player].ChangeVolume(newVolume, type);
            }
        }

        /// <summary>
        /// Destroy an <see cref="AudioPlayer"/>
        /// </summary>
        /// <param name="audioPlayer"></param>
        public void DestroyAudioPlayer(AudioPlayer audioPlayer)
        {
            foreach(AudioCollection collection in audioPlayers.Values)
            {
                collection.DestroyAudioPlayer(audioPlayer);
            }
        }


        /// <summary>
        /// Play a clip at a fixed position
        /// </summary>
        /// <param name="players"><see cref="Player"/> who can hear the clip</param>
        /// <param name="soundType"></param>
        /// <param name="clipName"></param>
        /// <param name="position"></param>
        /// <param name="maxDistance"></param>
        /// <returns></returns>
        public Dictionary<Player,AudioClipPlayback> Play(IEnumerable<Player> players, SoundType soundType, string clipName,Vector3 position, float maxDistance = 20)
        {
            Dictionary<Player, AudioClipPlayback> result = new Dictionary<Player, AudioClipPlayback>();
            foreach(Player player in players)
            {
                result.Add(player, Play(player, soundType, clipName, position,maxDistance));
            }

            return result;
        }

        /// <summary>
        /// Play a clip attached to a <see cref="GameObject"/> to all <see cref="Player"/>
        /// </summary>
        /// <param name="soundType"></param>
        /// <param name="clipName"></param>
        /// <param name="gameObject"></param>
        /// <param name="maxDistance"></param>
        /// <returns></returns>
        public Dictionary<Player, AudioClipPlayback> PlayToAll(SoundType soundType, string clipName, GameObject gameObject, float maxDistance = 20)
        {
            return Play(Player.List, soundType, clipName, gameObject, maxDistance);
        }
        /// <summary>
        /// Play a clip at a fixed position to all <see cref="Player"/>
        /// </summary>
        /// <param name="soundType"></param>
        /// <param name="clipName"></param>
        /// <param name="gameObject"></param>
        /// <param name="maxDistance"></param>
        /// <returns></returns>
        public Dictionary<Player, AudioClipPlayback> PlayToAll(SoundType soundType, string clipName, Vector3 position, float maxDistance = 20)
        {
            return Play(Player.List, soundType, clipName, position, maxDistance);
        }

        /// <summary>
        /// Play a clip attached to a <see cref="GameObject"/>
        /// </summary>
        /// <param name="players"><see cref="Player"/> who can hear the clip</param>
        /// <param name="soundType"></param>
        /// <param name="clipName"></param>
        /// <param name="position"></param>
        /// <param name="maxDistance"></param>
        /// <returns></returns>

        public Dictionary<Player, AudioClipPlayback> Play(IEnumerable<Player> players,SoundType soundType,string clipName,GameObject gameObject, float maxDistance = 20)
        {
            Dictionary<Player, AudioClipPlayback> result = new Dictionary<Player, AudioClipPlayback>();
            foreach (Player player in players)
            {
                result.Add(player, Play(player, soundType, clipName, gameObject, maxDistance));
            }

            return result;
        }

        /// <summary>
        /// Play a clip attached to a <see cref="GameObject"/>
        /// </summary>
        /// <param name="player"><see cref="Player"/> who can hear the clip</param>
        /// <param name="soundType"></param>
        /// <param name="clipName"></param>
        /// <param name="gameObject"></param>
        /// <param name="maxDistance"></param>
        /// <returns>return null if the speaker couldn't be created </returns>
        public AudioClipPlayback Play(Player player, SoundType soundType, string clipName, GameObject gameObject, float maxDistance = 20)
        {
            if(gameObject == null)
            {
                Log.Error("go null");
            }
            if (player == null)
            {
                Log.Error("player null");
            }


            Log.Info($"play gameObject ({gameObject.name}) {player.Nickname} [{soundType}] ({clipName})");
            AudioPlayer audioPlayer = CreateOrGet(player, soundType, clipName);
            float volume = GetVolume(player, soundType);

            Speaker speaker = CreateSpeaker(audioPlayer, soundType, volume);

            if (speaker == null) return null;
            speaker.transform.SetParent(gameObject.transform);
            speaker.transform.localPosition = Vector3.zero;
            KELog.Debug($"speaker pos = {speaker.Position}");
            KELog.Debug($"player pos = {player.Position}");


            speaker.MaxDistance = maxDistance;
            speaker.MinDistance = 1f;

            if (!audioPlayers.ContainsKey(player))
            {
                audioPlayers[player] = new AudioCollection();
            }


            audioPlayers[player].AddPlayer(audioPlayer, soundType);

            return audioPlayer.AddClip(clipName);
        }
        /// <summary>
        /// Play a clip at a fixed position to a single <see cref="Player"/>
        /// </summary>
        /// <param name="listener">the <see cref="Player"/> who can hear the clip</param>
        /// <param name="soundType"></param>
        /// <param name="clipName"></param>
        /// <param name="gameObject"></param>
        /// <param name="maxDistance"></param>
        /// <returns>return null if the speaker couldn't be created </returns>
        public AudioClipPlayback Play(Player listener, SoundType soundType,string clipName,Vector3 position, float maxDistance = 20)
        {
            GameObject gameObject = new GameObject();

            if (gameObject == null)
            {
                Log.Error("go null");
            }
            if (listener == null)
            {
                Log.Error("player null");
            }


            Log.Info($"play position ({gameObject.name}) {listener.Nickname} [{soundType}] ({clipName})");
            AudioPlayer audioPlayer = CreateOrGet(listener, soundType, clipName);
            float volume = GetVolume(listener, soundType);

            Speaker speaker = CreateSpeaker(audioPlayer, soundType, volume);

            if (speaker == null) return null;
            speaker.Position = position;


            speaker.MaxDistance = maxDistance;
            speaker.MinDistance = 1f;

            if (!audioPlayers.ContainsKey(listener))
            {
                audioPlayers[listener] = new AudioCollection();
            }


            audioPlayers[listener].AddPlayer(audioPlayer, soundType);

            return audioPlayer.AddClip(clipName);
        }


        /// <summary>
        /// Play a clip attached the <see cref="Player"/> and only the <see cref="Player"/> can hear it
        /// </summary>
        /// <param name="player"></param>
        /// <param name="soundType"></param>
        /// <param name="clipName"></param>
        /// <param name="gameObject"></param>
        /// <param name="maxDistance"></param>
        /// <returns>return null if the speaker couldn't be created </returns>
        public AudioClipPlayback Play(Player player, SoundType soundType, string clipName,float maxDistance = 20)
        {
            return Play(player, soundType, clipName, player.GameObject, maxDistance);
        }

        /// <summary>
        /// All <see cref="Speaker"/> and its ids
        /// </summary>
        internal Dictionary<Speaker, RecyclableId> Speakers = new Dictionary<Speaker, RecyclableId>();

        private Speaker CreateSpeaker(AudioPlayer audioPlayer,SoundType type,float volume)
        {
            RecyclableId id = new RecyclableId();
            

            Speaker speaker = audioPlayer.AddSpeaker(type.ToString() + id, volume);

            if(speaker != null)
            {
                KELog.Debug("creating speaker :" + speaker.Name);
                Speakers.Add(speaker, id);
            }
            else
            {
                id.Destroy();
            }
            

            return speaker;
        }

        private AudioPlayer CreateOrGet(Player player, SoundType soundType, string clipName)
        {
            AudioPlayer audio = AudioPlayer.CreateOrGet($"{soundType} {player.Id} {clipName}");
            audio.DestroyWhenAllClipsPlayed = true;
            audio.Condition = (p) =>
            {
                return player.ReferenceHub == p;
            };



            return audio;
        }

        
    }



}
