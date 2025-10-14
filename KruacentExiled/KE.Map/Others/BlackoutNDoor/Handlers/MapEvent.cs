using Exiled.API.Enums;
using Exiled.API.Features;
using KE.Map.Others.BlackoutNDoor.Events.EventArgs;
using KE.Map.Others.BlackoutNDoor.Interfaces;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Map.Others.BlackoutNDoor.Handlers
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

            Start(zone);
            Timing.CallDelayed(time, delegate
            {
                Log.Debug("stopping");
                Stop(zone);
                PostEventEventArgs postEv = new PostEventEventArgs(this);
                Events.Handlers.BlackoutNDoor.OnPostEvent(postEv);
            });


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
