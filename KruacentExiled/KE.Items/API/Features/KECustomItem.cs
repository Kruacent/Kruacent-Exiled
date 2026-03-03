using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.Items.API.Features.SpawnPoints;
using KE.Items.API.Interface;
using KE.Utils.API.Displays.DisplayMeow;
using PlayerRoles.SpawnData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using static KE.Items.API.Features.SpawnPoints.PoseRoomSpawnPointHandler;
using Pickup = Exiled.API.Features.Pickups.Pickup;

namespace KE.Items.API.Features
{
    public abstract class KECustomItem : CustomItem
    {


        private static Dictionary<Type, KECustomItem> _typeLookup = new();

        private static Dictionary<string, KECustomItem> _nameLookup = new();


        [Obsolete("Uses only the name",true)]
        public sealed override uint Id { get; set; } = 0;

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


        public override void Init()
        {
            _typeLookup.Add(GetType(), this);
            Name = Name.RemoveSpaces();

            _nameLookup.Add(Name, this);
            SubscribeEvents();
        }


        public override void Destroy()
        {
            UnsubscribeEvents();
            _nameLookup.Remove(Name);
            _typeLookup.Remove(GetType());
        }



        

        public static T Get<T>() where T : KECustomItem
        {
            return (T)_typeLookup[typeof(T)];
        }
        public new static KECustomItem Get(string name)
        {
            return _nameLookup[name];
        }
        public static bool TryGet(string name, out KECustomItem item)
        {


            return _nameLookup.TryGetValue(name,out item);
        }


        protected override void ShowPickedUpMessage(Player player)
        {
            Log.Debug("pickup");
            Message(this, player, true);
        }

        protected override void ShowSelectedMessage(Player player)
        {
            Log.Debug("select");
            Message(this, player);
        }


        public override uint Spawn(IEnumerable<SpawnPoint> spawnPoints, uint limit)
        {
            


            Log.Debug($"spawning {this.Name}");
            HashSet<SpawnPoint> spawns = spawnPoints.ToHashSet();
            uint num = 0;
            foreach (SpawnPoint spawnpoint in spawnPoints.Where(sp => sp is RoomSpawnPoint))
            {
                Pickup pickup;
                if (Exiled.Loader.Loader.Random.NextDouble() * 100.0 >= (double)spawnpoint.Chance || (limit != 0 && num >= limit))
                {
                    continue;
                }
                spawns.Remove(spawnpoint);
                RoomSpawnPoint room = spawnpoint as RoomSpawnPoint;
                ItemSpawn spawn = PoseRoomSpawnPointHandler.UseRandomPose(room.Room);
                Log.Debug(room.Room+ " : "+PoseRoomSpawnPointHandler.UsablePoses.Count(p => p.roomType == room.Room));
                Log.Debug($"spawning {this.Name} in {room.Room}" );

                if (spawn is not null)
                {
                    Log.Debug($"spawning custom pos");
                    pickup = Spawn(spawn.Position);
                }
                else
                {
                    Log.Error($"can't spawn ({Name}) in custom ({room.Room})");
                    pickup = Spawn(spawnpoint.Position);
                }



                if (pickup is not null)
                {
                    num++;
                }

                    
            }

            return base.Spawn(spawns, limit-num);
        }



        public void ReplacePickup(Pickup pickup)
        {
            Vector3 position = pickup.Position;
            pickup.Destroy();
            Spawn(position);
        }


        internal static void Message(CustomItem c, Player player, bool pickedUp = false)
        {

            StringBuilder builder = StringBuilderPool.Pool.Get();

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
                    builder.Append("You've picked up ");
                }
                else
                {
                    builder.Append("You've selected ");
                }
            }

            builder.AppendLine($"<b>{c.Name}</b>");

            bool desc = MainPlugin.Instance.SettingsHandler.GetDescriptionsSettings(player);

            if (desc)
            {
                builder.AppendLine(c.Description);
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
            IEnumerable<KECustomItem> items = Utils.API.ReflectionHelper.GetObjects<KECustomItem>(assembly);

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
