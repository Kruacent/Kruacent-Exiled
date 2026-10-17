using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Interactables.Interobjects.DoorUtils;
using KruacentExiled.GlobalEventFramework.Examples.API.Feature.MF;
using KruacentExiled.GlobalEventFramework.GEFE.API.Enums;
using KruacentExiled.GlobalEventFramework.GEFE.API.Features;
using KruacentExiled.GlobalEventFramework.GEFE.API.Interfaces;
using MapGeneration;
using MEC;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace KruacentExiled.GlobalEventFramework.Examples.GE
{
    /// <summary>
    /// <b>The original</b>
    /// <list type="bullet">
    /// <item>The nuke can go off random from 15 to 30 min in the round (can be disable like a normal nuke)</item>
    /// <item>If BlackoutNDoor is enabled in the server, increase the frequence of blackouts and door lockdowns</item>
    /// <item>Can lock Elevator and Gate for an amount of time</item>
    /// <item>Checkpoints can open randomly</item>
    /// </list>
    /// </summary>
    public class SystemMalfunction : GlobalEvent, IStart
    {


        public override ImpactLevel ImpactLevel => ImpactLevel.VeryLow;
        /// <inheritdoc/>        
        public override string Name { get; set; } = "System Malfunction";
        /// <inheritdoc/>
        public override string Description { get; } = "System Malfunction";
        public override string[] AltDescription => new string[]
        {
            "La facilité marche pas trop là"
        };
        /// <inheritdoc/>
        public override int WeightedChance { get; set; } = 1;



        public float ChanceNukeEveryMinute { get; set; } = 5;

        private CoroutineHandle handleNuke;
        private CoroutineHandle handleCheckpoints;

        /// <inheritdoc/>
        public void Start()
        {
            //5% every minute nuke start
            handleNuke = Timing.RunCoroutine(EarlyNuke());

            // checkpoints auto open starts at light
            handleCheckpoints = Timing.RunCoroutine(AutoCheckpoints());

        }

        public override void Destroy()
        {
            Timing.KillCoroutines(handleNuke, handleCheckpoints);
            base.Destroy();
        }

        private IEnumerator<float> EarlyNuke()
        {
            bool stopped = false;
            while (!stopped)
            {
                yield return Timing.WaitForSeconds(60);
                if (Random.Range(0f, 100f) < ChanceNukeEveryMinute)
                {
                    Warhead.Start();
                    stopped = true;
                }
            }
        }


        private CheckpointDoor RandomDoor(ZoneType zone = ZoneType.Unspecified)
        {
            return Door.List.GetRandomValue(d => d.IsCheckpoint && (zone == ZoneType.Unspecified || d.Zone == zone)) as CheckpointDoor;
        }

        private IEnumerator<float> AutoCheckpoints()
        {
            CheckpointDoor door = RandomDoor(ZoneType.LightContainment);


            while (true)
            {
                yield return Timing.WaitForSeconds(Random.Range(2*60,5*60));
                door.IsOpen = true;
                door = RandomDoor();
            }
        }

        
    }
}