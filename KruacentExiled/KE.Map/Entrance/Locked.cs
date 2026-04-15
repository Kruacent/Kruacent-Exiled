using Exiled.API.Enums;
using Exiled.API.Features;
using Interactables.Interobjects.DoorUtils;
using KE.Utils.API.Map;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Map.Entrance
{
    public class Locked : MoreRoom
    {
        public override RoomType RoomType => RoomType.EzVent;

        public override int Limit => 1;

        private PrimitiveObjectToy primitive;

        public override void Create(Vector3 position, Quaternion rotation)
        {
            Log.Debug("create");
            DoorVariant door1 = StructureSpawner.SpawnDoor(ProjectMER.Features.Enums.DoorType.Gate, position, rotation, new Vector3(.6f, 1.1f, .6f));
            Door door = Door.Get(door1);

            door.Lock(DoorLockReason.AdminCommand, true);
            primitive = PrimitiveObjectToy.Create(door.Transform,false);
            primitive.Transform.localPosition = Vector3.up * 2;
            primitive.Type = PrimitiveType.Cube;
            primitive.Flags = AdminToys.PrimitiveFlags.Collidable;
            primitive.Scale =  new Vector3(6,5,1);
            primitive.Spawn();
        }

        public override void Destroy()
        {
            primitive.Destroy();
        }
    }
}
