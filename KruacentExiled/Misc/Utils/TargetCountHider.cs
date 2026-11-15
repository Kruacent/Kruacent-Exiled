using Exiled.API.Features;
using HarmonyLib;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using System.Collections.Generic;
using System.Linq;


namespace KruacentExiled.Misc.Utils
{
    /// <summary>
    /// Hide a player from SCP's TargetCount automatically readded if the player is dead or change his role
    /// </summary>
    public static class TargetCountHider
    {
        private static List<int> HiddenLives = new List<int>();

        public static void Hide(Player player)
        {
            if (SCPTeam.IsSCP(player.ReferenceHub)) return;
            RemoveLife();
            HiddenLives.Add(player.Role.Base.UniqueLifeIdentifier);
            RoundSummary.singleton.UpdateTargetCount();
        }

        private static void RemoveLife()
        {
            List<int> livesCopy = HiddenLives.ToList();
            foreach (int lifeIdentifier in livesCopy)
            {
                if (!Player.List.Any(p => p.Role.Base.UniqueLifeIdentifier == lifeIdentifier))
                {
                    HiddenLives.Remove(lifeIdentifier);
                }
            }
        }

        public static void Show(Player player)
        {
            HiddenLives.Remove(player.Role.Base.UniqueLifeIdentifier);
            RoundSummary.singleton.UpdateTargetCount();
        }

        [HarmonyPatch(typeof(RoundSummary), nameof(RoundSummary.UpdateTargetCount))]
        private static class Patch
        {
            private static void Postfix(RoundSummary __instance)
            {
                if (__instance.TargetCount <= 1) return;
                int hiddenPlayer = 0;
                foreach (Player player in Player.List)
                {
                    if (HiddenLives.Contains(player.Role.Base.UniqueLifeIdentifier))
                    {
                        hiddenPlayer++;
                    }
                }

                int visible = __instance.TargetCount - hiddenPlayer;
                if (visible < 1) visible = 1;

                __instance.TargetCount = visible;
            }
        }
    }
}