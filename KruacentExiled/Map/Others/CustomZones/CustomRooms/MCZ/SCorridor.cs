using Exiled.API.Features.Toys;
using MapGeneration;
using System.Collections.Generic;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KruacentExiled.Map.Others.CustomZones.CustomRooms.MCZ
{
    public class SCorridor : CustomRoom
    {
        public override RoomShape Shape => RoomShape.Straight;

        public override CustomFacilityZone FacilityZone => CustomFacilityZone.MediumContainment;

        private float height = 4;
        protected override IEnumerable<AdminToy> SpawnRoom(Vector3 position, Vector3 rotation)
        {
            Quaternion rot = Quaternion.Euler(rotation);

            
            Light light = Light.Create(position + Vector3.up * height, null, null, false, Color.white);
            light.LightType = LightType.Point;
            light.Rotation = Quaternion.LookRotation(Vector3.down);
            light.Range = 10;
            light.Intensity = 50;
            light.ShadowType = LightShadows.None;


            return new HashSet<AdminToy>()
            {
                Primitive.Create(PrimitiveType.Cube,position, rotation, new Vector3(6f,1,15f), false),
                Primitive.Create(PrimitiveType.Cube,position+Vector3.up*height, rotation, new Vector3(6f,.5f,15f), false),
                Primitive.Create(PrimitiveType.Cube,position+Vector3.up*(height/2)+rot*Vector3.left*2, rotation, new Vector3(1f,height,15f), false),
                Primitive.Create(PrimitiveType.Cube,position+Vector3.up*(height/2)+rot*Vector3.right*2, rotation, new Vector3(1f,height,15f), false),
                light,
            };
        }
    }
}
