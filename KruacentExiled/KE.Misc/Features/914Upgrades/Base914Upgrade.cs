using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp914;
using KE.Utils.API.Features;
using KE.Utils.API.Interfaces;
using UnityEngine;


namespace KE.Misc.Features._914Upgrades
{
    public abstract class Base914Upgrade : IUsingEvents
    {

        protected abstract float Chance { get; }

        public virtual void SubscribeEvents()
        {
            Exiled.Events.Handlers.Scp914.UpgradingPlayer += InternalUpgradingPlayer;
        }

        public virtual void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Scp914.UpgradingPlayer -= InternalUpgradingPlayer;
        }

        private void InternalUpgradingPlayer(UpgradingPlayerEventArgs ev)
        {
            if (!ev.IsAllowed) return;
            if (!LuckCheck()) return;
            OnUpgradingPlayer(ev);
        }


        /// <summary>
        /// Auto check the probability with the <see cref="Chance"/> and if it's allowed
        /// </summary>
        protected virtual void OnUpgradingPlayer(UpgradingPlayerEventArgs ev)
        {

        }

        protected bool LuckCheck()
        {
            return UnityEngine.Random.Range(0f, 100f) < Mathf.Clamp(Chance, 0f, 100f);
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
