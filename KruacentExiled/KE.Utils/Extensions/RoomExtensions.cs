using Exiled.API.Features;
using Exiled.API.Enums;
using Discord;
using System.Linq;
using Exiled.API.Extensions;

namespace KE.Utils.Extensions
{
    public static class RoomExtensions
    {


        public static Room RandomSafeRoom(this ZoneType zone)
        {
            return Room.List.Where(r => r.IsSafe() && r.Zone == zone).GetRandomValue();
        }


        /// <summary>
        /// Check if a <see cref="Room"/> is Safe (Decontamination,Warhead)
        /// <para>
        /// Note: Does NOT check if the <see cref="Room"/> is safe to teleport in (ex : TestRoom)
        /// </para>
        /// </summary>
        /// <returns>return true if the <see cref="Room"/> is safe for a <see cref="Player"/> ; false otherwise</returns>
        public static bool IsSafe(this Room room)
        {
            return room.Zone.IsSafe();
        }


        /// <summary>
        /// Check if a <see cref="ZoneType"/> is Safe (Decontamination,Warhead)
        /// </summary>
        /// <returns>return true if the zone is safe for a <see cref="Player"/> ; false otherwise</returns>
        public static bool IsSafe(this ZoneType zone)
        {
            bool result = true;
            if (zone == ZoneType.LightContainment)
                result = Map.DecontaminationState < DecontaminationState.Countdown;
            switch (zone)
            {
                case ZoneType.LightContainment:
                case ZoneType.HeavyContainment:
                case ZoneType.Entrance:
                    result = !Warhead.IsDetonated;
                    break;
            }
            return result;
        }



    }
}
