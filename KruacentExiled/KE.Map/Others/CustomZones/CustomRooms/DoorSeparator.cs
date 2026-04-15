using Exiled.API.Features;
using KE.Map.Others.CustomZones;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Map.Others.CustomZones.CustomRooms
{
    public abstract class DoorSeparator
    {
        private static readonly HashSet<DoorSeparator> registered = new HashSet<DoorSeparator>();
        public static IReadOnlyCollection<DoorSeparator> RegisteredDoorSeparator => registered;
        public DoorSeparator()
        {
            registered.Add(this);
        }

        

        public HashSet<SpawnedDoorSeparator> SpawnDoorSeparator { get; } = new HashSet<SpawnedDoorSeparator>();
        public abstract CustomFacilityZone FacilityZone { get; }

        protected abstract IEnumerable<GameObject> Create(Vector3 position, Vector3 rotation);


        public SpawnedDoorSeparator Spawn(SpawnedCustomRoom room1, SpawnedCustomRoom room2)
        {
            Vector3 posdif = room1.Position + room2.Position;
            Vector3 position = posdif / 2f;
            Vector3 rotation = Vector3.zero;
            if (Mathf.Approximately(room1.Position.z, room2.Position.z))
            {
                rotation = new Vector3(0, 90, 0);
            }


            //Log.Debug("spawning door at " + position + $"({rotation})");
            IEnumerable<GameObject> objs = Create(position, rotation);



            SpawnedDoorSeparator spawned = new SpawnedDoorSeparator(this, position, rotation, new List<SpawnedCustomRoom>() { room1, room2 }, objs.ToHashSet());
            spawned.Spawn();
            
            SpawnDoorSeparator.Add(spawned);
            room1.Door.Add(spawned);
            room2.Door.Add(spawned);
            return spawned;
        }

    }
}
