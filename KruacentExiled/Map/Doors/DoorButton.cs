using Exiled.API.Features.Pickups;
using Exiled.API.Features.Toys;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Player;
using KE.Map.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Map.Doors
{
    internal class DoorButton : IWorldSpace
    {
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }

        public bool IsOpen
        {
            get
            {
                return isOpen;
            }
            set
            {
                isOpen = value;
                UpdateColor();
            }
        }
        private bool isOpen;

        internal InteractiblePickup _ipickup { get; }
        private Primitive _primitive;

        internal DoorButton(Vector3 position,Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
            _ipickup = new(ItemType.Medkit, Position, Vector3.one,0, Rotation, false);

            _primitive = Primitive.Create(PrimitiveType.Cube, Position, null, _ipickup.GetPickupTrueSize() + new Vector3(.1f, .1f, .1f));
            _primitive.Collidable = false;

        }
        public void Destroy()
        {
            _primitive.Destroy();
            _ipickup.Destroy();
        }

        private void UpdateColor()
        {
            if (isOpen)
            {
                _primitive.Color = Color.red;
            }
            else
            {
                _primitive.Color = Color.green;
            }
        }
        
    }
}
