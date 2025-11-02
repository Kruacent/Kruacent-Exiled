using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Map;
using Footprinting;
using HarmonyLib;
using InventorySystem.Items.ThrowableProjectiles;
using KE.Items.API.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static HarmonyLib.AccessTools;

namespace KE.Items.Patches
{
    public static class ExplosionGrenadePatches
    {

        [HarmonyPatch(typeof(ExplosionGrenade),nameof(ExplosionGrenade.ExplodeDestructible))]
        public static class ExplodeDestructiblePatch
        {
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);

                var argsCtor = AccessTools.Constructor(typeof(OnExplodeDestructibleEventsArgs),
                    new[] { typeof(IDestructible), typeof(float), typeof(ExplosionGrenade) });
                var eventInvoker = AccessTools.Method(typeof(ExplodeEvent), nameof(ExplodeEvent.OnExplodeDestructible));
                var damageGetter = AccessTools.PropertyGetter(typeof(OnExplodeDestructibleEventsArgs), nameof(OnExplodeDestructibleEventsArgs.Damage));

                for (int i = 0; i < codes.Count - 2; i++)
                {
                    // Look for Vector3::op_Addition followed by any stloc.* instruction
                    if (codes[i].Calls(AccessTools.Method(typeof(Vector3), "op_Addition")) &&
                        codes[i + 1].opcode.Name.StartsWith("stloc"))
                    {
                        var injectionIndex = i + 2;

                        codes.InsertRange(injectionIndex, new[]
                        {
                    // Create and fire event
                    new CodeInstruction(OpCodes.Ldarg_0), // dest
                    new CodeInstruction(OpCodes.Ldloc_2), // num
                    new CodeInstruction(OpCodes.Ldarg_3), // setts
                    new CodeInstruction(OpCodes.Newobj, argsCtor),
                    new CodeInstruction(OpCodes.Dup),
                    new CodeInstruction(OpCodes.Call, eventInvoker),
                    new CodeInstruction(OpCodes.Callvirt, damageGetter),
                    new CodeInstruction(OpCodes.Stloc_2),
                });

                        UnityEngine.Debug.Log("[ExplosionPatch] Injected OnPreExplodeDestructible successfully");
                        break;
                    }
                }

                return codes;
            }
        }
    }
}
