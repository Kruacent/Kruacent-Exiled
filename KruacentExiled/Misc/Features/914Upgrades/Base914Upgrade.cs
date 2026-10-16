using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp914;
using KE.Utils.API.Features;
using KE.Utils.API.Interfaces;
using KruacentExiled.Misc.Utils;
using UnityEngine;


namespace KE.Misc.Features._914Upgrades
{
    public abstract class Base914Upgrade
    {

        protected abstract float Chance { get; }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="ev"></param>
        /// <returns></returns>
        internal bool InternalUpgradingPlayer(UpgradingPlayerEventArgs ev)
        {
            if (!LuckCheck(ev.Player)) return false;
            return OnUpgradingPlayer(ev);
        }


        /// <summary>
        /// Auto check the probability with the <see cref="Chance"/> and if it's allowed
        /// </summary>
        /// 
        protected abstract bool OnUpgradingPlayer(UpgradingPlayerEventArgs ev);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player></param>
        /// <returns></returns>
        public bool LuckCheck(Player player)
        {
            return LuckCheck(Chance, player);
        }

        /// <summary>
        /// Check luck with a different value than <see cref="Chance"/>
        /// </summary>
        /// <param name="player>The player need to be had the chance</param>
        /// <returns>true if it passed the luck check ; false otherwise</returns>
        protected bool LuckCheck(float chance, Player player)
        {
            float wanted = Mathf.Clamp(chance, 0f, 100f);

            bool luckCheck = RandomNumberGenerator.RollChance(wanted, player, true);

            KELog.Debug($"{luckCheck}");

            return luckCheck ;
        }

    }
}
