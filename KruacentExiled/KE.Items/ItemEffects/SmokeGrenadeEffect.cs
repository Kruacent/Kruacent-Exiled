using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Items.Extensions;
using KE.Items.Interface;
using MEC;
using System.ComponentModel;
using UnityEngine;

namespace KE.Items.ItemEffects
{
    public class SmokeGrenadeEffect : CustomItemEffect
    {
        public bool RemoveSmoke { get; set; } = true;
        [Description("If RemoveSmoke is true, how long does it take before the smoke will be removed")]
        public float FogTime { get; set; } = 30;

        public override void Effect(UsedItemEventArgs ev)
        {
            OnExploding(ev.Player.Position);
        }
        public override void Effect(DroppingItemEventArgs ev)
        {
            OnExploding(ev.Player.Position);
        }

        public override void Effect(ExplodingGrenadeEventArgs ev)
        {
            OnExploding(ev.Position);
        }

        public void OnExploding(Vector3 position)
        {
            Vector3 savedGrenadePosition = position;
            Scp244 scp244 = (Scp244)Item.Create(ItemType.SCP244a);
            Pickup pickup = null;
            scp244.Scale = new Vector3(0.01f, 0.01f, 0.01f);
            scp244.Primed = true;
            scp244.MaxDiameter = 0.0f;
            pickup = scp244.CreatePickup(savedGrenadePosition);
            if (RemoveSmoke)
            {
                Timing.CallDelayed(FogTime, () =>
                {
                    pickup.Position += Vector3.down * 10;

                    Timing.CallDelayed(10, () =>
                    {
                        pickup.Destroy();
                    });
                });
            }
        }
    }
}