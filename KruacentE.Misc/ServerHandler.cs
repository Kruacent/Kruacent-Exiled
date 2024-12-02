using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEC;

namespace KE.Misc
{
    internal class ServerHandler
    {
        public void OnRoundStarted()
        {
            MainPlugin.Instance.RandomFF();
            Timing.RunCoroutine(MainPlugin.Instance.NukeAnnouncement());
        }
    }
}
