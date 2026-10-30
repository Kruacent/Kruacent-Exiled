using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Interfaces;
using HarmonyLib;
using KE.Utils.API;
using KE.Utils.API.Features;
using KruacentExiled.Audio;
using KruacentExiled.ClientPrimitives;
using KruacentExiled.CustomSpawnPoint;
using KruacentExiled.CustomSpawnPoint.Spawned;
using KruacentExiled.DebugSettings;
using LabApi.Events.Arguments.ServerEvents;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KruacentExiled
{
    internal class MainPlugin : Plugin<Config>
    {

        private KEPlugin[] plugins;
        public override string Author => "Patrique & OmerGS";
        public override Version Version { get; } = new Version(2, 2, 0);

        public static MainPlugin Instance { get; private set; }


        public static AudioHandler AudioHandler { get; private set; }

        private static Harmony harmony;

        public override void OnEnabled()
        {
            Instance = this;

            KE.Utils.API.Sounds.SoundPlayer.Load();
            
            plugins = new KEPlugin[]
            {
                new CustomRoles.MainPlugin(),
                new GlobalEvents.MainPlugin(),
                new CustomItems.MainPlugin(),
                new Misc.MainPlugin(),
                new Map.MainPlugin(),
            };

            for (int i = 0; i < plugins.Length; i++)
            {
                KEPlugin plugin = plugins[i];
                try
                {
                    plugin.OnEnabled();
                }
                catch(Exception e)
                {
                    Log.Error(e);
                }

            }

            if (Config.Debug)
            {

                ReflectionHelper.GetObjects<DebugSetting>().ToList();
                foreach (DebugSetting debug in DebugSetting.settings)
                {
                    debug.Create();
                    debug.GetCategory();
                }

            }
            //harmony = new Harmony("KEMainPlugin");
            //harmony.PatchAll();

            AudioHandler = new AudioHandler(Config.Debug);
            AudioHandler.SubscribeEvents();


            RoomCustomPoint.Populate();

            Timing.CallDelayed(10, () =>
            {
                Loader.Load();
            });



            Exiled.Events.Handlers.Map.Generated += OnGenerated;
            LabApi.Events.Handlers.ServerEvents.RoundEnded += OnRoundEnded;
            LabApi.Events.Handlers.ServerEvents.RoundRestarted += OnRoundRestarted;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {

            //harmony.UnpatchAll("KEMainPlugin");

            for (int i = 0; i < plugins.Length; i++)
            {
                KEPlugin plugin = plugins[i];

                plugin.OnDisabled();
                Log.Info(plugin.Name + " has been disabled!");

            }
            Exiled.Events.Handlers.Map.Generated -= OnGenerated;
            LabApi.Events.Handlers.ServerEvents.RoundEnded -= OnRoundEnded;
            LabApi.Events.Handlers.ServerEvents.RoundRestarted -= OnRoundRestarted;
            AudioHandler.UnsubscribeEvents();

            Instance = null;
            AudioHandler = null;

            base.OnDisabled();
        }


        #region events

        public void OnGenerated()
        {
            Log.Warn("map generated");
            RoomSpawnedCustomPoint.SpawnAll();

            Timing.CallDelayed(.1f, () =>
            {
                try
                {
                    SpawnItems();
                }
                catch(Exception e)
                {
                    Log.Error(e);
                }
            });


        }

        private void SpawnItems()
        {
            Dictionary<Exiled.API.Enums.RoomType, ItemType> dict = new Dictionary<Exiled.API.Enums.RoomType, ItemType>()
            {
                {RoomType.EzGateA,ItemType.KeycardZoneManager },
                {RoomType.EzDownstairsPcs,ItemType.Flashlight },

            };


            foreach(var kvp in dict)
            {
                Room room = Room.Get(kvp.Key);

                RoomSpawnedCustomPoint point = RoomCustomPointHandler.GetAll(room, RoomCustomPointType.ItemSpawn).First();
                if(point == null)
                {
                    KELog.Debug("null");
                }
                Pickup pickup = Pickup.CreateAndSpawn(kvp.Value, point.Position);
                KELog.Debug("spawned " + pickup);
            }

        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            RoomSpawnedCustomPoint.DestroyAll();
        }

        public void OnRoundRestarted()
        {
            RoomSpawnedCustomPoint.DestroyAll();
        }


        #endregion

    }




    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        public CustomRoles.Config CustomRoleConfig { get; set; } = new CustomRoles.Config();
        public CustomItems.Config CustomItemConfig { get; set; } = new CustomItems.Config();
        public Map.Config MapConfig { get; set; } = new Map.Config();
        public Misc.Config MiscConfig { get; set; } = new Misc.Config();
        public GlobalEvents.Config GEFEConfig { get; set; } = new GlobalEvents.Config();
    }

}
