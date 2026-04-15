
using AdminToys;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.API.Interfaces;
using HarmonyLib;
using KE.Map.Heavy;
using KE.Map.Heavy.GamblingZone;
using KE.Map.Others.BlackoutNDoor.Handlers;
using KE.Map.Others.CustomZones;
using KE.Map.Surface.ElevatorGateA;
using KE.Map.Surface.Rooms;
using KE.Utils.API.Features;
using KE.Utils.API.KETextToy;
using KE.Utils.Extensions;
using KruacentExiled;
using MEC;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp106;
using ProjectMER.Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static PlayerList;
using Door = Exiled.API.Features.Doors.Door;
using Player = Exiled.API.Features.Player;

namespace KE.Map
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

        private SurfaceRooms SurfaceRooms;
        private CREventHandler cREventHandler;


        private Translations translations;

        public override void OnEnabled()
        {
            Instance = this;
            handler = new Handler();
            harmony = new Harmony(Prefix);

            config = KruacentExiled.MainPlugin.Instance.Config.MapConfig;


            //cREventHandler = new CREventHandler();

            //cREventHandler.SubscribeEvents();
            handler.SubscribeEvents();
            KE.Utils.API.Sounds.SoundPlayer.Instance.TryLoad();
            Exiled.Events.Handlers.Map.Generated += OnGenerated;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;


            //SurfaceRooms.SubscribeEvents();
            
            GamblingRoom.SubscribeEvents();
            //MoreRoom.CreateAll();
            //MoreRoom.SubscribeEvents();
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
                //StructureSpawner.SpawnPedestal(ItemType.KeycardJanitor,player.Position,Quaternion.identity,Vector3.one);

                //player.Teleport(Room.List.Where(r => r.Type == Exiled.API.Enums.RoomType.EzVent).First());


                //player.Teleport(teleport);

                Timing.CallDelayed(2, () =>
                {
                    //player.Teleport(teleport);
                });

                //pos = RoleTypeId.Scp049.GetRandomSpawnLocation().Position;
                pos = player.Position;




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

                //FollowingTextToy f1 = new([player], player.Position, Quaternion.identity, Vector3.one);
                //f1.Toy.TextFormat = "F1 sur un bateau";

                //FollowingTextToy f2 = new([], player.Position, Quaternion.identity, Vector3.one);
                //f2.Toy.TextFormat = "F2 tombe à l'eau";


                // [█ ]
                //[█      ]
                //[█      ]

                //[█                                                            ]\n[██████████]
                /*try
                {
                    Primitive parent = Primitive.Create(PrimitiveType.Cube, pos, spawn: false);
                    parent.Flags = PrimitiveFlags.None;

                    parent.Spawn();

                    Primitive prim = Primitive.Create(PrimitiveType.Cube, null, spawn: false);
                    prim.Transform.parent = parent.Transform;
                    prim.Transform.localPosition = Vector3.zero;
                    prim.Flags = PrimitiveFlags.Visible;
                    prim.MovementSmoothing = 60;
                    prim.Spawn();

                    Log.Info(prim.Position);

                    AnimationClip clip = new AnimationClip();
                    clip.legacy = true;
                    clip.wrapMode = WrapMode.PingPong;

                    Transform t = prim.GameObject.transform;

                    float startX = t.localPosition.x;

                    AnimationCurve curve = AnimationCurve.Linear(
                        0f, startX,
                        3f, startX + 1f
                    );

                    clip.SetCurve("", typeof(Transform), "localPosition.x", curve);

                    Animation animation = prim.GameObject.AddComponent<Animation>();
                    animation.AddClip(clip, "move");
                    animation.Play("move");


                    Log.Debug(animation.IsPlaying("move"));
                    Timing.CallDelayed(3, () =>
                    {
                        Log.Info(prim.Position);
                        player.Position = prim.Position;
                    });
                }
                catch(Exception e)
                {
                    Log.Error(e);
                }

                */


            }


        }



        public override void OnDisabled()
        {
            handler?.UnsubscribeEvents();
            Exiled.Events.Handlers.Map.Generated -= OnGenerated;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;

            cREventHandler?.UnsubscribeEvents();
            cREventHandler = null;


            harmony.UnpatchAll(harmony.Id);
            GamblingRoom.UnsubscribeEvents();
            //MoreRoom.UnsubscribeEvents();
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

            //var g = new OldGamblingRoom(RoleTypeId.Scp173.GetRandomSpawnLocation().Position + Vector3.down, Vector3.one*10, new LootTable(normal));

            


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
