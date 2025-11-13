using Exiled.API.Features;
using GameCore;
using MEC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features
{
    internal class AutoNukeAnnoucement
    {


        private float autodetonatetime = 20*60f;
        private bool flagSaid = false;
        private CoroutineHandle handle;

        public void OnRoundStarted()
        {
            //autodetonatetime = ConfigFile.ServerConfig.GetFloat("auto_warhead_start_minutes") * 60f;
            if (!handle.IsRunning)
            {
                handle = Timing.RunCoroutine(Timer());
            }
        }

        private IEnumerator<float> Timer()
        {
            Stopwatch watch = Stopwatch.StartNew();
            bool flag = false;

            while (watch.Elapsed.TotalMinutes < autodetonatetime)
            {
                yield return Timing.WaitForSeconds(60);
                if(Warhead.IsDetonated || Warhead.IsInProgress)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                SayAnnouncement();
            }

        }




        public void SayAnnouncement()
        {
            if (flagSaid) return;
            Cassie.MessageTranslated("Warning automatic warhead will detonate in 5 minutes",
                "Warning automatic warhead will detonate in <color=#FF0000>5</color> minutes");
            flagSaid = true;
        }


    }
}
