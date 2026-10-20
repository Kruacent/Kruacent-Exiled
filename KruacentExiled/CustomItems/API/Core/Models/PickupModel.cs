using AdminToys;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using InventorySystem.Items.Pickups;
using KE.Utils.API.Features;
using KE.Utils.API.Features.Models;
using KE.Utils.API.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace KruacentExiled.CustomItems.API.Core.Models
{
    /// <summary>
    /// Models of <see cref="Pickup"/>s
    /// </summary>
    public abstract class PickupModel : ModelBase, IUsingEvents
    {
        /// <summary>
        /// The <see cref="CustomItem"/> of the model
        /// </summary>
        public CustomItem KECI { get; }
        /// <summary>
        /// The scale of the model (1 is default)
        /// </summary>
        public abstract float Scale { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="PickupModel"/>
        /// </summary>
        /// <param name="customItem">The custom item which gets the model</param>
        public PickupModel(CustomItem customItem)
        {
            KECI = customItem;
        }

        public void SubscribeEvents()
        {
            PickupToParent = new Dictionary<Pickup, Primitive>();
            
            ItemPickupBase.OnPickupAdded += OnPickupAdded;
            ItemPickupBase.OnPickupDestroyed += OnPickupDestroyed;
        }

        public void UnsubscribeEvents()
        {
            ItemPickupBase.OnPickupAdded -= OnPickupAdded;
            ItemPickupBase.OnPickupDestroyed -= OnPickupDestroyed;
            PickupToParent = null;
        }

        private Dictionary<Pickup, Primitive> PickupToParent;

        protected override PrimitiveObjectToy CreatePrimitive(Transform parent, PrimitiveType type, Vector3 localPosition, Quaternion localRotation, Vector3 localScale, Color32 color, byte movementSmoothing = 0, bool collidable = false)
        {
            return base.CreatePrimitive(parent, type, localPosition, localRotation, localScale, color, 0, false);
        }

        /// <summary>
        /// Create the parent primitive of the model
        /// </summary>
        /// <param name="pickup"></param>
        /// <returns></returns>
        private Primitive CreateParent(Pickup pickup)
        {


            Transform parent = pickup.Transform;
            Primitive prim = Primitive.Create(null, null, null, false);
            prim.Flags = PrimitiveFlags.None;

            prim.Transform.parent = parent;

            Vector3 parentScale = parent.localScale;
            prim.Transform.localPosition = Vector3.zero;
            prim.Transform.localRotation = Quaternion.identity;
            prim.Transform.localScale = new Vector3(1f/ parentScale.x,1f/ parentScale.y,1f/parentScale.z);
            prim.Transform.localScale *= Scale;
            prim.MovementSmoothing = 0;

            prim.Spawn();
            PickupToParent.Add(pickup, prim);

            return prim;
        }

        public void OnPickupAdded(ItemPickupBase pickupBase)
        {
            Pickup pickup = Pickup.Get(pickupBase);
            if (!Check(pickup)) return;

            Transform parent = CreateParent(pickup).Transform;
            CreateModel(parent);

        }

        public void OnPickupDestroyed(ItemPickupBase pickupBase)
        {
            Pickup pickup = Pickup.Get(pickupBase);
            if (!Check(pickup)) return;

            if(PickupToParent.TryGetValue(pickup,out Primitive prim))
            {

                KELog.Debug("destroying parent of " + pickup);
                Destroy(prim.Transform);
                PickupToParent.Remove(pickup);
            }
            
        }
        /// <summary>
        /// Check if the pickup can have this model
        /// </summary>
        /// <param name="pickup"></param>
        /// <returns>true if the pickup can use this model ; false otherwise</returns>
        public bool Check(Pickup pickup)
        {
            if (pickup is null) return false;
            if (!CustomItem.TryGet(pickup, out CustomItem ci)) return false;
            if (ci != KECI) return false;

            return true;

        }

    }
}
