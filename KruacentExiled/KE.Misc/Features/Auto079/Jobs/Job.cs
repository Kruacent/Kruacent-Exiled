using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features.Auto079.Jobs
{
    public abstract class Job
    {
        /// <summary>
        /// The base waiting time between 2 actions
        /// </summary>
        public const float WaitTime = .5f;
        protected NPC079 npc;

        public CoroutineHandle Start(NPC079 npc)
        {
            Log.Debug("job name " + this.GetType().Name);
            this.npc = npc;
            return Timing.RunCoroutine(Started());
            

        }

       

        protected abstract IEnumerator<float> Started();


    }
}
