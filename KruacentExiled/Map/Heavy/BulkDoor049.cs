using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Interactables.Interobjects.DoorUtils;
using KE.Utils.API.Map;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KruacentExiled.Map.Heavy
{
    public static class BulkDoor049
    {
        private static DoorVariant door;
        public static void Create()
        {


            Room room = Room.List.Where(r => r.Type == RoomType.Hcz049).First();


            Vector3 pos = new Vector3(-19.7f ,89f, 9f);
            Quaternion rot = Quaternion.identity;

            Vector3 worldpos = room.Position + room.Rotation*pos;



            door = StructureSpawner.SpawnDoor(ProjectMER.Features.Enums.DoorType.HeavyBulkDoor, worldpos, room.Rotation * rot , Vector3.one,ZoneType.HeavyContainment);


            door.OnStateChanged += Door_OnStateChanged;

            DoorVariant.OnInstanceRemoved += DoorVariant_OnInstanceRemoved;


            
            Log.Debug("spawn 049 bulk door at "+ worldpos);
        }
        private static CoroutineHandle handle;
        private static void DoorVariant_OnInstanceRemoved(DoorVariant obj)
        {
            door.OnStateChanged -= Door_OnStateChanged;
            DoorVariant.OnInstanceRemoved -= DoorVariant_OnInstanceRemoved;
            Timing.KillCoroutines(handle);
        }

        public const float IdleDuration = 20f;
        private static float duration = IdleDuration;
        public const float RefreshRate = 1f;
        private static void Door_OnStateChanged()
        {
            if (door.NetworkTargetState)
            {
                handle = Timing.RunCoroutine(AutoClose());
            }
            
        }

        private static IEnumerator<float> AutoClose()
        {
            duration = IdleDuration;

            while (duration > 0)
            {
                if (!door.NetworkTargetState)
                {
                    yield break;
                }
                if(door.NetworkActiveLocks == 0)
                {
                    duration -= RefreshRate;
                    Log.Debug(duration);
                    if (duration <= 0)
                    {
                        Log.Debug("auto close");
                        door.NetworkTargetState = false;
                        duration = IdleDuration;
                    }
                }
               
                yield return Timing.WaitForSeconds(RefreshRate);
              
            }
        }

    }
}
