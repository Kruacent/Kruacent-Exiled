using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Utils.API.Sounds
{
    public class SoundPlayer
    {
        private static SoundPlayer _instance;

        public static SoundPlayer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new();
                }
                return _instance;
            }
        }

        private SoundPlayer() { }

        private bool _loaded = false;
        public bool Loaded => _loaded;

        public static readonly HashSet<string> clips = new();

        public static string SoundLocation => Paths.Configs + "/Sounds";
        public static void Load() => Instance.TryLoad();
        public void TryLoad()
        {
            if (Loaded)
            {
                return;
            }

            if (!Directory.Exists(SoundLocation))
            {
                Log.Warn("Directory not found. creating...");
                Directory.CreateDirectory(SoundLocation);
            }

            string[] rawfile = Directory.GetFiles(SoundLocation, "*.ogg");

            foreach (string file in rawfile)
            {
                string noExFile = Path.GetFileNameWithoutExtension(file);
                Log.Info($"loading {file} as {noExFile}");
                clips.Add(noExFile);
                AudioClipStorage.LoadClip(file);
            }
            _loaded = true;
        }


        /// <summary>
        /// Play a clip at a static point
        /// </summary>
        /// <param name="clipName"></param>
        /// <param name="pos"></param>
        /// <param name="volume"></param>
        /// <param name="maxDistance"></param>
        public AudioClipPlayback Play(string clipName, Vector3 pos, float volume = 50f, float maxDistance = 20f, bool isSpatial = true)
        {
            if (!Loaded) throw new Exception("clips not loaded use SoundPlayer.Load()");
            Log.Debug($"playing {clipName} at {pos}");

            var audioPlayer = AudioPlayer.CreateOrGet($"{clipName} ({pos})", onIntialCreation: (p) =>
            {
                GameObject a = new GameObject();
                a.transform.position = pos;
                p.transform.parent = a.transform;
                Speaker speaker = p.AddSpeaker("main", isSpatial: isSpatial, maxDistance: maxDistance, minDistance: 1f);
                speaker.transform.parent = a.transform;
                speaker.transform.localPosition = Vector3.zero;
            });

            audioPlayer.AddClip(clipName, volume: volume);
            audioPlayer.DestroyWhenAllClipsPlayed = true;
            return audioPlayer.AddClip(clipName, volume: volume);


        }

        /// <summary>
        /// Play a clip at a <see cref="GameObject"/>
        /// </summary>
        /// <param name="clipName"></param>
        /// <param name="pos"></param>
        /// <param name="volume"></param>
        /// <param name="maxDistance"></param>
        public void Play(string clipName, GameObject objectEmittingSound, float volume = 50f, float maxDistance = 20f, bool isSpatial = true)
        {
            if (!Loaded) throw new Exception("clips not loaded use SoundPlayer.Instance.Load()");
            Log.Debug($"playing {clipName} at {objectEmittingSound}");

            var audioPlayer = AudioPlayer.CreateOrGet($"{clipName} ({objectEmittingSound})", onIntialCreation: (p) =>
            {

                p.transform.parent = objectEmittingSound.transform;
                Speaker speaker = p.AddSpeaker("main", isSpatial: isSpatial, maxDistance: maxDistance, minDistance: 1f);
                speaker.transform.parent = objectEmittingSound.transform;
                speaker.transform.localPosition = Vector3.zero;
            });
            audioPlayer.AddClip(clipName, volume: volume);
            audioPlayer.DestroyWhenAllClipsPlayed = true;
        }
    }
}
