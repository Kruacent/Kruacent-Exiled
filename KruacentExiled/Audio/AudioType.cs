using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.Audio
{
    public class AudioType
    {

        private HashSet<AudioPlayer> players;

        public SoundType SoundType { get; }


        public AudioType(SoundType type)
        {
            SoundType = type;
            players = new HashSet<AudioPlayer>();
        }


        public void TryAdd(AudioPlayer player,SoundType type)
        {
            if(type == SoundType)
            {
                players.Add(player);
            }
        }

        public void TryChangeVolume(float newVolume, SoundType type)
        {
            if(type == SoundType)
            {
                foreach (AudioPlayer player in players)
                {
                    foreach (AudioClipPlayback clip in player.ClipsById.Values)
                    {
                        clip.Volume = newVolume;
                    }
                }
            }
        }

        public void DestroyAudioPlayer(AudioPlayer player)
        {
            foreach (AudioPlayer audio in players.ToList())
            {
                if (audio == player)
                {
                    players.Remove(player);
                }
            }
        }

    }
}
