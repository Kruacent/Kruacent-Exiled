using Exiled.API.Extensions;
using Exiled.API.Interfaces;
using HarmonyLib;
using KruacentExiled.Map.Heavy;
using KruacentExiled.Map.Heavy.GamblingZone;
using KruacentExiled.Map.Others.BlackoutNDoor.Handlers;
using KruacentExiled.Map.Surface.ElevatorGateA;
using KruacentExiled.Map.Surface.SupplyDrops;
using PlayerRoles;
using ProjectMER.Features;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Door = Exiled.API.Features.Doors.Door;
using Player = Exiled.API.Features.Player;

namespace KruacentExiled.Map
{
    public class MainPlugin : KEPlugin
    {
        public override string Name => "KE.Map";
        public override string Prefix => "KE.M";
        public static MainPlugin Instance { get; private set; }
        private Handler handler;
        public static Translations Translations { get; } = new Translations();
        public static Config Configs => (Config) Instance?.Config;

        public override IConfig Config => config;
        private Config config;
        private Harmony harmony;

        public override void OnEnabled()
        {
            Instance = this;
            handler = new Handler();
            harmony = new Harmony(Prefix);

            config = KruacentExiled.MainPlugin.Instance.Config.MapConfig;

            handler.SubscribeEvents();
            KE.Utils.API.Sounds.SoundPlayer.Instance.TryLoad();
            Exiled.Events.Handlers.Map.Generated += OnGenerated;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;

            SupplyDrop.SubscribeEvents();
            GamblingRoom.SubscribeEvents();
            harmony.PatchAll(KruacentExiled.MainPlugin.Instance.Assembly);
        }

        private void OnWaitingForPlayers()
        {
            PrefabManager.RegisterPrefabs();
            BulkDoor049.Create();
            CustomElevatorGateA.Create();
        }

        private void OnRoundStarted()
        {
            if (Config.Debug)
            {
                Player player = Player.List.First();
                Vector3 pos;
                pos = player.Position;
            }
        }



        public override void OnDisabled()
        {
            handler?.UnsubscribeEvents();
            Exiled.Events.Handlers.Map.Generated -= OnGenerated;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;

            SupplyDrop.UnsubscribeEvents();
            harmony.UnpatchAll(harmony.Id);
            GamblingRoom.UnsubscribeEvents();
            handler = null;
            Instance = null;
        }



        private void OnGenerated()
        {

            Door lcz173 = Door.Get(Exiled.API.Enums.DoorType.Scp173Gate);
            HashSet<DroppableItem> normal = new HashSet<DroppableItem>()
            {
                new DroppableItem(ItemType.KeycardO5,1,2),
                new DroppableItem(ItemType.Jailbird,1,2),
                new DroppableItem(ItemType.SCP268,1,1),
                ItemType.SCP500,
                ItemType.KeycardMTFCaptain,
                ItemType.GunCOM15,
                ItemType.SCP207,
                ItemType.Adrenaline,
                ItemType.GunCOM18,
                ItemType.KeycardFacilityManager,
                ItemType.Medkit,
                ItemType.KeycardMTFOperative,
                ItemType.KeycardGuard,
                ItemType.Radio,
                ItemType.Ammo9x19,
                ItemType.Ammo44cal,
                ItemType.Ammo12gauge,
                ItemType.Ammo556x45,
                ItemType.Ammo762x39,
                ItemType.GrenadeFlash,
                ItemType.KeycardScientist,
                ItemType.KeycardJanitor,
                ItemType.Coin,
                ItemType.Flashlight,
                ItemType.AntiSCP207,                            
                ItemType.GunCom45,
                ItemType.GunShotgun,
                ItemType.GunRevolver,
                ItemType.GunA7,
            };


            var g = new GamblingRoom(RoleTypeId.Scp173.GetRandomSpawnLocation().Position + Vector3.down, new LootTable(normal));
        }
    }


    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public bool SupplyDropEnabled { get; set; } = false;
        public bool TurretEnabled { get; set; } = false;
        public bool EasterEggEnabled { get; set; } = true;
        public bool BlackoutNDoorEnabled { get; set; } = true;
        public bool BlackoutNDoorWeightVerbose { get; set; } = false;
    }
}
