using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.CustomItems.API.Features;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
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
        protected HashSet<CoroutineHandle> coroutineHandles { get; } = new();
        protected static HashSet<KEEvents> _activeEvents = new();

        #endregion

        #region Static Variables
        private static Dictionary<uint, KEEvents> _idLookup = new();
        private static Dictionary<string, KEEvents> _nameLookup = new();

        public static HashSet<KEEvents> List => [.._idLookup.Values];
        #endregion

        #region Register

        public static IEnumerable<KEEvents> RegisterAll()
        {
            List<Assembly> assemblies = new();
            foreach(var plugin in Exiled.Loader.Loader.Plugins)
            {
                assemblies.Add(plugin.Assembly);
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
                throw new FailedRegisterException("already registered");
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
                throw new System.Exception("name can't be null or empty");
            }
            globalEvent = uint.TryParse(name, out uint id) ? Get(id) : Get(name);

            return globalEvent != null;
        }

        public static KEEvents Get(string name)
        {
            return List.FirstOrDefault(ge => ge.Name == name);
        }

        public static KEEvents Get(uint id)
        {
            return _idLookup.TryGetValue(id, out KEEvents globalEvent) ? globalEvent : null;
        }

        public bool IsCompatible()
        {
            foreach(KEEvents ev in _activeEvents)
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
