
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Server;
using Interactables.Interobjects.DoorUtils;
using KE.Map.Entrance;
using KE.Map.Heavy.GamblingZone;
using KE.Map.Others.BlackoutNDoor.Handlers;
using KE.Map.Utils;
using KE.Utils.API.Models;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Features.Wrappers;
using PlayerRoles;
using ProjectMER.Features.Serializable.Lockers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Door = Exiled.API.Features.Doors.Door;
using Player = Exiled.API.Features.Player;
using Room = Exiled.API.Features.Room;

namespace KE.Map
{
    public class MainPlugin : Plugin<Config,Translations>
    {
        public override string Name => "KE.Map";
        public override string Prefix => "KE.M";
        public static MainPlugin Instance { get; private set; }
        public Models models => Models.Instance;
        private Handler handler;
        public static Translations Translations => Instance?.Translation;
        public static Config Configs => Instance?.Config;
        public override void OnEnabled()
        {
            handler = new();

            //handler.SubscribeEvents();
            KE.Utils.API.Sounds.SoundPlayer.Instance.TryLoad();
            Exiled.Events.Handlers.Map.Generated += OnGenerated;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            GamblingRoom.SubscribeEvents();
            //MoreRoom.CreateAll();
            //MoreRoom.SubscribeEvents();
            Instance = this;
        }

        private void OnRoundStarted()
        {
            //Player player = Player.List.First();
            //StructureSpawner.SpawnPedestal(ItemType.KeycardJanitor,player.Position,Quaternion.identity,Vector3.one);

            //player.Teleport(Room.List.Where(r => r.Type == Exiled.API.Enums.RoomType.EzVent).First());



            
        }


        public override void OnDisabled()
        {
            //handler.UnsubscribeEvents();
            Exiled.Events.Handlers.Map.Generated -= OnGenerated;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
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
                new(ItemType.SCP500,1),
                new(ItemType.KeycardMTFCaptain,1),
                new(ItemType.SCP268,1,1),
                new(ItemType.GunCOM15,1),
                new(ItemType.SCP207,1),
                new(ItemType.Adrenaline,1),
                new(ItemType.GunCOM18,1),
                new(ItemType.KeycardFacilityManager,1),
                new(ItemType.Medkit,1),
                new(ItemType.KeycardMTFOperative,1),
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
                new(ItemType.Jailbird,1,1),
                ItemType.Flashlight,
                ItemType.AntiSCP207,                            
                new(ItemType.ParticleDisruptor,1,1),
                ItemType.GunCom45,
                ItemType.GunShotgun,
                ItemType.GunRevolver,
                ItemType.GunA7,
            };


            var g = new GamblingRoom(RoleTypeId.Scp173.GetRandomSpawnLocation().Position + Vector3.down, new(normal));


            
            
            //var g = new GamblingRoom(lcz173, new(normal), -lcz173.Transform.forward * 5f);
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



       /* private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            

            foreach (var g in OldGamblingRoom.List)
                g.UnsubscribeEvents();
        }*/

    }


    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public bool SupplyDropEnabled { get; set; } = false;
        public bool TurretEnabled { get; set; } = false;
        public bool EasterEggEnabled { get; set; } = true;


    }
}
