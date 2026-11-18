using Exiled.API.Features;
using KE.Utils.API.Features;
using KE.Utils.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Misc.Features
{
    public abstract class MiscFeature : IUsingEvents
    {
        private static HashSet<MiscFeature> _list = new HashSet<MiscFeature>();


        internal MiscFeature()
        {
            _list.Add(this);
        }

        internal static void SubscribeAllEvents()
        {
            foreach (MiscFeature f in _list)
            {
                try
                {
                    f.SubscribeEvents();
                }catch(Exception e)
                {
                    Log.Error(e);
                }
                
            }
                
        }

        internal static void UnsubscribeAllEvents()
        {
            foreach (MiscFeature f in _list)
            {
                try
                {
                    f.UnsubscribeEvents();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
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
