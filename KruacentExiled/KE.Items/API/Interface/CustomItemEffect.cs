using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KE.Items.API.Interface
{
    public abstract class CustomItemEffect
    {
        public abstract void Effect(UsedItemEventArgs ev);

        public abstract void Effect(ExplodingGrenadeEventArgs ev);

        public abstract void Effect(DroppingItemEventArgs ev);

    }
}
