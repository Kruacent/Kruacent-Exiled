using HarmonyLib;
using PlayerRoles.FirstPersonControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.Patches
{
    public static class FpcMotorPatches
    {
        //me when slowness 200

        //[HarmonyPatch(typeof(FpcMotor),nameof(FpcMotor.DesiredMove),MethodType.Getter)]
        public static class DesiredMovePatch
        {
            [HarmonyPostfix]
            public static void Postfix(FpcMotor __instance,ref Vector3 __result)
            {
                __result = new(__result.x * -1, __result.y, __result.z * -1);
            }


        }

        //[HarmonyPatch(typeof(FpcMotor), nameof(FpcMotor.GetFrameMove), MethodType.Normal)]
        public static class GetFrameMovePatch
        {
            [HarmonyPostfix]
            public static void Postfix(FpcMotor __instance, ref Vector3 __result)
            {
                __result = new(__result.x * -1, __result.y, __result.z * -1);
            }


        }

    }
}
