using HarmonyLib;
using PlayerRoles.PlayableScps.HumanTracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Misc.Patches
{
    public static class LastHumanTrackerPatches
    {
        [HarmonyPatch(typeof(LastHumanTracker), nameof(LastHumanTracker.LateUpdate))]
        public static class LateUpdatePatch
        {

            public static bool Prefix()
            {
                return false;
            }

        }
    }
}
