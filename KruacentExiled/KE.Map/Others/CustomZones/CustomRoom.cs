using Exiled.API.Features;
using Exiled.API.Features.Toys;
using MapGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Map.Others.CustomZones
{
    public abstract class CustomRoom
    {
        private static readonly HashSet<CustomRoom> registered = new HashSet<CustomRoom>();
        public static IReadOnlyCollection<CustomRoom> RegisteredRoom => registered;

        public abstract RoomShape Shape { get; }

        public abstract CustomFacilityZone FacilityZone { get; }
        
        public Vector3 Size => RoomIdentifier.GridScale;
        public HashSet<SpawnedCustomRoom> SpawnedRoom { get; } = new HashSet<SpawnedCustomRoom>();

        public CustomRoom()
        {
            registered.Add(this);
        }

        protected abstract IEnumerable<AdminToy> SpawnRoom(Vector3 position,Vector3 rotation);


        public SpawnedCustomRoom Spawn(Vector2Int coord,Vector3 rotation,Vector3 spawnzone)
        {
            Vector3 position = new Vector3(-coord.x * Size.x+ spawnzone.x, spawnzone.y, coord.y * Size.z + spawnzone.z);

            Log.Debug("spawn room at " + position);
            IEnumerable<AdminToy> prims = SpawnRoom(position,rotation);

            foreach(AdminToy admin in prims)
            {
                Log.Debug("spawning prim at "+ admin.Position);
            }


            SpawnedCustomRoom room = new SpawnedCustomRoom(this, Shape, position,rotation, coord, prims.ToHashSet());
            room.Spawn();
            SpawnedRoom.Add(room);
            return room;

        }


        public static Primitive CreatePrimitive(PrimitiveType type = PrimitiveType.Cube,Vector3? position = null, Vector3? rotation = null, Vector3? scale = null,Color? color = null)
        {
            return Primitive.Create(type, position, rotation, scale, false, color);
        }



    }
}
