using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using InventorySystem.Items.Pickups;
using KE.Utils.API.Interfaces;
using KruacentExiled.CustomItems.API.Interface;
using LabApi.Features.Wrappers;

namespace KruacentExiled.CustomItems.API.Core.Lights
{
    internal class LightsHandler : IUsingEvents
    {
        public float Intensity { get; set; } = .5f;
        public void SubscribeEvents()
        {
            ItemPickupBase.OnPickupAdded += AddPickup;
        }

        public void UnsubscribeEvents()
        {
            ItemPickupBase.OnPickupAdded -= AddPickup;
        }


        private void AddPickup(ItemPickupBase pickup)
        {
            if (CustomItem.TryGet(Exiled.API.Features.Pickups.Pickup.Get(pickup), out CustomItem item) && item is ILumosItem li)
            {

                var l = LightSourceToy.Create(pickup.transform, false);
                l.Color = li.Color;
                l.Intensity = Intensity;
                l.MovementSmoothing = 0;
                l.Spawn();
            }

        }
    }
}
