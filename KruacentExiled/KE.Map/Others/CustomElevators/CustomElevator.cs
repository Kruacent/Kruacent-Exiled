using Exiled.API.Interfaces;
using KE.Map.Surface.ElevatorGateA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Map.Others.CustomElevators
{
    public class CustomElevator : KEElevator
    {

        public static ElevatorModel model { get; } = new ElevatorModel();

        public override bool IsReady => false;

        public override void Send()
        {

        }


        public CustomElevator()
        {
            
        }
    }
}
