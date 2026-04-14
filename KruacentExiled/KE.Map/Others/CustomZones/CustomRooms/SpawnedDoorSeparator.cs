using Exiled.API.Features.Toys;
using MapGeneration;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Map.Others.CustomZones.CustomRooms
{
    public class SpawnedDoorSeparator
    {
        public SpawnedDoorSeparator(DoorSeparator basedoor, Vector3 position, Vector3 rotation, IEnumerable<SpawnedCustomRoom> rooms, HashSet<GameObject> gameObjects)
        {
            BaseDoor = basedoor;
            Position = position;
            Rooms = rooms;
            GameObjects = [.. gameObjects];
        }


        public DoorSeparator BaseDoor { get; }
        public Vector3 Position { get; }
        public Vector3 Rotation { get; }

        public IEnumerable<SpawnedCustomRoom> Rooms { get; }

        public HashSet<GameObject> GameObjects { get; }


        public void Spawn()
        {
            foreach(GameObject obj in GameObjects)
            {
                NetworkServer.Spawn(obj);
            }
        }

        public void Unspawn()
        {
            foreach (GameObject obj in GameObjects)
            {
                NetworkServer.UnSpawn(obj);
            }
        }

        public void Destroy()
        {
            foreach (GameObject obj in GameObjects.ToList())
            {
                NetworkServer.Destroy(obj);
            }
        }

    }
}
