using Exiled.API.Features.Toys;
using MapGeneration;
using System.Collections.Generic;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.Map.CustomZones.CustomRooms.MCZ
{
    public class EndRoom : CustomRoom
    {
        public override RoomShape Shape => RoomShape.Endroom;

        public override CustomFacilityZone FacilityZone => CustomFacilityZone.MediumContainment; 

        protected override IEnumerable<AdminToy> SpawnRoom(Vector3 position,Vector3 rotation)
        {
            Quaternion rot = Quaternion.Euler(rotation);


            Light light = Light.Create(position + Vector3.up * 2, null, null, false, Color.white);
            light.LightType = LightType.Spot;
            light.Rotation = Quaternion.LookRotation(Vector3.down);
            light.Range = 50f;
            light.SpotAngle = 120;
            light.Intensity = 8;




            return new HashSet<AdminToy>()
            {
                Primitive.Create(PrimitiveType.Cube,position, rotation, new Vector3(15f,1,6f), false),
                light,
            };
        }
    }
}
