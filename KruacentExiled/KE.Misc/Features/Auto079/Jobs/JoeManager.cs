using Exiled.API.Features;
using Exiled.API.Features.Items;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features.Auto079.Jobs
{
    public class JobManager
    {

        private readonly List<Job> Queue = new();
        private readonly List<Job> PriorityQueue = new();
        public NPC079 npc;

        private CoroutineHandle loopHandle;




        public Job defaultJob = new ScanZone();


        public JobManager(NPC079 npc)
        {
            this.npc = npc;
        }


        public void StartLoop()
        {
            Timing.RunCoroutineSingleton(QueueLoop(), loopHandle, SingletonBehavior.AbortAndUnpause);
        }
        private IEnumerator<float> QueueLoop()
        {
            Log.Debug("loop : "+ npc.Npc.IsAlive);
            while (npc.Npc.IsAlive)
            {
                Log.Debug("in loop");

                CoroutineHandle handle;
                Job currentJob = null;
                bool prioQueue = PriorityQueue.Count > 0;

                if(prioQueue)
                {
                    handle = PriorityQueueLoop(out currentJob);
                }
                else
                {
                    handle = NormalQueue(out currentJob);

                }


                yield return Timing.WaitUntilDone(handle);
                if (currentJob is not null)
                {
                    if (prioQueue)
                    {
                        PriorityQueue.Remove(currentJob);
                    }
                    else
                    {
                        Queue.Remove(currentJob);

                    }
                }

            }
        }



        private CoroutineHandle NormalQueue(out Job currentJob)
        {
            bool gotJob = Queue.Count > 0;
            currentJob = null;
            CoroutineHandle handle;

            if (gotJob)
            {
                currentJob = Queue[0];


                handle = currentJob.Start(npc);
            }
            else
            {
                handle = defaultJob.Start(npc);
            }
            return handle;
        }


        private CoroutineHandle PriorityQueueLoop(out Job currentJob)
        {
            currentJob = PriorityQueue[0];
            return currentJob.Start(npc);

        }

        public void AddToQueue(Job job)
        {
            Queue.Add(job);
        }


        public void AddToPriorityQueue(Job job)
        {
            PriorityQueue.Add(job);
        }
        

    }
}
