

namespace KE.Items
{
    internal class Sound
    {
        private AudioPlayer audioPlayer;

        public Sound()
        {

        }

        ~Sound()
        {
            audioPlayer.RemoveAllClips();
        }

        public void LoadClips()
        {
            AudioClipStorage.LoadClip("C:\\Users\\Patrique\\AppData\\Roaming\\EXILED\\Plugins\\audio\\lego.ogg", "build");
        }



        internal void PlayClip(string clipName, UnityEngine.Vector3 pos)
        {
            audioPlayer = AudioPlayer.CreateOrGet($"Global AudioPlayer", onIntialCreation: (p) =>
            {
                Speaker speaker = p.AddSpeaker("Main", isSpatial: true, maxDistance: 20f, minDistance: 5f);
                speaker.transform.localPosition = pos;
            });

            audioPlayer.AddClip(clipName, volume:1);
        }
    }
}
