using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features
{
    /// <summary>
    /// The elevator will random activate in the round
    /// </summary>
    internal class AutoElevator
    {
        private CoroutineHandle handle;
        ~AutoElevator()
        {
            Timing.KillCoroutines(handle);
        }


        public void StartLoop()
        {
            handle = Timing.RunCoroutine(StartElevator());
        }
        /// <summary>
        /// Start the auto elevator loop
        /// </summary>
        private IEnumerator<float> StartElevator()
        {
            //Log.Debug("elevator");
            while (!Round.IsEnded)
            {
                foreach (Lift l in Lift.List.ToList())
                {
                    yield return Timing.WaitForSeconds(UnityEngine.Random.Range(30, 45));
                    SendElevator(l);
                }
            }
        }


        private void SendElevator(Lift e)
        {
            //Log.Debug($"{e.Name}");
            e.TryStart(0, true);
        }
    }
}
