using System;
using System.Linq;
using Exiled.API.Features;

namespace KE.Misc.Features.GamblingCoin
{
    internal class PlayerUtils
    {
        public static void SendBroadcast(Player p, string message)
        {
            // todo better with SSS
            p.Broadcast(5, message);
        }
    }
}
