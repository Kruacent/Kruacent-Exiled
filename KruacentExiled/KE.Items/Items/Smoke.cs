using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Spawn;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Items.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Items.Items
{
    [CustomItem(ItemType.GrenadeFlash)]
    public class Smoke : KECustomGrenade
    {
        public override uint Id { get; set; } = 2000;
        public override string Name { get; set; } = "Smoke Grenade";
        public override string Description { get; set; } = "smoke";
        public override bool ExplodeOnCollision { get; set; } = false;
        public override float FuseTime { get; set; } = 5;
        public override float Weight { get; set; } = 0.65f;
        public override SpawnProperties SpawnProperties { get; set; } = new();

        public const float Duration = 20;
        private HashSet<Pickup> pickups = new();

        protected override void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            if (!Check(ev.Projectile)) return;
            Vector3 position = ev.Position;
            Scp244 scp = (Scp244) Scp244.Create(ItemType.SCP244a);
            scp.Primed = true;
            scp.MaxDiameter = float.Epsilon;

            Pickup p = scp.CreatePickup(position, null, false);

            p.Scale = Vector3.one / 10;
            p.Spawn();
            pickups.Add(p);

            Timing.CallDelayed(Duration, delegate
            {
                pickups.Remove(p);
                p.Destroy();
            });
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUpItem;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingUpItem;
            base.UnsubscribeEvents();
        }

        private void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (!pickups.Contains(ev.Pickup)) return;

            ev.IsAllowed = false;

        }
    }
}
