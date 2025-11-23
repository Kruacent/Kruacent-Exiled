using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using KE.Items.API.Features;
using KE.Items.API.Interface;
using KE.Items.Items.ItemEffects;
using UnityEngine;

namespace KE.Items.Items
{
    [CustomItem(ItemType.GrenadeFlash)]
    public class Molotov : KECustomGrenade, ISwichableEffect/*, ICustomPickupModel*/
    {
        //bouteille
        public override uint Id { get; set; } = 1049;
        public override string Name { get; set; } = "Cocktail Molotov";
        public override string Description { get; set; } = "ARSON";
        public override float Weight { get; set; } = 0.65f;
        public override float FuseTime { get; set; } = 5f;
        public override bool ExplodeOnCollision { get; set; } = true;
        public Color Color { get; set; } = Color.yellow;
        public CustomItemEffect Effect { get; set; }
        //public PickupModel PickupModel { get; }
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 2,
            LockerSpawnPoints = new List<LockerSpawnPoint>
            {
                new LockerSpawnPoint()
                {
                    Chance = 75,
                    UseChamber = true,
                    Type = LockerType.Misc,
                    Zone = ZoneType.Entrance,
                },
                new LockerSpawnPoint()
                {
                    Chance = 50,
                    UseChamber = true,
                    Type = LockerType.Misc,
                    Zone = ZoneType.LightContainment,
                },
                new LockerSpawnPoint()
                {
                    Chance = 50,
                    UseChamber = true,
                    Type = LockerType.Misc,
                    Zone = ZoneType.HeavyContainment,
                },
            },

            RoomSpawnPoints = new List<RoomSpawnPoint>
            {
                new RoomSpawnPoint()
                {
                    Chance = 75,
                    Room = RoomType.LczGlassBox,
                },
                new RoomSpawnPoint()
                {
                    Chance = 50,
                    Room = RoomType.HczNuke,
                },
            },
        };


        public Molotov()
        {
            Effect = new MolotovEffect();
            //PickupModel = new MolotovPModel(this);
        }

        protected override void SubscribeEvents()
        {
            //PickupModel.SubscribeEvents();
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            //PickupModel.UnsubscribeEvents();
            base.UnsubscribeEvents();
        }



        protected override void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            
            Effect.Effect(ev);
            ev.TargetsToAffect.Clear();
        }

    }
}