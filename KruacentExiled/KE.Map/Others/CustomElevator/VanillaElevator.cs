using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Map.Others.CustomElevator
{
    public class VanillaElevator : KEElevator
    {

        public Elevator Elevator { get; }


        public override bool IsReady => Elevator.IsReady;
        public override void Send()
        {
            Elevator.SendToNextFloor();
        }


        public VanillaElevator(Elevator elevator) : base()
        {
            Elevator = elevator;
        }
    }
}
