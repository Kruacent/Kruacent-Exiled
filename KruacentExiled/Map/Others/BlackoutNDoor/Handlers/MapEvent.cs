using Exiled.API.Enums;
using Exiled.API.Features;
using KruacentExiled.Map.Others.BlackoutNDoor.Events.EventArgs;
using MEC;
using UnityEngine;

namespace KruacentExiled.Map.Others.BlackoutNDoor.Handlers
{
    public abstract class MapEvent
    {

        public abstract string Cassie { get; }
        public abstract string CassieTranslated { get; }
        public virtual float Duration { get; } = 10f;
        public void StartEvent(ZoneType zone,float maxTime)
        {
            Log.Debug("start to " + zone);
            float time = Duration;
            if (maxTime > 1)
            {
                time = Mathf.Clamp(Duration, 1, maxTime);
            }

            try
            {
                Start(zone);
            }
            finally
            {
                Timing.CallDelayed(time, delegate
                {
                    Log.Debug("stopping");
                    Stop(zone);
                    PostEventEventArgs postEv = new PostEventEventArgs(this);
                    Events.Handlers.BlackoutNDoor.OnPostEvent(postEv);
                });
            }
            
            


        }

        /// <summary>
        /// Start a <see cref="MapEvent"/> without stopping it
        /// </summary>
        /// <param name="zone"></param>
        public abstract void Start(ZoneType zone);

        /// <summary>
        /// Stop a <see cref="MapEvent"/>
        /// </summary>
        /// <param name="zone"></param>
        public abstract void Stop(ZoneType zone);

    }
}
