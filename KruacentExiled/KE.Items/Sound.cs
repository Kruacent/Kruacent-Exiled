

using Exiled.API.Features;
using System.Collections.Generic;
using UnityEngine;

namespace KE.Items
{
    internal class Sound
    {
        private AudioPlayer audioPlayer;
        private Dictionary<string, string> _soundName = new Dictionary<string, string>();

        public Sound()
        {
            _soundName.Add("lego.ogg", "build");
            _soundName.Add("worms.ogg", "worms");
        }

        ~Sound()
        {
            audioPlayer.RemoveAllClips();
        }

        public void LoadClips()
        {
            foreach (var s in _soundName)
            {
                Log.Info($"Loading clip ({s.Value}) at {MainPlugin.Instance.Config.SoundLocation}\\{s.Key} ");
                AudioClipStorage.LoadClip($"{MainPlugin.Instance.Config.SoundLocation}\\{s.Key}", s.Value);
            }            
        }



        internal void PlayClip(string clipName, UnityEngine.Vector3 pos, float volume =1f, float maxDistance = 20f)
        {
            Log.Debug($"playing {clipName} at {pos}");
            audioPlayer = AudioPlayer.CreateOrGet($"{clipName} ({pos})", onIntialCreation: (p) =>
            {
                GameObject a = new GameObject();
                a.transform.position = pos;
                p.transform.parent = a.transform;
                Speaker speaker = p.AddSpeaker("Main", isSpatial: true, maxDistance: maxDistance, minDistance: 5f);
                speaker.transform.parent = a.transform;
                speaker.transform.localPosition = Vector3.zero;
            });

            audioPlayer.AddClip(clipName, volume:volume);
        }


        internal void PlayClip(string clipName, GameObject objectEmittingSound, float volume = 1f,float maxDistance = 20f)
        {
            Log.Debug($"playing {clipName} at {objectEmittingSound}");
            audioPlayer = AudioPlayer.CreateOrGet($"{clipName} ({objectEmittingSound})", onIntialCreation: (p) =>
            {

                p.transform.parent = objectEmittingSound.transform;
                Speaker speaker = p.AddSpeaker("Main", isSpatial: true, maxDistance: maxDistance, minDistance: 5f);
                speaker.transform.parent = objectEmittingSound.transform;
                speaker.transform.localPosition = Vector3.zero;
            });

            audioPlayer.AddClip(clipName, volume: volume);
        }
        


    }
}
