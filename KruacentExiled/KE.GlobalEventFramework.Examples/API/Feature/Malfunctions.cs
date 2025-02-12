using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp049;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.NonAllocLINQ;

namespace KE.GlobalEventFramework.Examples.API.Feature
{
    public class Malfunctions
    {

        private sbyte _malfunction = 15;
        public const sbyte Lower = -50;
        public const sbyte Higher = 125;
        public sbyte Malfunction
        {
            get { return _malfunction; }
            set
            {
                if (value > Higher) _malfunction = Higher;
                else if (value < Lower) _malfunction = Lower;
                else _malfunction = value;
            }
        }

        public MalfunctionLevel MalfunctionLevels
        {
            get{return (MalfunctionLevel)Malfunction;}
        }

        private HashSet<MalfunctionEffect> MalfunctionEffects = new HashSet<MalfunctionEffect>();

        public sbyte MalfunctionAdd { get; set; } = 1;

        private bool[] CassieVoiceLine = new[] { true, true, true, true, true };
        private string[] msg = new[]
        {
            "Malfunctions back to more stable levels",
            "Malfunctions levels above . 25 percent . . decontamination of Light Containment Zone in . 30 seconds",
            "Malfunctions levels above . 50 percent . . terminating all door locks",
            "",
            "Malfunctions levels above . 90 percent . . starting emergency warhead",
        };

        private string[] msgtrans = new[]
        {
            "Malfunctions back to more stable levels",
            "Malfunctions levels above 25%, decontamination of Light Containment Zone in 30 seconds",
            "Malfunctions levels above 50%, terminating all door locks",
            "",
            "Malfunctions levels above 90%, starting emergency warhead",

        };
        public bool CassieYapYap { get; set; } = false;
        private Dictionary<Door, KeycardPermissions> doorkeys = Door.List.ToDictionary(d => d, d => d.KeycardPermissions);
        private CoroutineHandle _checkNuke;
        internal Malfunctions() { }

        internal IEnumerator<float> Tick()
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
            if (malfunction <= 0)
            {
                CassieSay(0);
                return;
            }

            //force decontamination
            if (malfunction >= 25)
            {
                if (!Map.IsLczDecontaminated && Map.IsDecontaminationEnabled)
                {
                    string msg = CassieSay(1);
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
                    Timing.CallDelayed(30 + Cassie.CalculateDuration(msg), () =>
                    {
                        Map.StartDecontamination();

                        Door.List.ToList().ForEach(d =>
                        {
                            if (d.IsElevator && (d.Type == DoorType.ElevatorLczA || d.Type == DoorType.ElevatorLczB))
                            {
                                d.Lock(DoorLockType.DecontEvacuate);
                                d.IsOpen = false;
                            }
                        });
                    });
                    return;
                }

            }


            //remove the lock on every doors
            if (malfunction >= 50)
            {
                string msg = CassieSay(2);
                Door.List.ToList().ForEach(d =>
                {
                    if (d.IsKeycardDoor)
                        d.KeycardPermissions = KeycardPermissions.None;

                });
                return;
            }

            //reput the locks
            if (malfunction < 40 && (CassieVoiceLine[3] || CassieYapYap))
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

        private string CassieSay(int id)
        {
            if (!string.IsNullOrEmpty(msg[id]))
                return "";
            if (CassieVoiceLine[id] || CassieYapYap)
            {
                CassieVoiceLine[id] = false;

                Cassie.MessageTranslated(msg[id], msgtrans[id], false, false);
            }
            return msg[id];
        }

        private IEnumerator<float> CheckNuke()
        {
            while (Warhead.IsInProgress)
            {
                yield return Timing.WaitForSeconds(5);
                Log.Debug("check nuke : " + Malfunction);
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
            sbyte result = (sbyte)UnityEngine.Random.Range(-2, 3);
            //generator reduce the malfunction
            result -= (sbyte)(Generator.List.Count(x => x.IsEngaged) * 3);
            //number of scp increase 3 (except zombies)
            result += (sbyte)(Player.List.Count(p => p.Role.Side == Side.Scp && p.Role != RoleTypeId.Scp0492) * 3);
            //number of zombies increase 1
            result += (sbyte)Player.List.Count(p => p.Role == RoleTypeId.Scp0492);
            return result;

        }


        internal void OnDying(DyingEventArgs ev)
        {
            switch (ev.Player.Role.Side)
            {
                case Side.Mtf:
                    Malfunction += 1;
                    break;
                case Side.ChaosInsurgency:
                    Malfunction -= 1;
                    break;
                case Side.Scp:
                    if (ev.Player.Role != RoleTypeId.Scp0492) Malfunction -= 10;
                    else Malfunction -= 1;
                    break;
            }
        }

        internal void OnFinishingRevive(FinishingRecallEventArgs ev)
        {
            Malfunction += 3;
        }



        public enum MalfunctionLevel
        {
            VeryLowMalfunction = 0,
            LowMalfunction = 25,
            MediumMalfunction = 50,
            HighMalfunction = 75,
            VeryHighMalfunction = 100,
        }
    }
}
