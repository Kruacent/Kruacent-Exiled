using KE.Utils.API;
using KE.Utils.API.Features;
using KE.Utils.API.Interfaces;
using System.Collections.Generic;

namespace KruacentExiled.Misc.Features
{
    internal class LoadingMiscFeature<T> : MiscFeature
    {
        protected readonly HashSet<T> _allLoadedFeatures;

        internal LoadingMiscFeature() : base()
        {
            _allLoadedFeatures = new HashSet<T>(ReflectionHelper.GetObjects<T>());
        }


        public override void SubscribeEvents()
        {
            foreach(T loaded in _allLoadedFeatures)
            {
                KELog.Debug("subscribing "+ loaded.GetType().Name);
                if(loaded is IUsingEvents iue)
                    iue.SubscribeEvents();
            }
        }

        public override void UnsubscribeEvents()
        {
            foreach (T loaded in _allLoadedFeatures)
            {
                KELog.Debug("Unsubscribing " + loaded.GetType().Name);
                if (loaded is IUsingEvents iue)
                    iue.UnsubscribeEvents();
            }
        }
    }
}
