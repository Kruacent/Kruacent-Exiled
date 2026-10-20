using HarmonyLib;
using KE.Utils.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Audio.Patches
{
    public static class SpeakerPatches
    {
        [HarmonyPatch(typeof(Speaker), "OnDestroy")]
        public static class OnDestroyPatch
        {
            public static void Prefix(Speaker __instance)
            {
                if (MainPlugin.AudioHandler.Speakers.TryGetValue(__instance, out RecyclableId id))
                {
                    id.Destroy();
                }

            }
        }
    }
}
