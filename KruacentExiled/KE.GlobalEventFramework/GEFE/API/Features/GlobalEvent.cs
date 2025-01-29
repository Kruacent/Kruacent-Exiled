using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
using MEC;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using System;

namespace KE.GlobalEventFramework.GEFE.API.Features
{
    public abstract class GlobalEvent : IGlobalEvent
    {
        /// <summary>
        /// A list of Active GlobalEvents
        /// </summary>
        public static List<IGlobalEvent> ActiveGlobalEvents => ActiveGE.ToList();
        internal static List<IGlobalEvent> ActiveGE { get; set; } = new List<IGlobalEvent>();
        internal static List<CoroutineHandle> coroutineHandles = new List<CoroutineHandle>();
        internal static Dictionary<uint, IGlobalEvent> GlobalEvents { get; private set; } = new Dictionary<uint, IGlobalEvent>();
        /// <summary>
        /// A list of all registered GlobalEvents
        /// </summary>
        public static List<IGlobalEvent> GlobalEventsList => GlobalEvents.Values.ToList();
        ///<inheritdoc/>
        public abstract uint Id { get; set; }
        ///<inheritdoc/>
        public abstract string Name { get; set; }
        ///<inheritdoc/>
        public abstract string Description { get; set; }
        ///<inheritdoc/>
        public abstract int Weight { get; set; }
        ///<inheritdoc/>
        public virtual uint[] IncompatibleGE { get; set; } = new uint[0];



        ///<inheritdoc/>
        public virtual IEnumerator<float> Start()
        {
            if (MainPlugin.Instance.Config.Debug) Log.Error($"{GetType().Name} Start is NOT overrided");
            yield return 0;
        }
        ///<inheritdoc/>
        public virtual void SubscribeEvent()
        {
            if (MainPlugin.Instance.Config.Debug) Log.Warn($"{GetType().Name} : SubscribeEvent is NOT overrided");
        }
        ///<inheritdoc/>
        public virtual void UnsubscribeEvent()
        {
            if (MainPlugin.Instance.Config.Debug) Log.Warn($"{GetType().Name} : UnsubscribeEvent is NOT overrided");
        }
        public static void Register(IGlobalEvent globalEvent)
        {
            Log.Send($"REGISTERING {globalEvent.Name}", Discord.LogLevel.Info, ConsoleColor.Blue);
            if (GlobalEvents.ContainsKey(globalEvent.Id))
            {
                Log.Error($"{globalEvent.Name}'s id is already registered by {Get(globalEvent.Id)}");
                return;
            }
            GlobalEvents.Add(globalEvent.Id, globalEvent);
            Log.Info($"{globalEvent.Name} is registered");
        }

        public static void Register(List<IGlobalEvent> globalEvents)
        {
            globalEvents.ForEach(globalEvent => Register(globalEvent));
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

        public static bool TryGet(uint id, out IGlobalEvent globalEvent)
        {
            globalEvent = Get(id);
            return globalEvent != null;
        }

        public static bool TryGet(string name, out IGlobalEvent globalEvent)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new System.Exception("name can't be null or empty");
            }
            globalEvent = uint.TryParse(name, out uint id) ? Get(id) : Get(name);

            return globalEvent != null;
        }

        public static IGlobalEvent Get(string name)
        {
            return GlobalEvents.Values.FirstOrDefault(ge => ge.Name == name);
        }

        public static IGlobalEvent Get(uint id)
        {
            return GlobalEvents.TryGetValue(id, out IGlobalEvent globalEvent) ? globalEvent : null;
        }

        private static void Show()
        {
            var random = UnityEngine.Random.value;

            foreach (Player player in Player.List)
            {
                Exiled.API.Features.Broadcast b = new Exiled.API.Features.Broadcast
                {
                    Content = ShowText(random > .5f),
                    Duration = 10
                };
                player.Broadcast(b);
            }
        }

        private static String ShowText(bool redacted = false)
        {
            String result = "Global Events: ";
            Log.Info($"Global Event(s) ({ActiveGE.Count()}): ");
            for (int i = 0; i < ActiveGE.Count(); i++)
            {
                Log.Info(ActiveGE[i].Name);
                if (redacted)
                {
                    result += ActiveGE[i].Description;
                }
                else
                {
                    result += "[REDACTED]";
                }

                if (ActiveGE.Count() > 1 && i < ActiveGE.Count() - 1)
                {
                    result += ", ";
                }
            }


            return result;
        }

        public static List<IGlobalEvent> ChooseGE(int numberOfGlobalEvent = 1)
        {
            List<IGlobalEvent> activeGE = ChooseRandomGE(numberOfGlobalEvent);
            Log.Debug($"activeGE size : {activeGE.Count}");

            return activeGE;
        }

        internal static void ActivateAll()
        {
            ActivateAll(ActiveGE);
        }

        private static void ActivateAll(List<IGlobalEvent> globalEvent)
        {
            if(globalEvent.Count != globalEvent.Distinct().Count()) throw new ArgumentException("You can't have the same GE twice in the same round");
            ActiveGE = globalEvent;
            globalEvent.ForEach(e => e.SubscribeEvent());

            foreach (IGlobalEvent ge in ActiveGE)
            {
                
                CoroutineHandle a = Timing.RunCoroutine(ge.Start());
                coroutineHandles.Add(a);
            }
            Show();
        }

        private static List<IGlobalEvent> ChooseRandomGE(int nbGE = 1)
        {
            List<IGlobalEvent> result = new List<IGlobalEvent>();

            List<IGlobalEvent> weightedPool = new List<IGlobalEvent>();
            foreach (IGlobalEvent ge in GlobalEvent.GlobalEventsList)
            {
                for (int i = 0; i < ge.Weight; i++)
                {
                    weightedPool.Add(ge);
                    Log.Debug($"getochoose : {ge.Name} ");
                }
            }

            nbGE = Math.Min(nbGE, GlobalEvent.GlobalEventsList.Count);

            for (int i = 0; i < nbGE; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, weightedPool.Count);
                IGlobalEvent selectedGE = weightedPool[randomIndex];

                result.Add(selectedGE);

                weightedPool.RemoveAll(e => e == selectedGE);
                weightedPool.RemoveAll(e => selectedGE.IncompatibleGE.Contains(e.Id));
            }

            return result;
        }
    }
}
