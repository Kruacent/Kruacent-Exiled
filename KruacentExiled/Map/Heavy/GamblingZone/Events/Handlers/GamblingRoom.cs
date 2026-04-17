using KruacentExiled.Map.Heavy.GamblingZone.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Map.Heavy.GamblingZone.Events.Handlers
{
    public static class GamblingRoom
    {


        public static event Action<GamblingEventArgs> Gambling = delegate { };


        public static void OnGambling(GamblingEventArgs ev)
        {
            Gambling?.Invoke(ev);
        }
    }
}
