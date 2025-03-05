using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using KE.Items.Interface;
using Player = Exiled.API.Features.Player;
using MEC;
using UnityEngine;
using KE.Items.ItemEffects;

namespace KE.Items.Items
{
    [CustomItem(ItemType.GrenadeFlash)]
    public class HealZone : CustomGrenade, ILumosItem, ISwichableEffect
    {
        public override uint Id { get; set; } = 1051;
        public override string Name { get; set; } = "Heal Zone";
        public override string Description { get; set; } = "Allow to heal you and your ally";
        public override float Weight { get; set; } = 0.65f;
        public override float FuseTime { get; set; } = 5f;
        public override bool ExplodeOnCollision { get; set; } = true;
        public UnityEngine.Color Color { get; set; } = UnityEngine.Color.green;
        public CustomItemEffect Effect { get; set; }
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 3,
            LockerSpawnPoints = new List<LockerSpawnPoint>
            {
                new LockerSpawnPoint()
                {
                    Chance = 75,
                    UseChamber = true,
                    Type = LockerType.Medkit,
                    Zone = ZoneType.Entrance,
                },
                new LockerSpawnPoint()
                {
                    Chance = 50,
                    UseChamber = true,
                    Type = LockerType.Medkit,
                    Zone = ZoneType.LightContainment,
                },
                new LockerSpawnPoint()
                {
                    Chance = 100,
                    UseChamber = true,
                    Type = LockerType.Medkit,
                    Zone = ZoneType.HeavyContainment,
                },
            },

            RoomSpawnPoints = new List<RoomSpawnPoint>
            {
                new RoomSpawnPoint()
                {
                    Chance = 75,
                    Room = RoomType.HczHid,
                },
                new RoomSpawnPoint()
                {
                    Chance = 50,
                    Room = RoomType.HczNuke,
                },
            },
        };

        public HealZone()
        {
            Effect = new HealZoneEffect();
        }

        protected override void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            
            Effect.Effect(ev);
            ev.TargetsToAffect.Clear();
        }
    }
}