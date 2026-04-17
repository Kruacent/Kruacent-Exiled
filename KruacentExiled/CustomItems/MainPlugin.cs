using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using HarmonyLib;
using KE.Utils.API.Displays.DisplayMeow;
using KruacentExiled;
using KruacentExiled.CustomItems.API.Core.Lights;
using KruacentExiled.CustomItems.API.Core.Settings;
using KruacentExiled.CustomItems.API.Core.Upgrade;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Features.SpawnPoints;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KruacentExiled.CustomItems
{
    public class MainPlugin : KEPlugin
    {
        public override string Name => "KE.Items";
        public override string Prefix => "KE.I";
        internal UpgradeHandler UpgradeHandler { get; private set; }
        internal LightsHandler LightsHandler { get; private set; }
        internal static MainPlugin Instance { get; private set; }
        internal SettingsHandler SettingsHandler { get; private set; }

        public override IConfig Config => config;
        private IConfig config;

        internal static readonly HintPlacement ItemEffectPlacement = new HintPlacement(0, 200, HintServiceMeow.Core.Enum.HintAlignment.Center);
        internal static readonly HintPlacement HintPlacement = new HintPlacement(0, 400, HintServiceMeow.Core.Enum.HintAlignment.Center);

        internal Harmony harmony;
        
        public override void OnEnabled()
        {
            Instance = this;
            config = KruacentExiled.MainPlugin.Instance.Config.CustomItemConfig;
            harmony = new Harmony(Name);
            harmony.PatchAll(KruacentExiled.MainPlugin.Instance.Assembly);
            UpgradeHandler = new UpgradeHandler();
            LightsHandler = new LightsHandler();
            SettingsHandler = new SettingsHandler();


            KE.Utils.API.Sounds.SoundPlayer.Load();
            PoseRoomSpawnPointHandler.AddRoomPoses(new HashSet<PoseRoomSpawnPointHandler.ItemSpawn>()
            {
                new PoseRoomSpawnPointHandler.ItemSpawn(RoomType.Lcz914,new Vector3(0,0.70f,-7.14f),Quaternion.identity),
                new PoseRoomSpawnPointHandler.ItemSpawn(RoomType.LczGlassBox,new Vector3(7.39f,0.75f,-5.89f),Quaternion.identity),
                new PoseRoomSpawnPointHandler.ItemSpawn(RoomType.LczGlassBox,new Vector3(8.71f,1.2f,-5.89f),Quaternion.identity),
                new PoseRoomSpawnPointHandler.ItemSpawn(RoomType.Lcz173,new Vector3(7.39f,0.75f,-5.89f),Quaternion.identity),
                new PoseRoomSpawnPointHandler.ItemSpawn(RoomType.EzUpstairsPcs,new Vector3(-2.09f,0.75f,-7f),Quaternion.identity),
                new PoseRoomSpawnPointHandler.ItemSpawn(RoomType.EzUpstairsPcs,new Vector3(-4,1.1f,-0.36f),Quaternion.identity),
                new PoseRoomSpawnPointHandler.ItemSpawn(RoomType.EzGateB,new Vector3(-0.68f,1.33f,4f),Quaternion.identity),
                new PoseRoomSpawnPointHandler.ItemSpawn(RoomType.EzChef,new Vector3(2.36f,.2f,-0.16f),Quaternion.identity),
                new PoseRoomSpawnPointHandler.ItemSpawn(RoomType.HczStraightPipeRoom,new Vector3(6.1f,1.05f,-4.8f),Quaternion.identity),
                new PoseRoomSpawnPointHandler.ItemSpawn(RoomType.HczStraightPipeRoom,new Vector3(-4f,0.23f,-4.52f),Quaternion.identity),
                new PoseRoomSpawnPointHandler.ItemSpawn(RoomType.HczServerRoom,new Vector3(1.79f,0.78f,-0.6f),Quaternion.identity),
                new PoseRoomSpawnPointHandler.ItemSpawn(RoomType.HczNuke,new Vector3(12.11f,-75.11f,2.6f),Quaternion.identity),
                new PoseRoomSpawnPointHandler.ItemSpawn(RoomType.HczNuke,new Vector3(11.06f,-74.85f,-2.5f),Quaternion.identity),
                new PoseRoomSpawnPointHandler.ItemSpawn(RoomType.Surface,new Vector3(27.45f,-8.07f,24.96f),Quaternion.identity),
                new PoseRoomSpawnPointHandler.ItemSpawn(RoomType.Surface,new Vector3(27.45f,-8.07f,-23.62f),Quaternion.identity),
                new PoseRoomSpawnPointHandler.ItemSpawn(RoomType.Surface,new Vector3(27.45f,-8.07f,-23.62f),Quaternion.identity),
                new PoseRoomSpawnPointHandler.ItemSpawn(RoomType.HczHid,new Vector3(-3.41f,5.68f,-2.3f),Quaternion.identity),
                new PoseRoomSpawnPointHandler.ItemSpawn(RoomType.HczHid,new Vector3(-6.44f,5.7f,-2.5f),Quaternion.identity),
                new PoseRoomSpawnPointHandler.ItemSpawn(RoomType.Hcz127,new Vector3(4.77f, 1.12f, 1.83f),Quaternion.identity),
                new PoseRoomSpawnPointHandler.ItemSpawn(RoomType.Hcz939,new Vector3(.6f, 1.3f, -2.8f),Quaternion.identity),
                new PoseRoomSpawnPointHandler.ItemSpawn(RoomType.LczCafe,new Vector3(-2.27f, 0.92f, 3.28f),Quaternion.identity),
                new PoseRoomSpawnPointHandler.ItemSpawn(RoomType.LczCafe,new Vector3(-2.38f, 0.87f, -1.49f),Quaternion.identity),
                //new(RoomType.HczArmory,new Vector3(1.66f, 1.06f, -2.25f),Quaternion.identity),
                //new(RoomType.HczArmory,new Vector3(1.33f, 1.06f, -1.66f),Quaternion.identity),
                new PoseRoomSpawnPointHandler.ItemSpawn(RoomType.Hcz049,new Vector3(-6.31f, 89.22f, -11.38f),Quaternion.identity),
                new PoseRoomSpawnPointHandler.ItemSpawn(RoomType.Hcz049,new Vector3(0.64f, 89.31f, 7.22f),Quaternion.identity),
                new PoseRoomSpawnPointHandler.ItemSpawn(RoomType.Hcz049,new Vector3(18.46f, 93.73f, 13.13f),Quaternion.identity),
            });



            KECustomItem.RegisterItems();
            UpgradeHandler.SubscribeEvents();
            LightsHandler.SubscribeEvents();
            Exiled.Events.Handlers.Map.Generated += OnGenerated;
        }

        public override void OnDisabled()
        {
            KECustomItem.UnregisterItems();
            UpgradeHandler?.UnsubscribeEvents();
            LightsHandler?.UnsubscribeEvents();
            Exiled.Events.Handlers.Map.Generated -= OnGenerated;

            harmony.UnpatchAll(harmony.Id);
            SettingsHandler = null;
            LightsHandler = null;
            UpgradeHandler = null;
            Instance = null;
        }

        private void OnGenerated()
        {
            PoseRoomSpawnPointHandler.Reset();
        }

    }
}