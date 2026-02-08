using KE.CustomRoles.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.Events.Handlers
{
    public static class KECustomRole
    {

        public static event Action<ReceivingCustomRoleEventArgs> ReceivingCustomRole = delegate { };
        public static event Action<ReceivedCustomRoleEventArgs> ReceivedCustomRole = delegate { };


        public static void OnReceivingCustomRole(ReceivingCustomRoleEventArgs ev)
        {
            ReceivingCustomRole?.Invoke(ev);
        }
        public static void OnReceivedCustomRole(ReceivedCustomRoleEventArgs ev)
        {
            ReceivedCustomRole?.Invoke(ev);
        }

    }
}
