using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Lockers;
using Exiled.API.Features.Pickups;
using KruacentExiled.GlobalEventFramework.GEFE.API.Enums;
using KruacentExiled.GlobalEventFramework.GEFE.API.Features;
using KruacentExiled.GlobalEventFramework.GEFE.API.Interfaces;
using MEC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.GlobalEventFramework.Examples.GE
{
    public class THEWORLDREVOLVING : GlobalEvent, IAsyncStart
    {
        ///<inheritdoc/>
        public override string Name { get; set; } = "THEWORLDREVOLVING";
        ///<inheritdoc/>
        public override string Description { get; } = "THE WORLD REVOLVING";
        ///<inheritdoc/>
        public override int WeightedChance => 0; //2
        public override ImpactLevel ImpactLevel => ImpactLevel.Medium;



        private static readonly TimeSpan MinBreakTime = new TimeSpan(0 , 5, 0);
        private static readonly TimeSpan MaxBreakTime = new TimeSpan(0, 10, 0);
        private static readonly TimeSpan MinDurationTime = new TimeSpan(0, 0, 10);
        private static readonly TimeSpan MaxDurationTime = new TimeSpan(0, 0, 30);
        private const float Speed = 100;
        public IEnumerator<float> Start()
        {
            //play song low volume
            //increase volume when break time is over
            //spin
            //decrease volume when duration is over
            //repeat

            Log.Info("sping");

            TimeSpan duration = MaxDurationTime; //todo random
            TimeSpan breakTime = MaxBreakTime; //todo random
            Stopwatch stopwatch = new Stopwatch();
            while (true)
            {
                stopwatch.Start();
                while (stopwatch.Elapsed < duration)
                {
                    try
                    {
                        SpinAllPickup();
                        SpinAllPlayer();
                    }
                    catch(Exception e)
                    {
                        Log.Error(e);
                    }
                    
                    yield return Timing.WaitForSeconds(1/Speed);
                }
                stopwatch.Reset();
                yield return Timing.WaitForSeconds((float)breakTime.TotalSeconds);
            }


        }


        private void SpinAllPickup()
        {
            foreach (Pickup pickup in Pickup.List)
            {
                
                if(pickup != null && pickup.IsSpawned && pickup.Transform != null)
                {
                    pickup.Rotation *= Quaternion.Euler(0f, 3f, 0f);
                }

                
            }
        }

        private void SpinAllPlayer()
        {
            foreach (Player player in Player.List)
            {
                player.Rotation *= Quaternion.Euler(0f, 3f, 0f);
            }
        }

    }
}
