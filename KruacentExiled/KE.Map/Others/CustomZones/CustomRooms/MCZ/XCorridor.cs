using Exiled.API.Features.Toys;
using MapGeneration;
using System.Collections.Generic;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.Map.Others.CustomZones.CustomRooms.MCZ
{
    public class XCorridor : CustomRoom
    {
        public override RoomShape Shape => RoomShape.XShape;

        public override CustomFacilityZone FacilityZone => CustomFacilityZone.MediumContainment;
        private float width = 6f;
        protected override IEnumerable<AdminToy> SpawnRoom(Vector3 position, Vector3 rotation)
        {
            Quaternion rot = Quaternion.Euler(rotation);


            Light light = Light.Create(position + Vector3.up * 2, null, null, false, Color.white);
            light.LightType = LightType.Spot;
            light.Rotation = Quaternion.LookRotation(Vector3.down);
            light.Range = 50f;
            light.SpotAngle = 90;
            light.Intensity = 8;

            float lengthbranch = Size.x / 2f - width / 2f;


            return new HashSet<AdminToy>()
            {
                CreatePrimitive(PrimitiveType.Cube,position,rot.eulerAngles, new Vector3(Size.x,1,width)),
                CreatePrimitive(PrimitiveType.Cube,position + rot*Vector3.forward*(width/2+lengthbranch/2),rot.eulerAngles, new Vector3(width,1,lengthbranch)),
                CreatePrimitive(PrimitiveType.Cube,position + rot*Vector3.back*(width/2+lengthbranch/2),rot.eulerAngles, new Vector3(width,1,lengthbranch)),
                light,
            };
        }
    }
}
