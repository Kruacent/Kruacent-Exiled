using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;

namespace KruacentExiled.CustomSpawnPoint.Spawned
{
    public class RoomCustomItemPoint : RoomSpawnedCustomPoint
    {
        private static List<RoomCustomItemPoint> _usable = new List<RoomCustomItemPoint>();
        internal RoomCustomItemPoint(RoomCustomPoint roomCustomPoint, Room room, bool temporary = false) : base(roomCustomPoint, room, temporary)
        {
            _usable.Add(this);
        }
        protected override void OnDestroy()
        {
            _usable.Remove(this);
        }
        public static RoomCustomItemPoint UseRandom(RoomType roomType)
        {
            if (_usable.Count(r => r.Room.Type == roomType) <= 0)
            {
                return null;
            }
            RoomCustomItemPoint result = _usable.GetRandomValue(r => r.Room.Type == roomType);
            _usable.Remove(result);
            return result;
        }
    }
}
