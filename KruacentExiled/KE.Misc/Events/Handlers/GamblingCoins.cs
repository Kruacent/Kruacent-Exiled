using Exiled.Events.Features;
using KE.Misc.Events.EventsArgs.GamblingCoinsEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Events.Handlers
{
    public static class GamblingCoins
    {



        public static Event<GamblingEventArgs> Gambling = new();
        public static Event<GambledEventArgs> Gambled = new();





        public static void OnGambling(GamblingEventArgs ev)
        {
            Gambling?.InvokeSafely(ev);
        }

        public static void OnGambled(GambledEventArgs ev)
        {
            Gambled?.InvokeSafely(ev);
        }

    }
}
