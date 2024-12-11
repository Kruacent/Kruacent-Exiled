using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackoutKruacent.API.Features
{
    public class Controller
    {

        
        /// <summary>
        /// Select a random zone and close and lock all door of the zone
        /// </summary>
        internal IEnumerator<float> RandomDoorStuck()
        {
            yield return Timing.WaitForOneFrame;
            var zone = SelectZone();
            Log.Debug($"DoorStuck in {zone}");
            yield return Timing.WaitUntilFalse(() => Cassie.IsSpeaking);
            CassieVoiceLine(zone, false);
            yield return Timing.WaitUntilFalse(() => Cassie.IsSpeaking);
            yield return Timing.WaitForSeconds(5);
            // if the zone is light and there is only 30s left then skip
            if (!(zone == ZoneType.LightContainment && Map.DecontaminationState == DecontaminationState.Countdown))
            {
                List<Door> doorList = Door.List
                    .Where(d => !new[] { DoorType.ElevatorGateA, DoorType.ElevatorGateB, DoorType.ElevatorLczA, DoorType.ElevatorLczB, DoorType.ElevatorNuke, DoorType.ElevatorScp049, DoorType.UnknownElevator }.Contains(d.Type))
                    .ToList();
                foreach (Door door in doorList)
                {
                    if (door.Zone == zone)
                    {
                        door.IsOpen = false;
                        door.ChangeLock(DoorLockType.Lockdown2176);
                        Timing.CallDelayed(MainPlugin.Instance.Config.DurationMalfunction / 2, () => door.IsOpen = true);
                        Timing.CallDelayed(MainPlugin.Instance.Config.DurationMalfunction, () => door.Unlock());
                        if (UnityEngine.Random.value < .5f)
                        {
                            door.IsOpen = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Select a random zone and blackout it
        /// </summary>
        public IEnumerator<float> RandomBlackout()
        {
            yield return Timing.WaitForOneFrame;

            var zone = SelectZone();
            yield return Timing.WaitUntilFalse(() => Cassie.IsSpeaking);
            CassieVoiceLine(zone, true);
            yield return Timing.WaitUntilFalse(() => Cassie.IsSpeaking);
            yield return Timing.WaitForSeconds(5);
            Log.Debug($"BlackOut in {zone}");
            

            Map.TurnOffAllLights(MainPlugin.Instance.Config.DurationMalfunction, zone);
        }


        private void CassieVoiceLine(ZoneType zone,bool isBlackout)
        {
            if (isBlackout)
            {
                switch (zone)
                {
                    case ZoneType.LightContainment:
                        Cassie.MessageTranslated("Warning Failure Of All Lights In Light Containment Zone in 5 seconds",
                            "<color=#F00>Warning</color> Failure Of All Lights In <color=#1BBB9B>Light</color> Containment Zone in 5 seconds", false, false);
                        break;
                    case ZoneType.HeavyContainment:
                        Cassie.MessageTranslated("Warning Failure Of All Lights In Heavy Containment Zone in 5 seconds",
                            "<color=#F00>Warning</color> Failure Of All Lights In <color=#431919>Heavy</color> Containment Zone in 5 seconds", false, false);
                        break;
                    case ZoneType.Entrance:
                        Cassie.MessageTranslated("Warning Failure Of All Lights In Entrance Zone in 5 seconds",
                            "<color=#F00>Warning</color> Failure Of All Lights In <color=#FFFF00>Entrance</color> Zone in 5 seconds", false, false);
                        break ;
                    case ZoneType.Surface:
                        Cassie.MessageTranslated("Warning Failure Of All Lights In Surface in 5 seconds",
                            "<color=#FF0000>Warning</color> Failure Of All Lights In <color=#FF0000>Surface</color> in 5 seconds", false, false);
                        break ;
                    case ZoneType.Unspecified:
                        Cassie.MessageTranslated("Warning Failure Of All Lights In All Of The Facility in 5 seconds", 
                            "<color=#FF0000>Warning</color> Failure Of All Lights In <color=#FF0000>All</color> Of The Facility in 5 seconds", false, false);
                        break;
                }
            }
            else
            {
                switch (zone)
                {
                    case ZoneType.LightContainment:
                        Cassie.MessageTranslated("Door system malfunction in Light Containment Zone in 5 seconds",
                            "Door system malfunction in <color=#1BBB9B>Light</color> containment zone in 5 seconds", false,false);
                        break;
                    case ZoneType.HeavyContainment:
                        Cassie.MessageTranslated("Door system malfunction in Heavy Containment Zone in 5 seconds",
                            "Door system malfunction in <color=#431919>Heavy</color> zone in 5 seconds", false, false);
                        break;
                    case ZoneType.Entrance:
                        Cassie.MessageTranslated("Door system malfunction in Entrance zone in 5 seconds",
                            "Door system malfunction in <color=#ffff00>Entrance</color> zone in 5 seconds", false, false);
                        break;
                    case ZoneType.Surface:
                        Cassie.MessageTranslated("Door system malfunction in Surface Zone in 5 seconds",
                            "Door system malfunction in <color=#F00>Surface</color> zone in 5 seconds", false, false);
                        break;
                    case ZoneType.Unspecified:
                        Cassie.MessageTranslated("Door system malfunction in All of the facility in 5 seconds",
                            "Door system malfunction in <color=#ff0000>All</color> of the facility in 5 seconds", false, false);
                        break;
                }
            }
        }

        /// <summary>
        /// Select a random zone
        /// If the nuke is detonated return only SurfaceZone
        /// </summary>
        /// <returns>a zone</returns>
        private ZoneType SelectZone() 
        {
            ZoneType z = ZoneType.Unspecified;
            float random = UnityEngine.Random.value;
            if (Warhead.IsDetonated)
            {
                z=  ZoneType.Surface;
            }
            if (!Map.IsLczDecontaminated)
            {
                z=  GetZoneType(GetProbabilityIndex(MainPlugin.Instance.Config.ChancePreConta, random));
            }
            else
            {
                z= GetZoneType(GetProbabilityIndex(MainPlugin.Instance.Config.ChancePostConta, random));
            }
            Log.Debug($"zone={z}");
            return z;
        }

        private int GetProbabilityIndex(float[] probabilities, float randomValue)
        {
            float cumulative = 0f;

            for (int i = 0; i < probabilities.Length; i++)
            {
                cumulative += probabilities[i];
                if (randomValue <= cumulative)
                {
                    return i;
                }
            }

            throw new ArgumentException("Random value is outside the range of probabilities.");
        }

        private ZoneType GetZoneType(int index)
        {
            switch (index)
            {
                case 0:
                    return ZoneType.LightContainment;
                case 1:
                    return ZoneType.HeavyContainment;
                case 2:
                    return ZoneType.Entrance;
                case 3:
                    return ZoneType.Surface;
                default:
                case 4:
                    return ZoneType.Unspecified;
            }
        }
    }
}
