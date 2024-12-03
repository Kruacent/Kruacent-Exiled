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
            if(MainPlugin.Instance.Config.ChanceFF >= 0 && MainPlugin.Instance.Config.ChanceFF <= 100)
                MainPlugin.Instance.RandomFF();
            if (MainPlugin.Instance.Config.ChanceClassDDoorGoesBoom >= 0 && MainPlugin.Instance.Config.ChanceClassDDoorGoesBoom <= 100)
                MainPlugin.Instance.ClassDDoorGoesBoom();
            if(MainPlugin.Instance.Config.AutoNukeAnnoucement)
                Timing.RunCoroutine(MainPlugin.Instance.NukeAnnouncement());
            if(MainPlugin.Instance.Config.PeanutLockDown)
                Timing.RunCoroutine(MainPlugin.Instance.PeanutLockdown());
            if(MainPlugin.Instance.Config.AutoElevator)
                Timing.RunCoroutine(MainPlugin.Instance.AutoElevator());
        }
    }
}
