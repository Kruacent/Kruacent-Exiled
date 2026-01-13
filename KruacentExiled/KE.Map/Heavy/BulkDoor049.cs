using Exiled.API.Features;
using KE.Utils.API;
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


            Room room = Room.List.Where(r => r.Type == Exiled.API.Enums.RoomType.Hcz049).First();


            Vector3 pos = new(-19.7f ,89f, 9f);
            Quaternion rot = Quaternion.identity;

            Vector3 worldpos = room.Position + room.Rotation*pos;



            StructureSpawner.SpawnDoor(ProjectMER.Features.Enums.DoorType.HeavyBulkDoor, worldpos, room.Rotation * rot , Vector3.one);

            Log.Debug("spawn 049 bulk door at "+ worldpos);
        }



    }
}
