using InventorySystem.Items.ThrowableProjectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.CustomItems.API.Events
{
    public static class ExplodeEvent
    {

        public static event Action<OnExplodeDestructibleEventsArgs> ExplodeDestructible = delegate { };


        public static void OnExplodeDestructible(OnExplodeDestructibleEventsArgs ev)
        {
            ExplodeDestructible?.Invoke(ev);
        }




    }
}
