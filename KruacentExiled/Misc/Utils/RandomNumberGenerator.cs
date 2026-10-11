using System;
using Exiled.API.Features;
using UnityEngine;
using KruacentExiled.Misc.Events;
using KE.Utils.API.Features;

namespace KruacentExiled.Misc.Utils
{
    public class RandomNumberGenerator
    {
        public static event Action<RollingChanceEventArgs> OnRollingChance;

        public static bool RollChance(float successChance, Player player, bool isBeneficial = true)
        {
            RollingChanceEventArgs ev = new RollingChanceEventArgs(successChance, player, isBeneficial);
            OnRollingChance?.Invoke(ev);


            if (ev.ForcedResult.HasValue)
            {
                KELog.Debug("RollChance Forced Value : " + ev.ForcedResult);
                return ev.ForcedResult.Value;
            }

            float chance = UnityEngine.Random.Range(0f, 100f);

            KELog.Debug("Chance RollChance : " + chance);
            bool result = UnityEngine.Random.Range(0f, 100f) <= ev.Chance;
            KELog.Debug("RollChance result : " + result);

            return result;
        }
    }
}