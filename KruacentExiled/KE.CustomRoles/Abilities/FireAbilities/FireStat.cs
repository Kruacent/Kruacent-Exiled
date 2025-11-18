using Exiled.API.Features;
using KE.CustomRoles.API.Features;
using MEC;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.Abilities.FireAbilities
{
    public class FireStat : CustomStatBase
    {

        public float BaseCapacity => MaxValue;

        public override float CurValue { get; set; }

        public override float MinValue => 0;

        private float maxvalue = 100;
        public override float MaxValue
        {
            get
            {
                return maxvalue;
            }
            set
            {
                maxvalue = value;
            }
        }

        public override string Name => "Fire";

        public float FireRegen { get; set; } = 3;


        private float stoptime = 120;
        private float currentstoptime = 0;


        public override void AddAmount(float amount)
        {
            currentstoptime = stoptime;
            base.AddAmount(amount);
        }

        public override void FixedUpdate()
        {

            if (currentstoptime <= 0)
            {
                
                float num = FireRegen * Time.deltaTime;
                if (num > 0f)
                {
                    if (CurValue < MaxValue)
                    {
                        CurValue = Mathf.MoveTowards(CurValue, MaxValue, num);
                    }
                }
                else if (CurValue > 0f)
                {
                    CurValue += num;
                }
            }
            else
            {
                currentstoptime--;
            }
            base.FixedUpdate();
        }
        
    }
}
