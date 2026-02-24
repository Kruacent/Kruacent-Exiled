using CustomPlayerEffects;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Map.Others.CustomElevator
{
    public abstract class KEElevator
    {

        public static HashSet<KEElevator> elevators = new();



        public abstract bool IsReady { get; }
        public abstract void Send();



        public KEElevator()
        {
            elevators.Add(this);
        }

    }
}
