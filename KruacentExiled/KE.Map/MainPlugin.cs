
using AdminToys;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.API.Interfaces;
using HarmonyLib;
using KE.Map.Heavy;
using KE.Map.Heavy.GamblingZone;
using KE.Map.Others.BlackoutNDoor.Handlers;
using KE.Utils.API.KETextToy;
using MEC;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp106;
using ProjectMER.Features;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Door = Exiled.API.Features.Doors.Door;
using Player = Exiled.API.Features.Player;

namespace KE.Map
{
    public class MainPlugin : Plugin<Config,Translations>
    {
        public override string Name => "KE.Map";
        public override string Prefix => "KE.M";
        public static MainPlugin Instance { get; private set; }
        private Handler handler;
        public static Translations Translations => Instance?.Translation;
        public static Config Configs => Instance?.Config;
        private Harmony harmony;
        public override void OnEnabled()
        {
            handler = new();
            harmony = new(Prefix);
            



            handler.SubscribeEvents();
            KE.Utils.API.Sounds.SoundPlayer.Instance.TryLoad();
            Exiled.Events.Handlers.Map.Generated += OnGenerated;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;

            GamblingRoom.SubscribeEvents();
            //MoreRoom.CreateAll();
            //MoreRoom.SubscribeEvents();
            harmony.PatchAll(Assembly);

             



            Instance = this;
            base.OnEnabled();
        }

        private void OnWaitingForPlayers()
        {
            PrefabManager.RegisterPrefabs();
            BulkDoor049.Create();
        }

        private void OnRoundStarted()
        {
            if (Config.Debug)
            {
                Player player = Player.List.First();
                //StructureSpawner.SpawnPedestal(ItemType.KeycardJanitor,player.Position,Quaternion.identity,Vector3.one);

                //player.Teleport(Room.List.Where(r => r.Type == Exiled.API.Enums.RoomType.EzVent).First());


                //player.Teleport(teleport);

                Timing.CallDelayed(2, () =>
                {
                    //player.Teleport(teleport);
                });

                Vector3 pos = RoleTypeId.Scp049.GetRandomSpawnLocation().Position;




                //glass or door w/glass => fake to 106s & 173s & 049-2s & 049s & 096s & ghostly
                //door w/out glass => fake to 106s & ghostly

                //Primitive prim = Primitive.Create(PrimitiveType.Cube, pos, spawn: false);

                //prim.Base.gameObject.layer = 14;
                //prim.Spawn();
                //Collider col = prim.Base.gameObject.GetComponent<BoxCollider>();

                //MirrorExtensions.SendFakeSyncVar(player, prim.Base.netIdentity,typeof(PrimitiveObjectToy), "NetworkPrimitiveFlags", PrimitiveFlags.Visible);


                //Log.Info(prim.Base.gameObject.layer);
                //Log.Info(LayerMask.LayerToName(14));

                //Log.Info(Scp106MovementModule.GetSlowdownFromCollider(col, out bool passable) + " passable?" + passable);

                FollowingTextToy f1 = new([player], player.Position, Quaternion.identity, Vector3.one);
                f1.Toy.TextFormat = "F1 sur un bateau";

                FollowingTextToy f2 = new([], player.Position, Quaternion.identity, Vector3.one);
                f2.Toy.TextFormat = "F2 tombe à l'eau";

                




            }


        }



        public override void OnDisabled()
        {
            handler?.UnsubscribeEvents();
            Exiled.Events.Handlers.Map.Generated -= OnGenerated;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;

            harmony.UnpatchAll(harmony.Id);
            GamblingRoom.UnsubscribeEvents();
            //MoreRoom.UnsubscribeEvents();
            handler = null;
            Instance = null;
        }



        private void OnGenerated()
        {

            Door lcz173 = Door.Get(Exiled.API.Enums.DoorType.Scp173Gate);
            HashSet<DroppableItem> normal = new()
            {
                new(ItemType.KeycardO5,1,2),
                new(ItemType.Jailbird,1,2),
                new(ItemType.SCP268,1,1),
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


            //var g = new GamblingRoom(RoleTypeId.Scp173.GetRandomSpawnLocation().Position + Vector3.down, new(normal));

            var g = new OldGamblingRoom(RoleTypeId.Scp173.GetRandomSpawnLocation().Position + Vector3.down, Vector3.one*10, new LootTable(normal));

            


            /*

            if (Config.Debug)
            {
                var door = KEDoor.Create(null, RoleTypeId.Scp049.GetRandomSpawnLocation().Position, new());
                var d2 = KEDoor.Create(null, RoleTypeId.Scp049.GetRandomSpawnLocation().Position + Vector3.forward * 2, new());
                door.LinkOtherDoor(d2);
            }

            
            //Blinking Blocks
            HashSet<BlinkingBlock> list = new()
            {
                new BlinkingBlock(new(19, 300, -44), new(), new(2, .5f, 2), BlockColor.Red),
                new BlinkingBlock(new(19, 300, -38), new(), new(2, .5f, 2), BlockColor.Blue)
            };
            
            BlinkingBlocksGroup group = new(list);
            Timing.RunCoroutine(ShowPos());
            */

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


    }
}
