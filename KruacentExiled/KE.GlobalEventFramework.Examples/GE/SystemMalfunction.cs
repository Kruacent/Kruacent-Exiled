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
        public override uint[] IncompatibleGE { get; set; } = { 38 };
        /// <summary>
        /// Set the cooldown for the BlackoutNDoor
        /// </summary>
        public int NewCooldown { get; set; } = 180;
        private BlackoutNDoor.MainPlugin BlackoutNDoor = null;
        private sbyte _malfunction = 15;
        public sbyte Malfunction 
        { 
            get { return _malfunction; } 
            private set 
            {
                if (value > 125) _malfunction = 125;
                else if (value < -50) _malfunction = 50;
                else _malfunction = value;
            } 
        } 
        public sbyte MalfunctionAdd { get; set; } = 1;

        private bool[] CassieVoiceLine = new[] {true, true , true , true , true };
        public bool CassieYapYap { get; private set; } = false;
        private Dictionary<Door,KeycardPermissions> doorkeys = Door.List.ToDictionary(d => d, d=> d.KeycardPermissions);
        private CoroutineHandle _checkNuke;


        /// <inheritdoc/>
        public IEnumerator<float> Start()
        {
            Log.Debug("system malfunction start");
            //MoreBlackOutNDoors();
            //Coroutine.LaunchCoroutine(EarlyNuke());
            Coroutine.LaunchCoroutine(Tick());
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

        private IEnumerator<float> Tick()
        {
            Log.Debug("in tick");
            while (Round.InProgress)
            {
                Log.Debug($"Malfunction={Malfunction}");
                CassieVoice(Malfunction);
                yield return Timing.WaitForSeconds(60);
                Malfunction += MalfunctionAdd;
                Malfunction += AdditionnalMalfunction();
            }
        }

        private void CassieVoice(sbyte malfunction)
        {
            if(malfunction <= 0 && (CassieVoiceLine[0] || CassieYapYap ))
            {
                Cassie.MessageTranslated("Malfunctions back to more stable levels",
                        "Malfunctions back to more stable levels", false, false);
                CassieVoiceLine[0] = false;
                return;
            }

            //force decontamination
            if(malfunction >= 25 && (CassieVoiceLine[1] || CassieYapYap) && !Map.IsLczDecontaminated && Map.IsDecontaminationEnabled)
            {
                string msg = "Malfunctions levels above . 25 percent . . decontamination of Light Containment Zone in . 30 seconds";
                Cassie.MessageTranslated(msg,
                    "Malfunctions levels above 25%, decontamination of Light Containment Zone in 30 seconds", false, false);
                CassieVoiceLine[1] = false;
                Door.List.ToList().ForEach(d =>
                {
                    if (d.Zone == ZoneType.LightContainment)
                    {
                        if (!d.IsElevator)
                        {
                            d.ChangeLock(DoorLockType.DecontEvacuate);
                            d.IsOpen = true;
                        }
                        
                    }
                });
                Timing.CallDelayed(30+Cassie.CalculateDuration(msg), () =>
                {
                    Map.StartDecontamination();

                    Door.List.ToList().ForEach(d =>
                    {
                        if(d.IsElevator && (d.Type == DoorType.ElevatorLczA || d.Type == DoorType.ElevatorLczB))
                        {
                            d.Lock(DoorLockType.DecontEvacuate);
                            d.IsOpen = false;
                        }
                    });
                });
                return;
            }

            //remove the lock on every doors
            if (malfunction >= 50 && (CassieVoiceLine[2] || CassieYapYap))
            {
                CassieVoiceLine[2] = false;
                Cassie.MessageTranslated("Malfunctions levels above . 50 percent . . terminating all door locks",
                    "Malfunctions levels above 50 percent, terminating all door locks", false, false);
                Door.List.ToList().ForEach(d =>
                {
                    if (d.IsKeycardDoor)
                        d.KeycardPermissions = KeycardPermissions.None;
                    
                });
                return;
            }

            //reput the locks
            if(malfunction < 40 && (CassieVoiceLine[3] || CassieYapYap))
            {
                CassieVoiceLine[3] = false;
                doorkeys.ForEach(dk => 
                {
                    dk.Key.KeycardPermissions = dk.Value;
                });
                
                return;
            }

            //nuke
            if (malfunction >= 90)
            {
                if (CassieVoiceLine[4] || CassieYapYap)
                {
                    CassieVoiceLine[4] = false;
                    string msg = "Malfunctions levels above . 90 percent . . starting emergency warhead";
                    Cassie.MessageTranslated(msg,
                       "Malfunctions levels above 90%, starting emergency warhead", false, false);
                }

                if (!Warhead.IsInProgress)
                {
                    Warhead.Start();
                    Warhead.IsLocked = true;
                    Timing.KillCoroutines(_checkNuke);
                    _checkNuke = Timing.RunCoroutine(CheckNuke());
                }
                return;
            }

        }

        private IEnumerator<float> CheckNuke()
        {
            while (Warhead.IsInProgress)
            {
                yield return Timing.WaitForSeconds(5);
                Log.Debug("check nuke : "+Malfunction);
                if (Malfunction <= 85)
                {
                    Log.Debug($"Malfunction low enough ({Malfunction}) disabling the nuke");
                    Warhead.IsLocked = false;
                    Warhead.Stop();
                    break;
                }
            }
        }

        private sbyte AdditionnalMalfunction()
        {
            sbyte result = (sbyte) UnityEngine.Random.Range(-2,3);
            //generator reduce the malfunction
            result -= (sbyte) (Generator.List.Count(x => x.IsEngaged)*3);
            //number of scp increase 3 (except zombies)
            result += (sbyte) (Player.List.Count(p => p.Role.Side == Side.Scp && p.Role != RoleTypeId.Scp0492) * 3);
            //number of zombies increase 1
            result += (sbyte)Player.List.Count(p => p.Role == RoleTypeId.Scp0492);
            return result;

        }


        private void OnDying(DyingEventArgs ev)
        {
            switch (ev.Player.Role.Side)
            {
                case Side.Mtf:
                    Malfunction += 1;
                    break;
                case Side.ChaosInsurgency :
                    Malfunction -= 1;
                    break;
                case Side.Scp:
                    if(ev.Player.Role != RoleTypeId.Scp0492) Malfunction -= 10;
                    else Malfunction -= 1;
                    break;
            }
        }

        private void OnFinishingRevive(FinishingRecallEventArgs ev)
        {
            Malfunction += 3;
        }


        public void SubscribeEvent()
        {

            Exiled.Events.Handlers.Player.Dying += OnDying;
            Exiled.Events.Handlers.Scp049.FinishingRecall += OnFinishingRevive;


            //Searching for the plugin
            var otherPlugin = Exiled.Loader.Loader.Plugins.FirstOrDefault(plugin => plugin.Name == "KE.BlackoutDoor");
            if (otherPlugin != null)
            {

                if (otherPlugin is BlackoutNDoor.MainPlugin blackout)
                {
                    Log.Info("Found BlackOutNDoors");
                    BlackoutNDoor = blackout;
                    return;
                }

            }
        }
        public void UnsubscribeEvent()
        {
            BlackoutNDoor = null;
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            Exiled.Events.Handlers.Scp049.FinishingRecall -= OnFinishingRevive;
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