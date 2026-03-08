using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp914;
using KE.Utils.API.Features;
using KE.Utils.API.Interfaces;
using UnityEngine;


namespace KE.Misc.Features._914Upgrades
{
    public abstract class Base914Upgrade
    {

        protected abstract float Chance { get; }




        internal bool InternalUpgradingPlayer(UpgradingPlayerEventArgs ev)
        {
            if (!LuckCheck()) return false;
            return OnUpgradingPlayer(ev);
        }


        /// <summary>
        /// Auto check the probability with the <see cref="Chance"/> and if it's allowed
        /// </summary>
        protected abstract bool OnUpgradingPlayer(UpgradingPlayerEventArgs ev);

        public bool LuckCheck()
        {
            return LuckCheck(Chance);
        }

        /// <summary>
        /// Check luck with a different value than <see cref="Chance"/>
        /// </summary>
        /// <returns>true if it passed the luck check ; false otherwise</returns>
        protected bool LuckCheck(float chance)
        {
            float wanted = Mathf.Clamp(chance, 0f, 100f);
            float random = UnityEngine.Random.Range(0f, 100f);

            KELog.Debug($"{random} < {wanted} : {random < wanted} ");

            return random < wanted ;
        }

    }
}
