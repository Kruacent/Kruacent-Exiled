using Exiled.API.Interfaces;
using KruacentExiled.Map.Surface.ElevatorGateA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.Map.Others.CustomElevators
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
