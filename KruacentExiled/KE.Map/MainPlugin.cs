
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using KE.Map.Doors;
using KE.Map.EasterEggs;
using KE.Map.GamblingZone;
using KE.Map.Surface.BlinkingBlocks;
using KE.Map.Surface.SupplyDrops;
using KE.Map.Surface.Turrets;
using KE.Utils.API.Models;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KE.Map
{
    public class MainPlugin : Plugin<Config>
    {
        public static MainPlugin Instance { get; private set; }
        public Models models => Models.Instance;
        public static Config Configs => Instance?.Config;
        private Capybaras Capybaras;
        public override void OnEnabled()
        {

            //Capybaras = new();


            //Capybaras.SubscribeEvents();
            KE.Utils.API.Sounds.SoundPlayer.Instance.TryLoad();
            Exiled.Events.Handlers.Map.Generated += OnGenerated;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
            Exiled.Events.Handlers.Server.RoundStarted += SupplyDrop.OnRoundStarted;
            
            //Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            //Turret.SubscribeEvents();

            //Exiled.Events.Handlers.Server.RoundStarted += SendFakePrimitives.Join;
            //models?.SubscribeEvents();
            
            Instance = this;
        }

        private void OnRoundStarted()
        {

            foreach(Player p in Player.List.Where(p => !p.IsNPC))
            {

                new Turret(p, RoleTypeId.ChaosConscript.GetRandomSpawnLocation().Position);
            }

            
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Map.Generated -= OnGenerated;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Server.RoundStarted -= SupplyDrop.OnRoundStarted;
            //Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            //Exiled.Events.Handlers.Server.RoundStarted -= SendFakePrimitives.Join;
            //Capybaras.UnsubscribeEvents();
            //Turret.UnsubscribeEvents();
            //models.UnsubscribeEvents();
            //models.DestroyInstance();
            Capybaras = null;
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
            
            g.SubscribeEvents();
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

        private IEnumerator<float> ShowPos()
        {
            while (true)
            {


                yield return Timing.WaitForSeconds(2);
            }
        }
        
        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            foreach (var g in GamblingRoom.List)
                g.UnsubscribeEvents();

            foreach (var g in OldGamblingRoom.List)
                g.UnsubscribeEvents();
        }

    }


    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = true;
    }
}
