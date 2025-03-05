using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Pickups;
using KE.Items.Interface;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using Pickup = InventorySystem.Items.Pickups.ItemPickupBase;

namespace KE.Items.Lights
{
    internal class LightsHandler
    {
        public float Intensity { get; set; } = .5f;
        private readonly Dictionary<Pickup, Light> pl = new Dictionary<Pickup, Light>();
        public void SubscribeEvents()
        {
            ItemPickupBase.OnPickupAdded += AddPickup;
            ItemPickupBase.OnPickupDestroyed += DestroyPickup;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        }

        public void UnsubscribeEvents()
        {
            ItemPickupBase.OnPickupAdded -= AddPickup;
            ItemPickupBase.OnPickupDestroyed -= DestroyPickup;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        }

        private void OnRoundStarted()
        {
            Timing.RunCoroutine(LightP());
        }


        private void AddPickup(ItemPickupBase pickup)
        {
            if (Check(pickup))
            {
                pl.Add(pickup, null);
            }
        }
        private void DestroyPickup(ItemPickupBase pickup)
        {
            
            if (pickup == null) return;

            if (pl.ContainsKey(pickup))
            {
                Light val = pl[pickup];
                val?.Destroy();
                pl.Remove(pickup);
            }
        }

        

        private IEnumerator<float> LightP()
        {
            while (true)
            {
                try
                {


                    foreach (var x in pl.ToList())
                    {
                        if (x.Key == null)
                        {
                            pl.Remove(x.Key);
                            continue;
                        }
                        if (CustomItem.TryGet(Exiled.API.Features.Pickups.Pickup.Get(x.Key), out CustomItem cui) && cui is ILumosItem ci)
                        {
                            if (x.Key == null) continue;
                            Light light = Light.Create(x.Key.Position, null, null, true, ci.Color);
                            light.Intensity = Intensity;
                            if (x.Value != null)
                            {
                                Light val = x.Value;
                                val?.Destroy();
                            }
                            pl[x.Key] = light;
                        }
                        else
                        {
                            Light val = x.Value;
                            val?.Destroy();
                            pl.Remove(x.Key);
                        }
                    }
                }
                catch (Exception e)
                {
                    
                }
                yield return Timing.WaitForSeconds(MainPlugin.Instance.Config.RefreshRate);
            }

        }

        public static bool Check(Pickup pickup)
        {
            return CustomItem.TryGet(Exiled.API.Features.Pickups.Pickup.Get(pickup), out CustomItem item) && item is ILumosItem;
        }
        

    }
}
