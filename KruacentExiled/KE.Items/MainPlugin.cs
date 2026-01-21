
using AdminToys;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using HarmonyLib;
using InventorySystem.Items.ThrowableProjectiles;
using KE.Items.API.Core.Lights;
using KE.Items.API.Core.Settings;
using KE.Items.API.Core.Upgrade;
using KE.Items.API.Events;
using KE.Items.API.Features.Complexes;
using KE.Items.API.Features.SpawnPoints;
using KE.Utils.API.Displays.DisplayMeow;
using System;
using System.Linq;
using UnityEngine;
using static PlayerRoles.Spectating.SpectatableModuleBase;
using InteractableToy = LabApi.Features.Wrappers.InteractableToy;

namespace KE.Items
{
    public class MainPlugin : Plugin<Config>
    {
        public override string Author => "Patrique & OmerGS";
        public override string Name => "KE.Items";
        public override string Prefix => "KE.I";
        internal UpgradeHandler UpgradeHandler { get; private set; }
        internal LightsHandler LightsHandler { get; private set; }
        internal static MainPlugin Instance { get; private set; }
        internal SettingsHandler SettingsHandler { get; private set; }

        internal static readonly HintPlacement ItemEffectPlacement = new(0, 200, HintServiceMeow.Core.Enum.HintAlignment.Center);
        internal static readonly HintPlacement HintPlacement = new(0, 400, HintServiceMeow.Core.Enum.HintAlignment.Center);

        //scrapped
        //internal PickupQuality PickupQuality { get; private set; }
        //internal QualityHandler QualityHandler { get; private set; }

        public override PluginPriority Priority => PluginPriority.Low;
        public override Version Version => new (1, 0, 0);
        internal Harmony harmony;
        
        public override void OnEnabled()
        {
            Instance = this;
            harmony = new(Name);
            harmony.PatchAll(Assembly);
            //QualityHandler = QualityHandler.Instance;
            //QualityHandler.Register();
            UpgradeHandler = new UpgradeHandler();
            LightsHandler = new LightsHandler();
            //PickupQuality = new PickupQuality();
            SettingsHandler = new();


            Utils.API.Sounds.SoundPlayer.Load();
            PoseRoomSpawnPointHandler.AddRoomPoses(new()
            {
                new(RoomType.Lcz914,new Vector3(0,0.70f,-7.14f),Quaternion.identity),
                new(RoomType.LczGlassBox,new Vector3(7.39f,0.75f,-5.89f),Quaternion.identity),
                new(RoomType.LczGlassBox,new Vector3(8.71f,1.2f,-5.89f),Quaternion.identity),
                new(RoomType.Lcz173,new Vector3(7.39f,0.75f,-5.89f),Quaternion.identity),
                new(RoomType.EzUpstairsPcs,new Vector3(-2.09f,0.75f,-7f),Quaternion.identity),
                new(RoomType.EzUpstairsPcs,new Vector3(-4,1.1f,-0.36f),Quaternion.identity),
                new(RoomType.EzGateB,new Vector3(-0.68f,1.33f,4f),Quaternion.identity),
                new(RoomType.EzChef,new Vector3(2.36f,.2f,-0.16f),Quaternion.identity),
                new(RoomType.HczStraightPipeRoom,new Vector3(6.1f,1.05f,-4.8f),Quaternion.identity),
                new(RoomType.HczStraightPipeRoom,new Vector3(-4f,0.23f,-4.52f),Quaternion.identity),
                new(RoomType.HczServerRoom,new Vector3(1.79f,0.78f,-0.6f),Quaternion.identity),
                new(RoomType.HczNuke,new Vector3(12.11f,-75.11f,2.6f),Quaternion.identity),
                new(RoomType.HczNuke,new Vector3(11.06f,-74.85f,-2.5f),Quaternion.identity),
                new(RoomType.Surface,new Vector3(27.45f,-8.07f,24.96f),Quaternion.identity),
                new(RoomType.Surface,new Vector3(27.45f,-8.07f,-23.62f),Quaternion.identity),
                new(RoomType.Surface,new Vector3(27.45f,-8.07f,-23.62f),Quaternion.identity),
                new(RoomType.HczHid,new Vector3(-3.41f,5.68f,-2.3f),Quaternion.identity),
                new(RoomType.HczHid,new Vector3(-6.44f,5.7f,-2.5f),Quaternion.identity),
                new(RoomType.Hcz127,new Vector3(4.77f, 1.12f, 1.83f),Quaternion.identity),
                new(RoomType.Hcz939,new Vector3(.6f, 1.3f, -2.8f),Quaternion.identity),
                new(RoomType.LczCafe,new Vector3(-2.27f, 0.92f, 3.28f),Quaternion.identity),
                new(RoomType.LczCafe,new Vector3(-2.38f, 0.87f, -1.49f),Quaternion.identity),
                //new(RoomType.HczArmory,new Vector3(1.66f, 1.06f, -2.25f),Quaternion.identity),
                //new(RoomType.HczArmory,new Vector3(1.33f, 1.06f, -1.66f),Quaternion.identity),
                new(RoomType.Hcz049,new Vector3(-6.31f, 89.22f, -11.38f),Quaternion.identity),
                new(RoomType.Hcz049,new Vector3(0.64f, 89.31f, 7.22f),Quaternion.identity),
                new(RoomType.Hcz049,new Vector3(18.46f, 93.73f, 13.13f),Quaternion.identity),
            });

            //Exiled.Events.Handlers.Server.RoundStarted += Test;

            
            CustomItem.RegisterItems();
            //PickupQuality?.SubscribeEvents();
            SettingsHandler.SubscribeEvents();
            UpgradeHandler.SubscribeEvents();
            LightsHandler.SubscribeEvents();
            Exiled.Events.Handlers.Map.Generated += OnGenerated;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            CustomItem.UnregisterItems();
            UpgradeHandler?.UnsubscribeEvents();
            LightsHandler?.UnsubscribeEvents();
            //PickupQuality?.UnsubscribeEvents();
            //QualityHandler?.Unregister();
            SettingsHandler.UnsubscribeEvents();
            Exiled.Events.Handlers.Map.Generated -= OnGenerated;

            //Exiled.Events.Handlers.Server.RoundStarted -= Test;

            //QualityHandler = null;
            //PickupQuality = null;
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

        public void Test()
        {
            ComplexBase complex = new ComplexGatling();

            Player player = Player.List.First();
            Log.Debug(player.Position);
            complex.Spawn(player.Position,Quaternion.identity);



        }


    }
}