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
    [CustomItem(ItemType.Flashlight)]
    public class SmokeGrenade : KECustomGrenade, ISwichableEffect
    {
        public override uint Id { get; set; } = 1071;
        public override string Name { get; set; } = "Smoke Grenade";
        public override string Description { get; set; } = "We finally put your grandma inside this thing ! Don't throw it or she will get out !";
        public override float Weight { get; set; } = 0.65f;
        public override float FuseTime { get; set; } = 3f;
        public override bool ExplodeOnCollision { get; set; } = false;
        public UnityEngine.Color Color { get; set; } = UnityEngine.Color.black;
        public CustomItemEffect Effect { get; set; }
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 3,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new DynamicSpawnPoint()
                {
                    Chance = 50,
                    Location = SpawnLocationType.Inside939Cryo,
                },
                new DynamicSpawnPoint()
                {
                    Chance = 5,
                    Location = SpawnLocationType.Inside914,
                },
                new DynamicSpawnPoint()
                {
                    Chance= 50,
                    Location = SpawnLocationType.InsideGateA,
                }
            },

        };

        public SmokeGrenade()
        {
            Effect = new SmokeGrenadeEffect();
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