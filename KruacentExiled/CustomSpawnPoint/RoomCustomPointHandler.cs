using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Spawn;
using KruacentExiled.CustomSpawnPoint.Spawned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.CustomSpawnPoint
{
    public static class RoomCustomPointHandler
    {


        public static RoomSpawnedCustomPoint GetRandomPoint(RoomCustomPointType type, ZoneType zone)
        {
            return GetRandomPoint(type,Room.Get(zone).GetRandomValue());
        }

        public static RoomSpawnedCustomPoint GetRandomPoint(RoomCustomPointType type, Room room)
        {
            return GetAll(room,type).GetRandomValue();
        }

        public static RoomSpawnedCustomPoint GetRandomPoint(RoomCustomPointType type, RoomType roomType)
        {

            List<Room> rooms = Room.Get(r => r.Type == roomType).ToList();



            List<RoomSpawnedCustomPoint> points = new List<RoomSpawnedCustomPoint>();

            foreach(Room room in rooms)
            {
                points.AddRange(GetAll(room,type));
            }
            

            //get not used list

            return null;
        }


        /// <summary>
        /// Get all <see cref="RoomSpawnedCustomPoint"/> of a <see cref="Room"/>
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static IEnumerable<RoomSpawnedCustomPoint> GetAll(Room room)
        {
            return RoomSpawnedCustomPoint.Dictionary[room];
        }


        public static bool HaveASpawnedPoint(Room room)
        {
            return RoomSpawnedCustomPoint.Dictionary.ContainsKey(room);
        }

        /// <summary>
        /// Get all <see cref="RoomSpawnedCustomPoint"/> of a <see cref="Room"/> with a specific <see cref="RoomCustomPointType"/>
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static IEnumerable<RoomSpawnedCustomPoint> GetAll(Room room,RoomCustomPointType type)
        {
            if (!HaveASpawnedPoint(room))
            {
                return null;
            }


            return RoomSpawnedCustomPoint.Dictionary[room].Where(r => r.type == type);
        }

    }
}
