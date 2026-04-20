using System.Collections.Generic;

namespace KruacentExiled.Map.Others.CustomDoors
{
    public abstract class KEDoor
    {


        public static HashSet<KEDoor> doors = new HashSet<KEDoor>();



        public abstract string NameTag { get; }
        public KEDoor()
        {
            doors.Add(this);
        }
    }
}
