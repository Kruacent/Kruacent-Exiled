using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc
{
    /// <summary>
    /// The elevator will random activate in the round
    /// </summary>
    internal class AutoElevator
    {
        /// <summary>
        /// Start the auto elevator loop
        /// </summary>
        internal IEnumerator<float> StartElevator()
        {
            Log.Debug("elevator");
            while (!Round.IsEnded)
            {
                foreach (Lift l in Lift.List)
                {
                    yield return Timing.WaitForSeconds(UnityEngine.Random.Range(30, 45));
                    SendElevator(l);
                }
            }
        }
        private void SendElevator(Lift e)
        {
            Log.Debug($"{e.Name}");
            e.TryStart(0, true);
        }
    }
}
