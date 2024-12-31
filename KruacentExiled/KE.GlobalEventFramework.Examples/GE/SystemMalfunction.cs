using Exiled.API.Features;
using Exiled.API.Features.Doors;
using KE.GlobalEventFramework.GEFE.API.Features;
using KE.GlobalEventFramework.GEFE.API.Utils;
using MEC;
using System.Collections.Generic;
using System.Linq;
using Interactables.Interobjects.DoorUtils;
using Exiled.API.Enums;
using Exiled.API.Extensions;

namespace KE.GlobalEventFramework.Examples.GE
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
    public class SystemMalfunction : GlobalEvent
    {
        /// <inheritdoc/>
        public override int Id { get; set; } = 1;
        /// <inheritdoc/>        
        public override string Name { get; set; } = "System Malfunction";
        /// <inheritdoc/>
        public override string Description { get; set; } = "On dirait que les systèmes informatiques sont défaillants";
        /// <inheritdoc/>
        public override int Weight { get; set; } = 1;
        /// <summary>
        /// Set the cooldown for the BlackoutNDoor
        /// </summary>
        public int NewCooldown { get; set; } = 180;

        public override int[] IncompatibleGE { get; set; } = { 38 };
        /// <inheritdoc/>
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
        public override void UnsubscribeEvent()
        {
            var otherPlugin = Exiled.Loader.Loader.Plugins.FirstOrDefault(plugin => plugin.Name == "KE.BlackoutDoor");
            if (otherPlugin != null)
            {

                if (otherPlugin is BlackoutNDoor.MainPlugin blackout)
                {
                    blackout.ServerHandler.Cooldown = -1;
                }

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
            var otherPlugin = Exiled.Loader.Loader.Plugins.FirstOrDefault(plugin => plugin.Name == "KE.BlackoutDoor");
            if (otherPlugin != null)
            {
                
                if (otherPlugin is BlackoutNDoor.MainPlugin blackout)
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