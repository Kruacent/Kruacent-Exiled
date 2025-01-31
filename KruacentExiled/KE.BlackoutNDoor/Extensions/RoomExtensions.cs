using Exiled.API.Features;
using Exiled.API.Enums;
using Discord;

namespace KE.BlackoutNDoor.Extensions
{
    public static class RoomExtensions
    {
        public static bool IsSafe(this Room room)
        {
            return IsSafe(room.Zone);
        }


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
