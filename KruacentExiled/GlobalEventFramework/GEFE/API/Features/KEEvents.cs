using Exiled.API.Features;
using KE.Utils.API;
using KE.Utils.API.Features;
using KruacentExiled.GlobalEventFramework.GEFE.API.Interfaces;
using KruacentExiled.GlobalEventFramework.GEFE.Events.EventArgs;
using KruacentExiled.GlobalEventFramework.GEFE.Events.Handlers;
using KruacentExiled.GlobalEventFramework.GEFE.Exceptions;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KruacentExiled.GlobalEventFramework.GEFE.API.Features
{

    public abstract class KEEvents
    {
        #region Abstract Properties
        /// <summary>
        /// The unique id of the <see cref="KEEvents"/>
        /// </summary>
        //public abstract uint Id { get; set; }
        /// <summary>
        /// The name of the <see cref="KEEvents"/>
        /// </summary>
        public abstract string Name { get; set; }
        /// <summary>
        /// <para>The chance to get this <see cref="KEEvents"/></para>
        /// <para>Note: Set to 0 or lower to disable it</para>
        /// </summary>
        public virtual int WeightedChance { get; set; } = 1;
        /// <summary>
        /// List of incompatible <see cref="KEEvents"/> by its id
        /// </summary>
        public virtual string[] IncompatibleEvents { get; set; } = new string[0];
        protected HashSet<CoroutineHandle> CoroutineHandles { get; } = new HashSet<CoroutineHandle>();
        protected static readonly HashSet<KEEvents> s_activeEvents = new HashSet<KEEvents>();

        #endregion

        #region Static Variables
        //private static Dictionary<uint, KEEvents> _idLookup = new Dictionary<uint, KEEvents>();
        private static Dictionary<string, KEEvents> _nameLookup = new Dictionary<string, KEEvents>();

        public static HashSet<KEEvents> List => new HashSet<KEEvents>(_nameLookup.Values);
        #endregion
        #region Events

        //public static Event<EnablingEventArgs> Enabling = new();
        //public static Event<EnabledEventArgs> Enabled = new();
        //public static Event<EnabledEventArgs> Disabled = new();
        #endregion

        #region Register

        public static IEnumerable<KEEvents> RegisterAll()
        {
            List<Assembly> assemblies = new List<Assembly>();
            foreach(var plugin in Exiled.Loader.Loader.Plugins)
            {
                if (!assemblies.Contains(plugin.Assembly) && plugin.Config.IsEnabled)
                {
                    assemblies.Add(plugin.Assembly);
                }
                    
            }


            IEnumerable<KEEvents> events = ReflectionHelper.GetObjects<KEEvents>(assemblies);
            foreach(KEEvents ev in events)
            {
                try
                {
                    ev.Register();
                }
                catch (FailedRegisterException e)
                {
                    Log.Error($"Failed to register KEevent {ev.Name} \nError : {e}");
                }
                
            }
            return events;
            

        }
        public virtual void Register()
        {
            if (_nameLookup.ContainsKey(Name))
            {
                throw new FailedRegisterException($"name already used");
            }
            LogRegister();
            Init();
        }

        public virtual void Init()
        {
            _nameLookup.Add(Name, this);
            SubscribeEvents();
        }


        public virtual void Destroy()
        {
            _nameLookup.Remove(Name);
            foreach(CoroutineHandle handle in CoroutineHandles)
            {
                Timing.KillCoroutines(handle);
            }
            UnsubscribeEvents();
        }


        public static void DestroyAll()
        {

            foreach (KEEvents ev in List)
            {
                ev.Destroy();
            }
        }
        #endregion



        public static void OnEnabled()
        {
            RegisterAll();
            
        }

        public static void OnDisabled()
        {
            DestroyAll();
        }




        protected virtual void SubscribeEvents()
        {
        }


        protected virtual void UnsubscribeEvents()
        {

        }



        protected static void EnableEvents(IEnumerable<KEEvents> events)
        {
            foreach (KEEvents ev in events)
            {
                Log.Info("enabling " + ev.Name);
                EnablingEventArgs args = new EnablingEventArgs(ev, true);
                KEEventsHandler.OnEnabling(args);

                if (!args.IsAllowed) continue;

                if (ev is IEvent @event)
                {
                    @event.SubscribeEvent();
                }


                if (ev is IAsyncStart asyncstart)
                {
                    ev.CoroutineHandles.Add(Timing.RunCoroutine(asyncstart.Start()));
                }
                    
                if(ev is IStart start)
                {
                    start.Start();
                }


                s_activeEvents.Add(ev);

                KEEventsHandler.OnEnabled(new EnabledEventArgs(ev));
            }
        }

        /// <summary>
        /// Disable all active <see cref="KEEvents"/>
        /// </summary>
        /// <param name="events"></param>
        protected static void DisableEvents(IEnumerable<KEEvents> events)
        {
            foreach (KEEvents ev in events.ToList())
            {
                Log.Info("disabling " + ev.Name);
                if (ev is IEvent @event)
                {
                    @event.UnsubscribeEvent();
                }
                foreach (CoroutineHandle handle in ev.CoroutineHandles)
                {
                    Timing.KillCoroutines(handle);
                }
                ev.Disable(ev);
                KEEventsHandler.OnDisabled(new DisabledEventArgs(ev));
            }
        }
        /// <summary>
        /// Executed after the <see cref="KEEvents"/> is disabled
        /// </summary>
        /// <param name="ev"></param>
        protected virtual void Disable(KEEvents ev)
        {

        }

        /// <summary>
        /// Get a random <see cref="KEEvents"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="numberEvent"></param>
        /// <returns></returns>
        protected static IEnumerable<T> GetRandomEvent<T>(int numberEvent = 1) where T : KEEvents
        {
            List<T> result = new List<T>();
            List<T> weightedPool = new List<T>();
            foreach (T ge in List.Where(ev => ev is T))
            {
                if (!(ge is IConditional) || ge is IConditional c && c.Condition())
                {
                    if (!ge.IsCompatible()) continue;
                    for (int i = 0; i < ge.WeightedChance; i++)
                    {

                        weightedPool.Add(ge);
                        
                    }
                    Log.Debug($"[{ge.Name}] weight : {ge.WeightedChance} ");
                }
            }

            numberEvent = Math.Min(numberEvent, weightedPool.Count);

            for (int i = 0; i < numberEvent; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, weightedPool.Count);
                T selectedGE = weightedPool[randomIndex];
                result.Add(selectedGE);
                if (selectedGE is IOnlyEvent)
                {
                    KELog.Debug($"selected : {selectedGE.Name} is only event");
                    break;
                }

                weightedPool.Remove(selectedGE);
                weightedPool.RemoveAll(e => selectedGE.IncompatibleEvents.Contains(e.Name) || e is IOnlyEvent);
                if (weightedPool.Count == 0) break;
            }

            return result;
        }

        public virtual void LogRegister() => 
            Log.Send($"REGISTERED {Name}", Discord.LogLevel.Info, ConsoleColor.Blue);

        #region Getter

        /// <summary>
        /// Try to get a <see cref="KEEvents"/> by its name
        /// </summary>
        public static bool TryGet(string name, out KEEvents globalEvent)
        {
            globalEvent = null;
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            if (!_nameLookup.ContainsKey(name))
            {
                return false;
            }

            globalEvent = Get(name);

            return globalEvent != null;
        }
        /// <summary>
        /// Get a <see cref="KEEvents"/> by its name
        /// </summary>
        public static KEEvents Get(string name)
        {
            return _nameLookup[name];
        }

        /// <summary>
        /// Check if this <see cref="KEEvents"/> is compatible with active <see cref="KEEvents"/>
        /// </summary>
        /// <returns></returns>
        public bool IsCompatible()
        {
            foreach(KEEvents ev in s_activeEvents)
            {
                foreach(string i in ev.IncompatibleEvents)
                {
                    if (i == Name)
                        return false;
                }
            }
            return true;
        }
#endregion

    }
}
