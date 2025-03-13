using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Pickups;
using Exiled.API.Interfaces;
using Exiled.Events;
using Exiled.Events.EventArgs.Player;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.Pickups;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Map.Doors
{
    public class KEDoor : IWorldSpace
    {
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }
        //0 -> closed ; 1 -> open
        private float _exactState =0f;
        private DoorButton[] _buttons = new DoorButton[2];
        //interacting door
        private InteractiblePickup _pickup;
        public KEDoor OtherDoor { get; set; }
        KeycardPermissions KeycardPermissions { get; set; } = KeycardPermissions.None;
        public bool IsOpen
        {
            get { return _exactState == 1f; }
            set
            {
                _exactState = value ? 1f : 0f;
            }
        }

        private KEDoor(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
            _buttons[0] = new(position, rotation);
            _buttons[1] = new(position, rotation);
        }

        public static void Create(Vector3 position,Quaternion rotation)
        {
            KEDoor door = new(position,rotation);

            door._pickup = new(ItemType.Medkit,position,Vector3.one,null,false);

            door._pickup.AddAction(door.ChangeDoorState);
        }

        public void UsingDoor(Player player)
        {
            if (!IsOpen) return;
            if(OtherDoor == null) return;
            
            

        }

        public void ChangeDoorState()
        {
            IsOpen = !IsOpen;
        }




    }
}
