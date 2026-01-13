using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Interactables.Interobjects.DoorUtils;
using KE.Utils.API.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Map.Heavy
{
    public static class BulkDoor049
    {

        public static void Create()
        {


            Room room = Room.List.Where(r => r.Type == RoomType.Hcz049).First();


            Vector3 pos = new(-19.7f ,89f, 9f);
            Quaternion rot = Quaternion.identity;

            Vector3 worldpos = room.Position + room.Rotation*pos;



            DoorVariant door = StructureSpawner.SpawnDoor(ProjectMER.Features.Enums.DoorType.HeavyBulkDoor, worldpos, room.Rotation * rot , Vector3.one,ZoneType.HeavyContainment);


            Door doorExiled = Door.Get(door);
            foreach(var k in StructureSpawner.AdditionalDoors)
            {
                Log.Debug(k.Key);
                foreach(DoorVariant doorv in k.Value)
                {
                    Log.Debug(doorv);
                }
            }


            Log.Debug("spawn 049 bulk door at "+ worldpos);
        }



    }
}
