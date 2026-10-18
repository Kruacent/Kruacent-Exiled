using Exiled.API.Features;
using HarmonyLib;
using InventorySystem.Items.ThrowableProjectiles;
using KE.Utils.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YamlDotNet.Core.Tokens;

namespace KruacentExiled.Patches
{
    public static class PlayerPatches
    {
        [HarmonyPatch(typeof(Player), nameof(Player.CustomInfo), MethodType.Setter)]
        public static class CustomInfoPatches
        {
            public static void Prefix(Player __instance,ref string value)
            {
                KELog.Debug($"changed cinfo {__instance.CustomInfo} to {value}");
                if (!NicknameSync.ValidateCustomInfo(value, out var rejectionText))
                {
                    Log.Error("invalid custominfo : " + rejectionText);
                    value = Regex.Replace(value, @"[^\p{L}\p{P}\p{Sc}\p{N} ^=+|~`<>\n]", "");
                }
            }
        }
    }
}
