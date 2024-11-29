using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
using MEC;
using GEFExiled.GEFE.API.Interfaces;
using Exiled.CustomItems.API.Features;
using System.Reflection;
using Exiled.API.Features.Attributes;
using Exiled.API.Interfaces;
using System.Collections;
using System;
using Exiled.API.Extensions;
using Exiled.API.Features.Pools;

namespace GEFExiled.GEFE.API.Features
{
    public class GlobalEvent : IGlobalEvent
    {
        /// <summary>
        /// A list of Active GlobalEvents
        /// </summary>
        public static List<IGlobalEvent> ActiveGlobalEvents => ActiveGE.ToList();
        internal static List<IGlobalEvent> ActiveGE { get; } = new List<IGlobalEvent>();
        internal static Dictionary<int, IGlobalEvent> GlobalEvents { get; set; } = new Dictionary<int, IGlobalEvent>();
        /// <summary>
        /// A list of all registered GlobalEvents
        /// </summary>
        public static List<IGlobalEvent> GlobalEventsList => GlobalEvents.Values.ToList();
        public static HashSet<GlobalEvent> Registered { get; } = new HashSet<GlobalEvent>();
        public virtual int Id { get; set; } = -1;
        public virtual string Name { get; set; } = "GE NOT SET";
        public virtual string Description { get; set; } = "DESC NOT SET";
        public virtual double Weight { get; set; } = 1;

        public static void Register(IGlobalEvent globalEvent)
        {
            Log.Debug("REGISTERING" + globalEvent.Name);
            if (GlobalEvents.ContainsKey(globalEvent.Id))
            {
                Log.Warn($"id already used for {GlobalEvents.TryGetValue(globalEvent.Id, out IGlobalEvent geAlready)}");
                Log.Warn("Trying to attribute a new id...");
                int key = 0;
                while (GlobalEvents.ContainsKey(key))
                {
                    key++;
                }
                globalEvent.Id = key;
                Log.Warn($"new id of {globalEvent.Name} : {globalEvent.Id}");
            }
            GlobalEvents.Add(globalEvent.Id, globalEvent);
            Log.Info($"{globalEvent.Name} is registered");

        }

        public virtual IEnumerator<float> Start()
        {
            Log.Error($"{GetType().Name} Start is NOT overrided");
            yield return Timing.WaitForSeconds(30f);
        }

        public virtual void SubscribeEvent()
        {
            Log.Error($"{GetType().Name} SubscribeEvent is NOT overrided");
        }
        public virtual void UnsubscribeEvent()
        {
            Log.Error($"{GetType().Name} UnsubscribeEvent is NOT overrided");
        }
    }
}
