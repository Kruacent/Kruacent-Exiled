using Exiled.API.Features;
using Exiled.API.Features.Toys;
using KE.Map.Others.CustomZones.CustomRooms;
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
        private static readonly HashSet<SpawnedCustomRoom> spawned = new HashSet<SpawnedCustomRoom>();
        public static IReadOnlyCollection<SpawnedCustomRoom> SpawnedRoom => spawned;
        public SpawnedCustomRoom(CustomRoom baseRoom, RoomShape shape, Vector3 position, Vector3 rotation, Vector2Int coord, HashSet<AdminToy> primitives)
        {
            BaseRoom = baseRoom;
            Shape = shape;
            Position = position;
            Coord = coord;
            Primitives = primitives.ToHashSet();
            spawned.Add(this);
        }


        public CustomRoom BaseRoom { get; }
        public RoomShape Shape { get; }

        public Vector3 Position { get; }
        public Vector3 Rotation { get; }

        public Vector2Int Coord { get; }

        private HashSet<SpawnedCustomRoom> cachedNeighbors = null;
        public IEnumerable<SpawnedCustomRoom> Neighbors
        {
            get
            {
                if (cachedNeighbors is null)
                {
                    cachedNeighbors = new HashSet<SpawnedCustomRoom>();
                    foreach (SpawnedCustomRoom scr in SpawnedRoom)
                    {

                        int dx = scr.Coord.x - Coord.x;
                        int dy = scr.Coord.y - Coord.y;

                        if ((Math.Abs(dx) == 1 && dy == 0) || (Math.Abs(dy) == 1 && dx == 0))
                        {
                            if (!cachedNeighbors.Contains(scr))
                                cachedNeighbors.Add(scr);
                        }
                    }
                }

                return cachedNeighbors;
            }
        }

        public HashSet<SpawnedDoorSeparator> Door { get; } = new HashSet<SpawnedDoorSeparator>();

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



        public bool DoorAvailable(Vector2Int roomToLink)
        {
            if(Shape == RoomShape.Curve)
            {
                var curveLinks = new Dictionary<int, (int dx, int dy)[]>
                {
                    {   0, new [] { ( 0, 1), ( 1,  0) } }, // Up, Right
                    {  90, new [] { ( -1,  0), ( 0,  1) } }, // Right, Down
                    { 180, new [] { ( 0,  -1), (-1,  0) } }, // Down, Left
                    { 270, new [] { (1, 0), ( 0, -1) } }, // Left, Up
                };

                int rot = Mathf.RoundToInt(Rotation.y) % 360;

                if (curveLinks.TryGetValue(rot, out var links))
                {
                    foreach (var (dx, dy) in links)
                    {
                        if (roomToLink.x == Coord.x + dx &&
                            roomToLink.y == Coord.y + dy)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }


            return true;
        }

    }
}
