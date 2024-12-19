using BlackoutKruacent;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.Commands.Reload;
using Exiled.Loader;
using GEFExiled.GEFE.API.Features;
using GEFExiled.GEFE.API.Utils;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interactables.Interobjects.DoorUtils;
using Exiled.API.Enums;
using Exiled.API.Extensions;

namespace GEFExiled.GEFE.Examples.GE
{

    public class SystemMalfunction : GlobalEvent
    {
        public override int Id { get; set; } = 1;
        public override string Name { get; set; } = "System Malfunction";
        public override string Description { get; set; } = "On dirait que les systèmes informatiques sont défaillants";
        public override double Weight { get; set; } = 1;
        public int NewCooldown { get; set; } = 180;

        public override IEnumerator<float> Start()
        {
            MoreBlackOutNDoors();
            Coroutine.LaunchCoroutine(EarlyNuke());
            
            CoroutineHandle handle;
            while(Round.InProgress){
                //todo change so it happen more frequently 
                Timing.WaitForSeconds((float)Round.ElapsedTime.TotalSeconds);
                List<IEnumerator<float>> l = new []{CheckpointMalfunction(),GateLockdown(),ElevatorLockdown()}.ToList();
                handle = Coroutine.LaunchCoroutine(l[UnityEngine.Random.Range(0,3)]);
                yield return Timing.WaitUntilDone(handle);

            }

        }

        private IEnumerator<float> EarlyNuke()
        {
            int timeNuke = UnityEngine.Random.Range(15, 30);
            Log.Debug($"the nuke will detonate in {timeNuke}min");
            yield return Timing.WaitUntilTrue(() => timeNuke == Round.ElapsedTime.TotalMinutes);
            Warhead.Start();
            Log.Debug($"kaboom");
        }

        private void MoreBlackOutNDoors()
        {
            var otherPlugin = Loader.Plugins.FirstOrDefault(plugin => plugin.Name == "BlackOutNDoors");
            if (otherPlugin != null)
            {
                if (otherPlugin is BlackoutKruacent.MainPlugin blackout)
                {
                    Log.Info("Found BlackOutNDoors");
                    blackout.ServerHandler.Cooldown = NewCooldown;
                }

            }
        }

        private IEnumerator<float> CheckpointMalfunction(){
            yield return Timing.WaitForSeconds(UnityEngine.Random.Range(20,60));
            var checkpoints = Door.List.Where(d => d.IsCheckpoint).ToList().RandomItem().IsOpen =true;
            
        }

        private IEnumerator<float> GateLockdown(){
            var gates = Door.List.Where(d => d.Type == DoorType.GateA ||d.Type == DoorType.GateB);
            var gate = gates.GetRandomValue();
            gate.IsOpen = false;
            var timelock = UnityEngine.Random.Range(10,30);
            gate.Lock(timelock,DoorLockType.Isolation);
            yield return Timing.WaitForSeconds(timelock);
        }

        private IEnumerator<float> ElevatorLockdown(){
            var lift = Lift.Random;
            lift.ChangeLock(DoorLockReason.Isolation);
            yield return Timing.WaitForSeconds(UnityEngine.Random.Range(10,20));
            lift.ChangeLock(DoorLockReason.None);
        }
        
    }
}