using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using KE.Items.API.Features;
using KE.Items.API.Interface;
using KE.Items.ItemEffects;
using KE.Items.Items.ItemEffects;

namespace KE.Items.Items
{
    public class LowGravityGrenade : KECustomGrenade, ISwichableEffect
    {

        public override ItemType ItemType => ItemType.GrenadeHE;
        public override string Name { get; set; } = "Low Gravity Grenade";
        public override string Description { get; set; } = "You always wanna be on the moon, if the answer is yes this grenade will grant your wishes!";
        public override float Weight { get; set; } = 0.65f;
        public override float FuseTime => 3f;
        public override bool ExplodeOnCollision => false;
        public UnityEngine.Color Color { get; set; } = UnityEngine.Color.gray;
        public CustomItemEffect Effect { get; set; }
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 2,
            RoomSpawnPoints = new List<RoomSpawnPoint>
            {
                new RoomSpawnPoint()
                {
                    Room = RoomType.Hcz127,
                    Chance = 25
                },

                new RoomSpawnPoint()
                {
                    Room = RoomType.Hcz939,
                    Chance = 25
                },
            },

            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new DynamicSpawnPoint()
                {
                    Chance = 25,
                    Location = SpawnLocationType.InsideHidLab,
                },
            },

        };

        public LowGravityGrenade()
        {
            Effect = new LowGravityGrenadeEffect();
        }

        protected override void OnExplodingGrenade(ExplodingGrenadeEventArgs ev)
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