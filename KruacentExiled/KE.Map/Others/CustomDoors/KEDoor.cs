using KE.Map.Others.CustomElevators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Map.Others.CustomDoors
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
