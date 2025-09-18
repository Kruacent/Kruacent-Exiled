using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using InventorySystem.Items.Pickups;
using KE.Utils.API.Models.Blueprints;
using System.Collections.Generic;
using UnityEngine;

namespace KE.Items.Items.PickupModels
{
    public abstract class PickupModel
    {
        public CustomItem KECI { get; }
        private static HashSet<PickupModel> allModels = new();


        private HashSet<AdminToyBlueprint> modelBlueprint = null;
        private Dictionary<ItemPickupBase, HashSet<AdminToy>> models;


        public PickupModel(CustomItem customItem)
        {
            KECI = customItem;
            models = new();
            allModels.Add(this);
        }


        protected abstract HashSet<AdminToyBlueprint> CreateModel();

        protected virtual Vector3 PickupSize { get; set; } = Vector3.one;

        public void SubscribeEvents()
        {
            ItemPickupBase.OnPickupAdded += OnPickupAdded;
            ItemPickupBase.OnPickupDestroyed += OnPickupDestroyed;
        }



        public void UnsubscribeEvents()
        {
            ItemPickupBase.OnPickupDestroyed -= OnPickupDestroyed;
            ItemPickupBase.OnPickupAdded -= OnPickupAdded;

            foreach (HashSet<AdminToy> toys in models.Values)
            {
                foreach (AdminToy toy in toys)
                {
                    toy.Destroy();
                }
            }
        }

        public bool Check(Pickup pickup)
        {
            if (pickup is null) return false;
            if (!CustomItem.TryGet(pickup, out CustomItem ci)) return false;
            if (ci != KECI) return false;

            return true;

        }

        public static bool AnyCheck(Pickup pickup)
        {
            foreach(PickupModel model in allModels)
            {
                
                if (model.Check(pickup))
                {
                    return true;
                }
            }
            return false;
        }

        private void OnPickupAdded(ItemPickupBase obj)
        {

            Pickup pickup = Pickup.Get(obj);
            if (!Check(pickup)) return;
            if(modelBlueprint is null)
            {
                modelBlueprint = CreateModel();
            }

            if(PickupSize != Vector3.one)
            {
                Log.Debug("set size to "+PickupSize);
                pickup.Scale = PickupSize;
            }



            models.Add(obj, new());

            foreach (AdminToyBlueprint blueprint in modelBlueprint)
            {
                //Log.Debug($"adding {obj.name} at ({obj.Position})");
                AdminToy prim = blueprint.Spawn(Vector3.zero);

                if (prim is Primitive p)
                    p.Collidable = false;
                Vector3 offset = prim.Position;


                prim.Transform.parent = obj.transform;
                prim.Transform.localPosition = offset;
                prim.Transform.rotation *= pickup.Rotation;
                prim.MovementSmoothing = 60;
                prim.AdminToyBase.syncInterval = 0f;

                /*
                Log.Debug($"prim trans = ({prim?.Transform.parent?.gameObject.name})");
                Log.Debug($"prim localpos = ({prim?.Transform.localPosition})");
                Log.Debug("+ offset =" + offset);
                Log.Debug($"prim globalpos = ({prim?.Position})");
                */
                prim.Spawn();
                models[obj].Add(prim);

            }


        }

        private void OnPickupDestroyed(ItemPickupBase obj)
        {
            if (!Check(Pickup.Get(obj))) return;

            Log.Debug("Destroyed " + obj.name);
            if (models.TryGetValue(obj, out HashSet<AdminToy> model))
            {
                foreach (AdminToy toy in model)
                {
                    toy.Destroy();
                }
            }
        }
    }
}
