using KE.Map.Surface.ElevatorGateA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Map.Others.CustomElevator
{
    public class CustomElevator : KEElevator
    {

        public static ElevatorModel model { get; } = new();

        public override bool IsReady => false;

        public override void Send()
        {

        }


        public CustomElevator()
        {
            
        }
    }
}
