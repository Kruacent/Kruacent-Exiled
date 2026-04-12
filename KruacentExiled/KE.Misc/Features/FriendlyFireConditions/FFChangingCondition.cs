using Exiled.API.Features;
using KE.Utils.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Misc.Features.FriendlyFireConditions
{
    internal abstract class FFChangingCondition : IUsingEvents
    {

        /// <summary>
        /// The chance of triggering a changement from 0f to 100f
        /// </summary>
        protected abstract float Chance { get; }


        /// <summary>
        /// No need to calculate the probability
        /// </summary>
        protected void ChangeFriendlyFire()
        {
            float correctedChance = Mathf.Clamp(Chance, 0f, 100f);
            if (UnityEngine.Random.Range(0f, 100f) < correctedChance)
            {
                Server.FriendlyFire = !Server.FriendlyFire;
            }
        }

        /// <summary>
        /// No need to calculate the probability
        /// </summary>
        protected void ForceFriendlyFireState(bool isFFActive)
        {
            byte correctedChance = (byte)Mathf.Clamp(Chance, 0, 100);
            if (UnityEngine.Random.Range(0f, 100f) < correctedChance)
            {
                Server.FriendlyFire = isFFActive;
            }
        }

        public virtual void SubscribeEvents()
        {

        }

        public virtual void UnsubscribeEvents()
        {

        }


    }
}
