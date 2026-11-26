using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using InventorySystem.Items.Pickups;
using KE.Utils.API.Interfaces;
using KruacentExiled.CustomItems.API.Interface;
using LabApi.Features.Wrappers;
using UnityEngine;

namespace KruacentExiled.CustomItems.API.Core.Lights
{

    /// <summary>
    /// Spawn a <see cref="LightSourceToy"/> at the pickup of any <see cref="ILumosItem"/>
    /// </summary>
    internal class LightsHandler : IUsingEvents
    {

        /// <summary>
        /// The intensity of the <see cref="LightSourceToy"/>
        /// </summary>
        public static float Intensity { get; set; } = .5f;
        public void SubscribeEvents()
        {
            ItemPickupBase.OnPickupAdded += AddPickup;
        }

        public void UnsubscribeEvents()
        {
            ItemPickupBase.OnPickupAdded -= AddPickup;
        }


        public static void AddPickup(ItemPickupBase pickup)
        {
            if (CustomItem.TryGet(Exiled.API.Features.Pickups.Pickup.Get(pickup), out CustomItem item) && item is ILumosItem li)
            {
                ForceAddLight(pickup, li.Color);
            }

        }


        internal static void ForceAddLight(ItemPickupBase pickup,Color color)
        {
            var l = LightSourceToy.Create(pickup.transform, false);
            l.Color = color;
            l.Intensity = Intensity;
            l.MovementSmoothing = 0;
            l.Spawn();
        }
    }
}
