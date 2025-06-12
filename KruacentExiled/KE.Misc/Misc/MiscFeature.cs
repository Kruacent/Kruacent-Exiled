using Exiled.API.Features;
using KE.Utils.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Misc
{
    internal abstract class MiscFeature : IUsingEvents
    {
        private static HashSet<MiscFeature> _list = new();


        internal MiscFeature()
        {
            _list.Add(this);
        }

        internal static void SubscribeAllEvents()
        {
            foreach (MiscFeature f in _list)
            {
                Log.Debug($"subscribing events in {f.GetType()}");
                f.SubscribeEvents();
            }
                
        }

        internal static void UnsubscribeAllEvents()
        {
            foreach (MiscFeature f in _list)
                f.UnsubscribeEvents();
            _list.Clear();
        }


        public virtual void SubscribeEvents()
        {

        }
        public virtual void UnsubscribeEvents()
        {

        }
    }
}
