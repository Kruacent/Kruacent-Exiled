using DrawableLine;
using Exiled.API.Features.Toys;
using KruacentExiled.Map.Others.CustomZones;
using MapGeneration;
using MEC;
using System.Collections.Generic;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KruacentExiled.Map.Others.CustomZones.CustomRooms.MCZ
{
    public class TCorridor : CustomRoom
    {
        public override RoomShape Shape => RoomShape.TShape;

        public override CustomFacilityZone FacilityZone => CustomFacilityZone.MediumContainment;

        private float width = 6f;
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



            float lengthbranch = Size.x / 2f - width / 2f;

            return new HashSet<AdminToy>()
            {
                
                CreatePrimitive(PrimitiveType.Cube,position,rotation, new Vector3(Size.x,1,width)),
                CreatePrimitive(PrimitiveType.Cube,position + rot*Vector3.forward*(width/2+lengthbranch/2),rotation, new Vector3(width,1,lengthbranch)),
                light,

                CreatePrimitive(PrimitiveType.Cube,position +Vector3.up * height,rotation, new Vector3(Size.x,1,width)),
                CreatePrimitive(PrimitiveType.Cube,position +Vector3.up * height+ rot*Vector3.forward*(width/2+lengthbranch/2),rotation, new Vector3(width,1,lengthbranch)),
            };
            
        }
    }
}
