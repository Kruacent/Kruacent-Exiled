using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using HarmonyLib;
using KE.Utils.API.Displays.DisplayMeow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.Patches
{
    public static class ActiveAbilityPatches
    {
        [HarmonyPatch(typeof(ActiveAbility),nameof(ActiveAbility.UseAbility))]
        public static class UseAbilityPatch
        {
            public static void Prefix(ActiveAbility __instance, Player player)
            {
                string msg = "Using "+__instance.Name;

                DisplayHandler.Instance.AddHint(MainPlugin.Abilities, player, msg, 5);
            }
        }
        [HarmonyPatch(typeof(ActiveAbility), nameof(ActiveAbility.SelectAbility))]
        public static class SelectAbilityPatch
        {
            public static void Prefix(ActiveAbility __instance, Player player)
            {
                string msg = __instance.Name + "\n" + __instance.Description;

                DisplayHandler.Instance.AddHint(MainPlugin.Abilities, player, msg, 5);
            }
        }



    }
}
