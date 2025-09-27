using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using InventorySystem.Items.Pickups;
using KE.Utils.API.Models.Blueprints;
using InteractableToy = LabApi.Features.Wrappers.InteractableToy;
using System.Collections.Generic;
using UnityEngine;
using LabPlayer = LabApi.Features.Wrappers.Player;

namespace KE.Items.Features
{
    public abstract class PickupModel
    {
        public CustomItem KECI { get; }
        private static HashSet<PickupModel> allModels = new();


        private HashSet<AdminToyBlueprint> modelBlueprint = null;
        private Dictionary<ItemPickupBase, HashSet<AdminToy>> models;
        private Dictionary<ItemPickupBase, HashSet<InteractableToy>> pickableItem;


        public PickupModel(CustomItem customItem)
        {
            KECI = customItem;
            models = new();
            pickableItem = new();
            allModels.Add(this);
        }


        protected abstract HashSet<AdminToyBlueprint> CreateModel();

        protected virtual bool HidePickup { get; } = false;


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

            foreach (HashSet<InteractableToy> toys in pickableItem.Values)
            {
                foreach (InteractableToy toy in toys)
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
            foreach (PickupModel model in allModels)
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
            if (modelBlueprint is null)
            {
                modelBlueprint = CreateModel();
            }

            if (HidePickup)
            {
                pickup.Scale = new Vector3(.01f, .01f, .01f);
            }
            
            Vector3 scale = pickup.Scale;

            pickableItem.Add(obj, new());

            models.Add(obj, new());

            foreach (AdminToyBlueprint blueprint in modelBlueprint)
            {
                //Log.Debug($"adding {obj.name} at ({obj.Position})");
                AdminToy prim = blueprint.Spawn(Vector3.zero);




                prim.Transform.parent = obj.transform;
                prim.Transform.localScale = new(blueprint.Scale.x/ scale.x, blueprint.Scale.y / scale.y, blueprint.Scale.z / scale.z);
                prim.Transform.localPosition = prim.Position + Vector3.Scale(blueprint.Position,scale);
                prim.Transform.rotation *= pickup.Rotation;
                prim.MovementSmoothing = 60;
                prim.AdminToyBase.syncInterval = 0f;

                Log.Debug("posP=" + prim.Transform.position);
                if (prim is Primitive p)
                {
                    p.Collidable = false;
                    var interact = InteractableToy.Create(prim.Transform, false);

                    interact.Transform.parent = obj.transform;
                    interact.Transform.localPosition = Vector3.zero;
                    interact.Transform.localRotation = Quaternion.identity;
                    interact.Scale = new(blueprint.Scale.x / scale.x, blueprint.Scale.y / scale.y, blueprint.Scale.z / scale.z);
                    interact.InteractionDuration = 0.245f + 0.175f * obj.Info.WeightKg;
                    interact.Shape = AdminToys.InvisibleInteractableToy.ColliderShape.Box;


                    Log.Debug("scale intec : " + interact.Scale);
                    Log.Debug("posI=" + interact.Transform.position);
                    pickableItem[obj].Add(interact);
                    interact.Spawn();
                }
                    
                




                
                Log.Debug("scale prim : " + prim.Transform.localScale);

                

                //interact.OnSearched += (player) => GiveCI(obj, player);
                
                models[obj].Add(prim);
                prim.Spawn();

                
                
            }
        }

        private void GiveCI(ItemPickupBase pickup, LabPlayer player)
        {
            Log.Debug("give");
            KECI.Give(player,true);
            pickup.DestroySelf();
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

            if (pickableItem.TryGetValue(obj, out HashSet<InteractableToy> interact))
            {
                foreach (InteractableToy toy in interact)
                {

                    //toy.OnSearchAborted -= (player) => GiveCI(obj, player);
                    toy.Destroy();
                }
            }

        }
    }
}
