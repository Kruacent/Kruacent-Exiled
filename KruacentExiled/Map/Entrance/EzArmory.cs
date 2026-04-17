using Exiled.API.Enums;
using Exiled.API.Features;
using Interactables.Interobjects.DoorUtils;
using KE.Utils.API.Map;
using LabApi.Events.Arguments.ServerEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.Map.Entrance
{
    public class EzArmory : MoreRoom
    {
        public override RoomType RoomType => RoomType.EzVent;

        public override int Limit => 1;


        public override void Create(Vector3 position, Quaternion rotation)
        {
            StructureSpawner.SpawnDoor(ProjectMER.Features.Enums.DoorType.Ez, position,rotation,new Vector3(2f,1,1f));
        }

        public override void Destroy()
        {
            
        }
    }
}
