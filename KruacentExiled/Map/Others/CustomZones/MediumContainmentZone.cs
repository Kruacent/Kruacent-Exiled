using Exiled.API.Extensions;
using Exiled.API.Features;
using Interactables.Interobjects.DoorButtons;
using KruacentExiled.Map.Others.CustomZones.CustomRooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using Random = System.Random;

namespace KruacentExiled.Map.Others.CustomZones
{
    public class MediumContainmentZone : CustomZone
    {
        public override CustomFacilityZone FacilityZone => CustomFacilityZone.MediumContainment;

        public override Vector3 Spawnzone { get; }

        private GameObject gameObject;
        public MediumContainmentZone()
        {
            Spawnzone = AlphaWarheadNukesitePanel.Singleton.lever.position+ new Vector3(0, -50, 0); 
            //Spawnzone = Room.List.Where(r => r.Type == Exiled.API.Enums.RoomType.HczEzCheckpointB).First().Position + new Vector3(0,0,-200);
            gameObject = new GameObject("MCZ");

        }



        public override void Generate(Random rng,Layout layout)
        {


            if(CustomRoom.RegisteredRoom.Count == 0)
            {
                throw new ArgumentNullException("no round found");
            }

            IEnumerable<CustomRoom> rooms = CustomRoom.RegisteredRoom.Where(r => r.FacilityZone == FacilityZone);
            
            foreach(var kvp in layout.coordtoroom)
            {
                CustomRoom room = rooms.GetRandomValue(c => c.Shape == kvp.Value.RoomShape);
                
                if(room is null)
                {
                    throw new Exception($"couldn't find a room for {FacilityZone} with shape {kvp.Value.RoomShape}");
                }

                Vector3 rotation = kvp.Value.Rotation;

                Vector2Int coord = kvp.Key/3;


                room.Spawn(coord, rotation, Spawnzone);
            }

            IEnumerable<DoorSeparator> doors = DoorSeparator.RegisteredDoorSeparator.Where(d => d.FacilityZone == FacilityZone);


            foreach(SpawnedCustomRoom scr in SpawnedCustomRoom.SpawnedRoom)
            {
                //Log.Debug($"scr1 {scr.Shape} ({scr.Coord})");
                foreach(SpawnedCustomRoom scr2 in scr.Neighbors)
                {
                    //Log.Debug($"scr2 {scr2.Shape} ({scr2.Coord})");
                    
                    var scrDoors = scr.Door.Intersect(scr2.Door);


                    if (scrDoors.Count() > 0)
                    {
                        //Log.Debug("already a door");
                        continue;
                    }

                    if (scr.DoorAvailable(scr2.Coord))
                    {
                        Log.Debug("not available " + scr2.Coord);
                        continue;
                    }

                    DoorSeparator door = doors.GetRandomValue();
                    door.Spawn(scr, scr2);

                }
            }



        }



    }
}
