using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.Features;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using KE.GlobalEventFramework.GEFE.Events.EventArgs;
using KE.GlobalEventFramework.GEFE.Events.Handlers;
using KE.GlobalEventFramework.GEFE.Exceptions;
using KE.Utils.API;
using KE.Utils.API.Interfaces;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KE.GlobalEventFramework.GEFE.API.Features
{

    public abstract class KEEvents
    {
        #region Abstract Properties
        public abstract uint Id { get; set; }
        public abstract string Name { get; set; }
        public virtual int WeightedChance { get; set; } = 1;
        public virtual uint[] IncompatibleEvents { get; set; } = new uint[0];
        protected HashSet<CoroutineHandle> CoroutineHandles { get; } = new();
        protected static readonly HashSet<KEEvents> s_activeEvents = new();

        #endregion

        #region Static Variables
        private static Dictionary<uint, KEEvents> _idLookup = new();
        private static Dictionary<string, KEEvents> _nameLookup = new();

        public static HashSet<KEEvents> List => [.._idLookup.Values];
        #endregion
        #region Events

        //public static Event<EnablingEventArgs> Enabling = new();
        //public static Event<EnabledEventArgs> Enabled = new();
        //public static Event<EnabledEventArgs> Disabled = new();
        #endregion

        #region Register

        public static IEnumerable<KEEvents> RegisterAll()
        {
            List<Assembly> assemblies = new();
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
            if (_idLookup.ContainsKey(Id))
            {
                throw new FailedRegisterException($"id already used by {Get(Id).Name} ");
            }
            LogRegister();
            Init();
        }

        public virtual void Init()
        {
            _idLookup.Add(Id, this);
            _nameLookup.Add(Name, this);
            SubscribeEvents();
        }


        public virtual void Destroy()
        {
            _idLookup.Remove(Id);
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
                EnablingEventArgs args = new(ev, true);
                KEEventsHandler.OnEnabling(args);

                if (!args.IsAllowed) continue;

                if (ev is IEvent @event)
                {
                    @event.SubscribeEvent();
                }


                if (ev is IStart start)
                    ev.CoroutineHandles.Add(Timing.RunCoroutine(start.Start()));

                s_activeEvents.Add(ev);

                KEEventsHandler.OnEnabled(new(ev));
            }
        }


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
                KEEventsHandler.OnDisabled(new(ev));
            }
        }

        protected virtual void Disable(KEEvents ev)
        {

        }


        protected static IEnumerable<T> GetRandomEvent<T>(int numberEvent = 1) where T : KEEvents
        {
            List<T> result = new();
            List<T> weightedPool = new();
            foreach (T ge in List.Where(ev => ev is T))
            {
                if (ge is not IConditional || (ge is IConditional c && c.Condition()))
                {
                    if (!ge.IsCompatible()) continue;
                    for (int i = 0; i < ge.WeightedChance; i++)
                    {

                        weightedPool.Add(ge);
                        Log.Debug($"gettochoose : {ge.Name} ");
                    }
                }
            }

            numberEvent = Math.Min(numberEvent, weightedPool.Count);

            for (int i = 0; i < numberEvent; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, weightedPool.Count);
                T selectedGE = weightedPool[randomIndex];

                result.Add(selectedGE);

                weightedPool.Remove(selectedGE);
                weightedPool.RemoveAll(e => selectedGE.IncompatibleEvents.Contains(e.Id));
                if (weightedPool.Count == 0) break;
            }

            return result;
        }

        public virtual void LogRegister() => 
            Log.Send($"REGISTERED {Name}", Discord.LogLevel.Info, ConsoleColor.Blue);

        #region Getter
        public static bool TryGet(uint id, out KEEvents globalEvent)
        {
            globalEvent = Get(id);
            return globalEvent != null;
        }

        public static bool TryGet(string name, out KEEvents globalEvent)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name can't be null or empty");
            }
            globalEvent = uint.TryParse(name, out uint id) ? Get(id) : Get(name);

            return globalEvent != null;
        }

        public static KEEvents Get(string name)
        {
            return _nameLookup[name];
        }

        public static KEEvents Get(uint id)
        {
            return _idLookup.TryGetValue(id, out KEEvents globalEvent) ? globalEvent : null;
        }


        public static T Get<T>(uint id) where T : KEEvents
        {
            return _idLookup.TryGetValue(id, out KEEvents globalEvent) && globalEvent is T ? globalEvent as T : null;
        }

        public static bool TryGet<T>(uint id, out T events) where T : KEEvents
        {
            events = Get<T>(id);
            return events != null;
        }

        public bool IsCompatible()
        {
            foreach(KEEvents ev in s_activeEvents)
            {
                foreach(int i in ev.IncompatibleEvents)
                {
                    if (i == Id)
                        return false;
                }
            }
            return true;
        }
#endregion

    }
}
