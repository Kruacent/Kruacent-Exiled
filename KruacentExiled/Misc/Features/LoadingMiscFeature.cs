using Exiled.API.Features;
using KE.Utils.API;
using KE.Utils.API.Features;
using KE.Utils.API.Interfaces;
using System;
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
                try
                {
                    if (loaded is IUsingEvents iue)
                    {
                        iue.SubscribeEvents();
                    }
                        
                }
                catch(Exception e)
                {
                    Log.Error(e);
                }
                
            }
        }

        public override void UnsubscribeEvents()
        {
            foreach (T loaded in _allLoadedFeatures)
            {
                try
                {
                    if (loaded is IUsingEvents iue)
                    {
                        iue.UnsubscribeEvents();
                    }

                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }
}
