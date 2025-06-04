using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Toys;
using Exiled.API.Interfaces;
using Exiled.Events;
using Exiled.Events.EventArgs.Player;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.Pickups;
using KE.Map.Doors.KEDoorTypes;
using KE.Map.Utils;
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
    public class KEDoor : IWorldSpace, IDoorPermissionRequester
    {

        internal static readonly HashSet<KEDoor> _list = new();

        public static HashSet<KEDoor> List => new(_list);


        public string RequesterLogSignature
        {
            get
            {
                return "";
            }
        }
        public DoorPermissionsPolicy PermissionsPolicy { get; } = new(DoorPermissionFlags.ContainmentLevelTwo, true);

        public Vector3 Position { get; }
        public Quaternion Rotation { get; }
        //0 -> closed ; 1 -> open
        private float _exactState =0f;
        private DoorButton _button;
        //interacting door
        private InteractiblePickup _pickup;
        private CoroutineHandle _handle;
        private KEDoorType _doorType;
        public KEDoor OtherDoor
        {
            get { return _otherDoor; }
        }

        private KEDoor _otherDoor;
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
            _list.Add(this);
            Position = position;
            Rotation = rotation;
            _button = new(position+position* rotation.eulerAngles.y, rotation);
        }

        public static KEDoor Create(KEDoorType doorType, Vector3 position,Quaternion rotation)
        {
            KEDoor door = new(position,rotation);

            door._doorType = doorType ?? new NormalKEDoor();
            door._doorType.Spawn(position,rotation);

            door._pickup = new(ItemType.Medkit,position,Vector3.one,0,null,false);
            door._pickup.AddAction(door.UsingDoor);

            door._button._ipickup.AddAction(door.UsingDoor);
            door._button.IsOpen = door.IsOpen;
                
            door._handle = Timing.RunCoroutine(door.Detect());
            return door;
        }


        public void Destroy()
        {
            Timing.KillCoroutines(_handle);
            _button.Destroy();
            _pickup.Destroy();

        }



        public void UsingDoor(Player player)
        {
            Log.Debug("using door");
            bool flag = PermissionsPolicy.CheckPermissions(player.ReferenceHub, this, out PermissionUsed callback);

            if(flag)
                ChangeDoorState();
        }

        public IEnumerator<float> Detect()
        {


            yield return Timing.WaitForOneFrame;
        }


        public void ChangeDoorState()
        {
            
            IsOpen = !IsOpen;
            _button.IsOpen = IsOpen;
        }

        public void LinkOtherDoor(KEDoor otherDoor)
        {
            otherDoor._otherDoor = this;
            _otherDoor = otherDoor;
        }

    }
}
