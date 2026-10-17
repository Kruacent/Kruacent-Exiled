using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Lockers;
using Exiled.API.Features.Pools;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.Feeds;
using KE.Utils.API.Features;
using KE.Utils.API.Translations;
using KruacentExiled.CustomItems.API.Core.Lights;
using KruacentExiled.CustomItems.API.Exceptions;
using KruacentExiled.CustomItems.API.Interface;
using KruacentExiled.CustomSpawnPoint.Spawned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Pickup = Exiled.API.Features.Pickups.Pickup;

namespace KruacentExiled.CustomItems.API.Features
{
    public abstract class KECustomItem : CustomItem
    {


        private static Dictionary<Type, KECustomItem> _typeLookup = new Dictionary<Type, KECustomItem>();

        private static Dictionary<string, KECustomItem> _nameLookup = new Dictionary<string, KECustomItem>();


        [Obsolete("Uses only the name",true)]
        public sealed override uint Id { get; set; } = 0;

        public sealed override string Description { get; set; } = string.Empty;

        public abstract ItemType ItemType { get; }

        public override float Weight { get; set; } = 1;

        public sealed override ItemType Type
        {
            get
            {
                return ItemType;
            }
            set
            {

            }
        }
        public const string CustomItemNameKey = "Name";
        public const string CustomItemDescriptionKey = "Desc";
        public string TranslationKeyName => Name + "_" + CustomItemNameKey;
        public string TranslationKeyDesc => Name + "_" + CustomItemDescriptionKey;

        public const string CustomItemTranslationId = "CustomItem";
        protected abstract Dictionary<string, Dictionary<string, string>> SetTranslation();


        public const string PickupKey = "Pickup";
        public const string InventoryKey = "Inventory";

        private Dictionary<string, Dictionary<string, string>> GetBasicTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [PickupKey] = "You've picked up ",
                    [InventoryKey] = "You've selected ",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [PickupKey] = "Tu as pris ",
                    [InventoryKey] = "Tu as selectionné ",
                },
            };
        }

        private void OneTimeInit()
        {
            if (init) return;

            var trans = GetBasicTranslation();
            TranslationHub.Add(CustomItemTranslationId, trans);
            init = true;
        }
        private bool init = false;

        public static string GetTranslation(Player player, string key)
        {
            return TranslationHub.Get(player, CustomItemTranslationId, key);
        }
        public static string GetTranslation(string lang, string key)
        {
            return TranslationHub.Get(lang, CustomItemTranslationId, key);
        }

        public static void TranslationHint(Player player,string key)
        {
            ItemEffectHint(player, GetTranslation(player, key));
        }

        public static void TranslationFeed(Player player,string key)
        {
            HintFeed.AddFeed(player, GetTranslation(player, key));
        }

        public override void Init()
        {
            _typeLookup.Add(GetType(), this);
            Name = Name.RemoveSpaces();

            _nameLookup.Add(Name, this);
            InternalSubscribeEvent();
            SubscribeEvents();

            OneTimeInit();

            var translate = SetTranslation();
            TranslationHub.Add(CustomItemTranslationId, translate);
        }

        private void InternalSubscribeEvent()
        {
            Exiled.Events.Handlers.Map.FillingLocker += InternalOnFillingLocker;
            Exiled.Events.Handlers.Map.Generating += InternalOnGenerating;
        }

        private void InternalUnsubscribeEvent()
        {
            Exiled.Events.Handlers.Map.FillingLocker -= InternalOnFillingLocker;
            Exiled.Events.Handlers.Map.Generating -= InternalOnGenerating;
        }


        protected virtual uint MaxPerLocker { get; } = 1;
        protected virtual List<LockerSpawnPoint> LockerSpawnPoint { get; } = null;

        public override void Destroy()
        {
            InternalUnsubscribeEvent();
            UnsubscribeEvents();
            _nameLookup.Remove(Name);
            _typeLookup.Remove(GetType());
        }


        private Dictionary<Locker, uint> _numberInLocker = null;

        private void InternalOnGenerating(GeneratingEventArgs ev)
        {

            if (_numberInLocker != null)
            {
                _numberInLocker.Clear();
            }
            else
            {
                _numberInLocker = new Dictionary<Locker, uint>();
            }

        }

        private void InternalOnFillingLocker(FillingLockerEventArgs ev)
        {
            if (LockerSpawnPoint == null) return;

            Locker locker = ev.Locker;

            if(_numberInLocker.TryGetValue(locker,out uint value) && value >= MaxPerLocker)
            {
                KELog.Debug("max got");
                return;
            }




            foreach (LockerSpawnPoint spawnpoint in LockerSpawnPoint)
            {
                if (!spawnpoint.UseChamber) continue;

                if (spawnpoint.Type != locker.Type) continue;
                if (spawnpoint.Zone != locker.Zone) continue;

                double rng = Exiled.Loader.Loader.Random.NextDouble() * 100.0;
                double luck = (double)spawnpoint.Chance;

                bool luckCheck = rng < luck;

                KELog.Debug($"luck check of {Name} at {spawnpoint.Position} ({spawnpoint.Name})  : {luckCheck}");

                if (!luckCheck)
                {
                    continue;
                }

                if (ev.Pickup.Type == Type)
                {
                    SetItem(ev.Pickup);
                    if (!_numberInLocker.ContainsKey(locker))
                    {
                        _numberInLocker[locker] = 1;
                    }
                    else
                    {
                        _numberInLocker[locker]++;
                    }
                    KELog.Debug("setitem at " + ev.Pickup.Position + " at " + spawnpoint.Type);
                }
            }

        }
        

        public static T Get<T>() where T : KECustomItem
        {
            return (T)_typeLookup[typeof(T)];
        }
        public new static KECustomItem Get(string name)
        {
            if (!name.Contains(name))
            {
                throw new CustomItemNotFoundException(name);
            }


            return _nameLookup[name];
        }
        public static bool TryGet(string name, out KECustomItem item)
        {


            return _nameLookup.TryGetValue(name,out item);
        }


        protected override void ShowPickedUpMessage(Player player)
        {
            Message(this, player, true);
        }

        protected override void ShowSelectedMessage(Player player)
        {
            Message(this, player);
        }


        public override uint Spawn(IEnumerable<SpawnPoint> spawnPoints, uint limit)
        {
            


            HashSet<SpawnPoint> spawns = spawnPoints.ToHashSet();
            uint num = 0;
            foreach (SpawnPoint spawnpoint in spawnPoints.Where(sp => sp is RoomSpawnPoint))
            {
                Pickup pickup = null;
                if (Exiled.Loader.Loader.Random.NextDouble() * 100.0 >= (double)spawnpoint.Chance || limit != 0 && num >= limit)
                {
                    continue;
                }
                spawns.Remove(spawnpoint);
                RoomSpawnPoint room = spawnpoint as RoomSpawnPoint;
                RoomCustomItemPoint spawn = RoomCustomItemPoint.UseRandom(room.Room);

                if (spawn != null)
                {
                    //Log.Debug($"spawning custom pos");
                    pickup = Spawn(spawn.Position);
                }
                else
                {
                    if (spawnpoint is LockerSpawnPoint { UseChamber: true } lockerSpawnPoint)
                    {
                        try
                        {
                            lockerSpawnPoint.GetSpawningInfo(out var _, out var chamber, out var position);

                            if(chamber.Base.Content.Count == 0)
                            {
                                pickup = Spawn(position);
                                chamber?.AddItem(pickup);
                            }

                            
                        }
                        catch (Exception ex)
                        {
                            Log.Error($"CustomItem {Name}(failed to spawn: {ex.Message})");
                            continue;
                        }
                    }
                    else
                    {
                        Log.Error($"can't spawn ({Name}) in custom ({room.Room})");
                        pickup = Spawn(spawnpoint.Position);
                    }
                }



                if (pickup != null)
                {
                    num++;
                }

                    
            }

            return num;
        }





        internal static void Message(CustomItem c, Player player, bool pickedUp = false)
        {
            KECustomItem kECustomItem = c as KECustomItem;
            StringBuilder builder = StringBuilderPool.Pool.Get();

            string lang = TranslationHub.GetLang(player);

            if (MainPlugin.Instance.SettingsHandler.GetPrefixes(player))
            {
                if (pickedUp)
                {
                    builder.Append("(P)");
                }
                else
                {
                    builder.Append("(I)");
                }
            }
            else
            {
                if (pickedUp)
                {
                    builder.Append(TranslationHub.Get(lang,CustomItemTranslationId,PickupKey));
                }
                else
                {
                    builder.Append(TranslationHub.Get(lang, CustomItemTranslationId, InventoryKey));
                }
            }

            builder.Append("<b>");
            builder.Append(TranslationHub.Get(lang, CustomItemTranslationId, kECustomItem.TranslationKeyName));
            builder.AppendLine("</b>");

            bool desc = MainPlugin.Instance.SettingsHandler.GetDescriptionsSettings(player);

            if (desc)
            {
                builder.Append(TranslationHub.Get(lang, CustomItemTranslationId, kECustomItem.TranslationKeyDesc));
                if (c is IUpgradableCustomItem ci)
                {
                    builder.Append("<b>");
                    foreach (var a in ci.Upgrade)
                    {
                        builder.Append(a.Key);
                        builder.Append(" (");
                        builder.Append(a.Value.Chance);
                        builder.Append("%) -> ???");
                    }
                    builder.AppendLine("</b>");
                }

            }



            float delay = MainPlugin.Instance.SettingsHandler.GetTime(player);

            DisplayHandler.Instance.AddHint(MainPlugin.HintPlacement, player, StringBuilderPool.Pool.ToStringReturn(builder), delay);


        }




        public static void ItemEffectHint(Player player, string text)
        {
            float delay = MainPlugin.Instance.SettingsHandler.GetTimeEffect(player);


            DisplayHandler.Instance.AddHint(MainPlugin.ItemEffectPlacement, player, text, delay);
        }


        public virtual bool TryRegister()
        {


            if (Registered.Contains(this))
            {
                Log.Error($"{Name} is already registered");
                return false;
            }


            if (TryGet(Name,out _))
            {
                Log.Error($"A Custom item already have the name {Name}");
                return false;
            }


            if(ItemType == ItemType.None)
            {
                Log.Error($"No ItemType for {Name}");
                return false;
            }


            Registered.Add(this);
            Init();
            return true;


        }

        /// <summary>
        /// Set a <see cref="Pickup"/> as the <see cref="KECustomItem"/>
        /// </summary>
        /// <param name="pickup"></param>
        /// <param name="forceSameItem">force the pickup to be the same item as the <see cref="KECustomItem"/><br/> see <seealso cref="Type"/></param>
        public void SetItem(Pickup pickup,bool forceSameItem = true)
        {
            
            if(forceSameItem && pickup.Type != Type)
            {
                throw new ArgumentException("wrong item type");
            }

            pickup.Scale = Scale;
            pickup.Weight = Weight;

            TrackedSerials.Add(pickup.Serial);
            LightsHandler.AddPickup(pickup.Base);
        }

        public static bool IsConsideredViolent(KECustomItem item)
        {
            if(item is IViolentItem violent)
            {
                return violent.IsViolent;
            }

            if (item.ItemType.IsWeapon(true))
            {
                return true;
            }
            ItemCategory itemCategory = item.ItemType.GetCategory();

            if (itemCategory == ItemCategory.Firearm)
            {
                return true;
            }
            ProjectileType projectileType = item.ItemType.GetProjectileType();
            if (projectileType == ProjectileType.FragGrenade || projectileType == ProjectileType.Scp018)
            {
                return true;
            }
            

            return false;
        }



//obselete warning
#pragma warning disable CS0618
#pragma warning disable CS0672
        protected sealed override void OnDropping(DroppingItemEventArgs ev)
        {
            base.OnDropping(ev);
        }

#pragma warning restore CS0618
#pragma warning restore CS0672
        public static void RegisterItems(Assembly assembly = null)
        {
            IEnumerable<KECustomItem> items = KE.Utils.API.ReflectionHelper.GetObjects<KECustomItem>(assembly);

            foreach(KECustomItem customItem in items)
            {
                customItem.TryRegister();
            }

        }

        public new static void UnregisterItems()
        {
            foreach(CustomItem item in Registered)
            {
                item.Unregister();
            }
        }
    }
}
