using Exiled.API.Interfaces;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Map.Others.CustomElevators
{
    public class VanillaElevator : KEElevator, IWrapper<Elevator>
    {

        public Elevator Base { get; }


        public override bool IsReady => Base.IsReady;
        public override void Send()
        {
            Base.SendToNextFloor();
        }


        public VanillaElevator(Elevator elevator) : base()
        {
            Base = elevator;
        }
    }
}
