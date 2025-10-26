using Exiled.API.Extensions;
using Exiled.API.Features;
using Interactables.Interobjects.DoorButtons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

namespace KE.Map.CustomZones
{
    public class MediumContainmentZone : CustomZone
    {
        public override CustomFacilityZone FacilityZone => CustomFacilityZone.MediumContainment;

        public override Vector3 Spawnzone { get; }

        private GameObject gameObject;
        public MediumContainmentZone()
        {
            Spawnzone = Room.List.Where(r => r.Type == Exiled.API.Enums.RoomType.HczEzCheckpointB).First().Position + new Vector3(0,0,-200);
            gameObject = new GameObject("MCZ");

        }


        public override void Generate(Random rng)
        {
            SetRandomLayout();

            if(CustomRoom.RegisteredRoom.Count == 0)
            {
                throw new ArgumentNullException("no round found");
            }

            IEnumerable<CustomRoom> rooms = CustomRoom.RegisteredRoom.Where(r => r.FacilityZone == FacilityZone);
            
            foreach(var kvp in Layout.coordtoroom)
            {
                CustomRoom room = rooms.GetRandomValue(c => c.Shape == kvp.Value.RoomShape);
                
                if(room is null)
                {
                    throw new Exception($"couldn't find a room for {FacilityZone} with shape {kvp.Value}");
                }

                Vector3 rotation = kvp.Value.Rotation;

                Vector2Int coord = kvp.Key/3;


                room.Spawn(coord, rotation, Spawnzone);
            }
            




        }



    }
}
