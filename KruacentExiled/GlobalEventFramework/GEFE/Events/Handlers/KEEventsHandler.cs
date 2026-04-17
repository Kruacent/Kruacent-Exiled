using KruacentExiled.GlobalEventFramework.GEFE.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.GlobalEventFramework.GEFE.Events.Handlers
{
    public static class KEEventsHandler
    {



        public static event Action<DisabledEventArgs> Disabled = delegate { };
        public static event Action<EnabledEventArgs> Enabled = delegate { };
        public static event Action<EnablingEventArgs> Enabling = delegate { };


        public static void OnDisabled(DisabledEventArgs ev)
        {
            Disabled?.Invoke(ev);
        }
        public static void OnEnabled(EnabledEventArgs ev)
        {
            Enabled?.Invoke(ev);
        }
        public static void OnEnabling(EnablingEventArgs ev)
        {
            Enabling?.Invoke(ev);
        }



    }
}
