using Exiled.API.Features;
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



        private bool flagSaid = false;
        private CoroutineHandle handle;

        public void OnRoundStarted()
        {
            if (!handle.IsRunning)
            {
                handle = Timing.RunCoroutine(Timer());
            }
        }

        private IEnumerator<float> Timer()
        {
            Stopwatch watch = Stopwatch.StartNew();


            while (watch.Elapsed.TotalMinutes < 25)
            {
                yield return Timing.WaitForSeconds(60);
            }

            SayAnnouncement();

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
