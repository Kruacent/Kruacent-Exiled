using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
using MEC;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using Exiled.CustomItems.API.Features;
using System.Reflection;
using Exiled.API.Features.Attributes;
using Exiled.API.Interfaces;
using System.Collections;
using System;
using Exiled.API.Extensions;
using Exiled.API.Features.Pools;

namespace KE.GlobalEventFramework.GEFE.API.Features
{
    public class GlobalEvent : IGlobalEvent
    {
        /// <summary>
        /// A list of Active GlobalEvents
        /// </summary>
        public static List<IGlobalEvent> ActiveGlobalEvents => ActiveGE.ToList();
        internal static List<IGlobalEvent> ActiveGE { get; set; } = new List<IGlobalEvent>();
        internal static List<CoroutineHandle> coroutineHandles = new List<CoroutineHandle>();
        internal static Dictionary<int, IGlobalEvent> GlobalEvents { get; set; } = new Dictionary<int, IGlobalEvent>();
        /// <summary>
        /// A list of all registered GlobalEvents
        /// </summary>
        public static List<IGlobalEvent> GlobalEventsList => GlobalEvents.Values.ToList();
        ///<inheritdoc/>
        public virtual int Id { get; set; } = -1;
        ///<inheritdoc/>
        public virtual string Name { get; set; } = "GE NOT SET";
        ///<inheritdoc/>
        public virtual string Description { get; set; } = "DESC NOT SET";
        ///<inheritdoc/>
        public virtual int Weight { get; set; } = 1;

        public static void Register(IGlobalEvent globalEvent)
        {
            Log.Send($"REGISTERING {globalEvent.Name}", Discord.LogLevel.Info, ConsoleColor.Blue);
            if (GlobalEvents.ContainsKey(globalEvent.Id))
            {
                Log.Warn($"id already used");
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
        public static void Register(List<IGlobalEvent> globalEvents)
        {
            globalEvents.ForEach(globalEvent => Register(globalEvent));
        }
        ///<inheritdoc/>
        public virtual IEnumerator<float> Start()
        {
            Log.Error($"{GetType().Name} Start is NOT overrided");
            yield return Timing.WaitForSeconds(30f);
        }
        ///<inheritdoc/>
        public virtual void SubscribeEvent()
        {
            Log.Warn($"{GetType().Name} : SubscribeEvent is NOT overrided");
        }
        ///<inheritdoc/>
        public virtual void UnsubscribeEvent()
        {
            Log.Warn($"{GetType().Name} : UnsubscribeEvent is NOT overrided");
        }
        /// <summary>
        /// Create new List/Dictionary for the Global Event storage
        /// </summary>
        internal void Clean()
        {
            GlobalEvents = new Dictionary<int, IGlobalEvent>();
            ActiveGE = new List<IGlobalEvent>();
        }

        /// <summary>
        /// Stop all Coroutine from GE
        /// </summary>
        internal static void StopCoroutines()
		{
			coroutineHandles.ForEach(coroutineHandle =>
			{
				Timing.KillCoroutines(coroutineHandle);
			});

        }
    }
}
