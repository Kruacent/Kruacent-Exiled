using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.CustomItems.API.Features;
using InventorySystem.Items.Pickups;
using KE.Items.Interface;
using KE.Utils.Quality.Models;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pickup = InventorySystem.Items.Pickups.ItemPickupBase;


namespace KE.Items.PickupModels
{
    internal class PickupQuality
    {
        public const float RefreshRate = .1f;
        private readonly Dictionary<Pickup, Model> pl = new ();
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
            Timing.RunCoroutine(ModelLoop());

        }


        private void AddPickup(ItemPickupBase pickup)
        {
            
            if (CustomItem.TryGet(Exiled.API.Features.Pickups.Pickup.Get(pickup), out CustomItem cui) && cui is ICustomPickupModel ci)
            {
                pl.Add(pickup, null);
            }
        }
        private void DestroyPickup(ItemPickupBase pickup)
        {
            
            if (pickup == null) return;

            if (pl.ContainsKey(pickup))
            {
                Model val = pl[pickup];
                val?.Destroy();
                pl.Remove(pickup);
            }
        }



        private IEnumerator<float> ModelLoop()
        {
            while (true)
            {
                try
                {
                    foreach (var x in pl.ToList())
                    {
                        Pickup pickup = x.Key;
                        Model oldModel = x.Value;
                        if (pickup == null)
                        {
                            pl.Remove(x.Key);
                            continue;
                        }
                        if (CustomItem.TryGet(Exiled.API.Features.Pickups.Pickup.Get(pickup), out CustomItem cui) && cui is ICustomPickupModel ci)
                        {
                            if (ci.PickupModel == null)
                            {
                                continue;
                            }
                            ModelPrefab modelpre = ci.PickupModel;



                            if (oldModel != null)
                            {

                                if (oldModel.Prefab == ci.PickupModel && oldModel.Position == pickup.Position && oldModel.Rotation == pickup.Rotation)
                                {
                                    continue;
                                }
                                oldModel.Destroy();

                            }
                            Model model = modelpre.Create(pickup.Position, pickup.Rotation);


                            pl[x.Key] = model;
                        }
                        else
                        {
                            Model val = x.Value;
                            val?.Destroy();
                            pl.Remove(x.Key);
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
                yield return Timing.WaitForSeconds(RefreshRate);

            }

        }

        
         

        public static bool Check(Pickup pickup)
        {
            return CustomItem.TryGet(Exiled.API.Features.Pickups.Pickup.Get(pickup), out CustomItem item) && item is ICustomPickupModel;
        }
    }
}
