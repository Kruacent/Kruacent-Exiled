using Exiled.API.Features.Pickups;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Pickups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Map.Doors
{
    public class InteractiblePickup
    {
        private HashSet<Action> _actions;
        private ushort _pickupSerial;
        private Pickup _pickup;
        private InteractiblePickup(Pickup pickup)
        {
            _pickupSerial = pickup.Serial;
            _pickup = pickup;
            SubscribeEvent();
        }

        private InteractiblePickup(Pickup pickup,Vector3 sizePickup)
        {
            _pickup = pickup;
            _pickupSerial = pickup.Serial;
            PickupSyncInfo info = pickup.Base.NetworkInfo;
            pickup.Scale = sizePickup;
            pickup.Base.NetworkInfo = info;
            pickup.Base.GetComponent<Rigidbody>().isKinematic = true;
            SubscribeEvent();
        }


        public InteractiblePickup(ItemType itemType, Vector3 position, Vector3 scalePickup, Quaternion? rotation,bool useGravity = false)
        {
            var pickup = Pickup.CreateAndSpawn(itemType, position, rotation);
            _pickup = pickup;
            pickup.Rigidbody.useGravity = useGravity;
            pickup.Rigidbody.detectCollisions = false;


            _pickupSerial = pickup.Serial;
            PickupSyncInfo info = pickup.Base.NetworkInfo;
            pickup.Scale = scalePickup;
            pickup.Base.NetworkInfo = info;
            pickup.Base.GetComponent<Rigidbody>().isKinematic = true;

            SubscribeEvent();
        }





        ~InteractiblePickup() 
        {
            Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingUpItem;
        }

        private void SubscribeEvent()
        {
            Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUpItem;
        }

        public bool AddAction(Action a)
        {
            return _actions.Add(a);
        }

        public void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (ev.Pickup.Serial != _pickupSerial) return;
            ev.IsAllowed = false;

            foreach (var action in _actions)
            {
                action?.Invoke();
            }
        }

        public static Vector3 GetPickupTrueSize(Pickup pickup)
        {
            if (pickup?.GameObject == null)
                return Vector3.zero;

            Renderer renderer = pickup.GameObject.GetComponentInChildren<Renderer>();
            Collider collider = pickup.GameObject.GetComponentInChildren<Collider>();

            if (renderer != null)
                return renderer.bounds.size;

            if (collider != null)
                return collider.bounds.size;

            return Vector3.zero;
        }

        public Vector3 GetPickupTrueSize()
        {
            if (_pickup.GameObject == null)
                return Vector3.zero;

            Renderer renderer = _pickup.GameObject.GetComponentInChildren<Renderer>();
            Collider collider = _pickup.GameObject.GetComponentInChildren<Collider>();

            if (renderer != null)
                return renderer.bounds.size;

            if (collider != null)
                return collider.bounds.size;

            return Vector3.zero;
        }
    }


}
