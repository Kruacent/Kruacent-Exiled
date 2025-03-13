using Exiled.API.Features.Pickups;
using Exiled.API.Features.Toys;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Player;
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

        private InteractiblePickup _ipickup;

        internal DoorButton(Vector3 position,Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
            _ipickup = new(ItemType.Medkit, Position,Vector3.one, Rotation,false);

            Primitive pr = Primitive.Create(PrimitiveType.Cube, Position, null, _ipickup.GetPickupTrueSize() + new Vector3(.1f, .1f, .1f));
            pr.Collidable = false;

        }


        
    }
}
