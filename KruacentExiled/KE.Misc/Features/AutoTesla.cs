using Exiled.API.Features;
using LabApi.Features.Wrappers;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Round = Exiled.API.Features.Round;

namespace KE.Misc.Features
{
    /// <summary>
    /// The elevator will random activate in the round
    /// </summary>
    internal class AutoTesla
    {
        private CoroutineHandle handle;

        public void StopLoop()
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
            while (!Round.IsEnded)
            {
                foreach (Tesla tesla in Tesla.List.ToList())
                {
                    yield return Timing.WaitForSeconds(UnityEngine.Random.Range(120f,200f));
                    tesla.Trigger();
                    if(UnityEngine.Random.Range(0f,100f) <= 70f)
                    {
                        yield return Timing.WaitForSeconds(tesla.Base.windupTime+.1f);
                        tesla.Trigger();
                    }
                    
                }
            }
        }
    }
}
