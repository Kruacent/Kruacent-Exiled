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
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using System;
using Exiled.Events.EventArgs.Player;
using System.Security.Policy;
using PlayerRoles;
using Utils.NonAllocLINQ;
using KeycardPermissions = Exiled.API.Enums.KeycardPermissions;
using Exiled.Events.EventArgs.Scp049;
using KE.GlobalEventFramework.Examples.API.Feature;

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
    public class SystemMalfunction : GlobalEvent, IStart, IEvent
    {
        /// <inheritdoc/>
        public override uint Id { get; set; } = 1041;
        /// <inheritdoc/>        
        public override string Name { get; set; } = "System Malfunction";
        /// <inheritdoc/>
        public override string Description { get; set; } = "On dirait que les systèmes informatiques sont défaillants";
        /// <inheritdoc/>
        public override int Weight { get; set; } = 1;
        ///<inheritdoc/>
        public override uint[] IncompatibleGE { get; set; } = { 38 };
        /// <summary>
        /// Set the cooldown for the BlackoutNDoor
        /// </summary>
        public int NewCooldown { get; set; } = 180;
        private BlackoutNDoor.MainPlugin BlackoutNDoor = null;
        public static Malfunctions Malfunction { get; private set; }
        


        /// <inheritdoc/>
        public IEnumerator<float> Start()
        {
            Log.Debug("system malfunction start");
            //MoreBlackOutNDoors();
            //Coroutine.LaunchCoroutine(EarlyNuke());
            
            Coroutine.LaunchCoroutine(Malfunction.Tick());
            CoroutineHandle handle;

            while(Round.InProgress){
                Log.Debug("system malfunction");
                yield return Timing.WaitForSeconds(UnityEngine.Random.Range(200, 300));
                List<IEnumerator<float>> l = new []{CheckpointMalfunction(),GateLockdown(),ElevatorLockdown()}.ToList();
                handle = Coroutine.LaunchCoroutine(l[UnityEngine.Random.Range(0,3)]);
                yield return Timing.WaitUntilDone(handle);
            }
            yield return 0;
        }


        public void SubscribeEvent()
        {
            Malfunction = new Malfunctions();
            Exiled.Events.Handlers.Player.Dying += Malfunction.OnDying;
            Exiled.Events.Handlers.Scp049.FinishingRecall += Malfunction.OnFinishingRevive;


            //Searching for the plugin
            /*var otherPlugin = Exiled.Loader.Loader.Plugins.FirstOrDefault(plugin => plugin.Name == "KE.BlackoutDoor");
            if (otherPlugin != null)
            {

                if (otherPlugin is BlackoutNDoor.MainPlugin blackout)
                {
                    Log.Info("Found BlackOutNDoors");
                    BlackoutNDoor = blackout;
                    return;
                }

            }*/
        }
        public void UnsubscribeEvent()
        {
            BlackoutNDoor = null;
            Exiled.Events.Handlers.Player.Dying -= Malfunction.OnDying;
            Exiled.Events.Handlers.Scp049.FinishingRecall -= Malfunction.OnFinishingRevive;
            Malfunction = null;
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
            try
            {
                if (BlackoutNDoor != null)
                {
                    if (BlackoutNDoor.ServerHandler != null)
                        BlackoutNDoor.ServerHandler.Cooldown = NewCooldown;
                    else
                        Log.Error("server handler null");
                }
                    
            }
            catch(Exception e)
            {
                Log.Error(e);
            }
            
        }

        private IEnumerator<float> CheckpointMalfunction(){
            Log.Debug("CheckpointMalfunction");
            var checkpoints = Door.List.Where(d => d.IsCheckpoint).ToList().RandomItem().IsOpen =true;
            yield return Timing.WaitForSeconds(UnityEngine.Random.Range(20, 60));

        }

        private IEnumerator<float> GateLockdown(){
            Log.Debug("GateLockdown");
            var gates = Door.List.Where(d => d.Type == DoorType.GateA ||d.Type == DoorType.GateB);
            var gate = gates.GetRandomValue();
            gate.IsOpen = UnityEngine.Random.value <= .5f ? false : true ;
            var timelock = UnityEngine.Random.Range(10,30);
            gate.Lock(timelock,DoorLockType.Isolation);
            yield return Timing.WaitForSeconds(timelock);
        }

        private IEnumerator<float> ElevatorLockdown(){
            Log.Debug("ElevatorLockdown");
            var lift = Lift.Random;
            lift.ChangeLock(DoorLockReason.Isolation);
            yield return Timing.WaitForSeconds(UnityEngine.Random.Range(10,20));
            lift.ChangeLock(DoorLockReason.None);
        }
        
    }
}