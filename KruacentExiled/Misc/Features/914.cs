using Exiled.Events.EventArgs.Scp914;
using KE.Misc.Features._914Upgrades;

namespace KruacentExiled.Misc.Features
{
    internal class _914 : LoadingMiscFeature<Base914Upgrade>
    {

        public override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Scp914.UpgradingPlayer += InternalUpgradingPlayer;
            base.SubscribeEvents();
        }

        public override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Scp914.UpgradingPlayer -= InternalUpgradingPlayer;
            base.UnsubscribeEvents();
        }



        internal void InternalUpgradingPlayer(UpgradingPlayerEventArgs ev)
        {
            if (!ev.IsAllowed)
            {
                return;
            }


            foreach(Base914Upgrade feature in _allLoadedFeatures)
            {
                bool flag = feature.InternalUpgradingPlayer(ev);
                if (flag)
                {
                    break;
                }
            }


        }


    }
}
