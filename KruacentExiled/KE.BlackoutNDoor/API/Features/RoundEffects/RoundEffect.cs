using Exiled.API.Enums;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.BlackoutNDoor.API.Features.RoundEffects
{
    public abstract class RoundEffect
    {
        /// <summary>
        /// How Cassie will announce the message
        /// </summary>
        public string EventTranslation { get; protected set; }
        public IReadOnlyDictionary<ZoneType,int> Chances { get; protected set; }
        public abstract void Effect(ZoneType zone);
        public abstract void StopEffect(ZoneType zone);


        internal ZoneType SelectZone()
        {
            int totalWeight = 0;
            foreach (var weight in Chances.Values)
            {
                totalWeight += weight;
            }

            if (totalWeight == 0)
            {
                Log.Error("Total probability must be greater than zero.");
                return ZoneType.Unspecified;
            }

            int randValue = UnityEngine.Random.Range(0, totalWeight);
            int cumulativeSum = 0;

            foreach (var entry in Chances)
            {
                cumulativeSum += entry.Value;
                if (randValue < cumulativeSum)
                    return entry.Key;
            }

            Log.Error("Zone selection failed");
            return ZoneType.Unspecified;
        }
    }
}
