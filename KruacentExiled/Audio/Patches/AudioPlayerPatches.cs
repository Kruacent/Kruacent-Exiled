using HarmonyLib;
using InventorySystem.Items.ThrowableProjectiles;
using KE.Utils.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Audio.Patches
{
    public static class AudioPlayerPatches
    {

        [HarmonyPatch(typeof(AudioPlayer), "SendAudioData")]
        public static class SendAudioDataPatch
        {
            public static void Prefix(AudioPlayer __instance)
            {
                if (__instance.ClipsById.Count == 0)
                {
                    foreach (string speaker in __instance.SpeakersByName.Keys)
                    {
                        __instance.RemoveSpeaker(speaker);
                        KELog.Debug($"destroyed speaker ({speaker}) from {__instance.Name}");
                    }
                    MainPlugin.AudioHandler.DestroyAudioPlayer(__instance);
                }
            }
        }

        [HarmonyPatch(typeof(AudioPlayer), nameof(AudioPlayer.Destroy))]
        public static class DestroyPatch
        {
            public static void Postfix(AudioPlayer __instance)
            {
                MainPlugin.AudioHandler.DestroyAudioPlayer(__instance);
            }
        }


    }
}
