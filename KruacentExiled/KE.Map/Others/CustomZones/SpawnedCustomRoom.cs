using Exiled.API.Features.Toys;
using MapGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Map.Others.CustomZones
{
    public class SpawnedCustomRoom
    {
        public SpawnedCustomRoom(CustomRoom baseRoom, RoomShape shape, Vector3 position, Vector3 rotation, Vector2Int coord, HashSet<AdminToy> primitives)
        {
            BaseRoom = baseRoom;
            Shape = shape;
            Position = position;
            Coord = coord;
            Primitives = [.. primitives];
            GameObject = new GameObject();
        }


        public GameObject GameObject { get; }
        public CustomRoom BaseRoom { get; }
        public RoomShape Shape { get; }

        public Vector3 Position { get; }
        public Vector3 Rotation { get; }

        public Vector2Int Coord { get; }

        public HashSet<AdminToy> Primitives { get; }

        public void Spawn()
        {
            foreach(AdminToy adminToy in Primitives)
            {
                adminToy.Spawn();
            }
        }

        public void Unspawn()
        {
            foreach (AdminToy adminToy in Primitives)
            {
                adminToy.UnSpawn();
            }
        }

        public void Destroy()
        {
            foreach (AdminToy adminToy in Primitives.ToList())
            {
                adminToy.Destroy();
            }
        }

    }
}
