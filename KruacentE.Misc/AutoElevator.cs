using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.KruacentE.Misc
{
    internal class AutoElevator
    {
        internal IEnumerator<float> StartElevator()
        {
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
            e.TryStart(0, true);
        }
    }
}
