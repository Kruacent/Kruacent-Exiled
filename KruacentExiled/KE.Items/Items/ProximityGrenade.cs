using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using KE.Items.Features;
using KE.Items.Interface;
using KE.Items.ItemEffects;

namespace KE.Items.Items
{
    [CustomItem(ItemType.GrenadeFlash)]
    public class ProximityGrenade : KECustomGrenade, ISwichableEffect
    {
        public override uint Id { get; set; } = 1073;
        public override string Name { get; set; } = "Proximity Grenade";
        public override string Description { get; set; } = "It will show line to all players arround 3 rooms";
        public override float Weight { get; set; } = 0.65f;
        public override float FuseTime { get; set; } = 3f;
        public override bool ExplodeOnCollision { get; set; } = false;
        public UnityEngine.Color Color { get; set; } = UnityEngine.Color.red;
        public CustomItemEffect Effect { get; set; }
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
            RoomSpawnPoints = new List<RoomSpawnPoint>
            {
                new RoomSpawnPoint()
                {
                    Room = RoomType.HczElevatorA,
                    Offset = new UnityEngine.Vector3(1f, 0f, 1f),
                    Chance = 50
                }
            },

        };

        public ProximityGrenade()
        {
            Effect = new ProximityGrenadeEffect();
        }

        protected override void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            Effect.Effect(ev);
            ev.TargetsToAffect.Clear();
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
        }
    }
}