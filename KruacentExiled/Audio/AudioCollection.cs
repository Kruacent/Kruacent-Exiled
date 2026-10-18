using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Audio
{
    public class AudioCollection
    {
        private HashSet<AudioType> players;

        public AudioCollection()
        {
            players = new HashSet<AudioType>();
            foreach(SoundType type in Enum.GetValues(typeof(SoundType)))
            {
                players.Add(new AudioType(type));
            }
        }


        public void AddPlayer(AudioPlayer player,SoundType type)
        {
            foreach(AudioType audioType in players)
            {
                audioType.TryAdd(player, type);
            }
        }

        public void ChangeVolume(float newVolume, SoundType type)
        {

            foreach (AudioType audioType in players)
            {
                audioType.TryChangeVolume(newVolume, type);
            }
        }


        public void DestroyAudioPlayer(AudioPlayer player)
        {

            foreach (AudioType audioType in players)
            {
                audioType.DestroyAudioPlayer(player);
            }
        }


    }
}
