using Exiled.API.Features.Toys;
using MapGeneration;
using System.Collections.Generic;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.Map.Others.CustomZones.CustomRooms.MCZ
{
    public class TCorridor : CustomRoom
    {
        public override RoomShape Shape => RoomShape.TShape;

        public override CustomFacilityZone FacilityZone => CustomFacilityZone.MediumContainment;

        protected override IEnumerable<AdminToy> SpawnRoom(Vector3 position, Vector3 rotation)
        {
            Quaternion rot = Quaternion.Euler(rotation);


            Light light = Light.Create(position + Vector3.up * 10, null, null, false, Color.white);
            light.LightType = LightType.Spot;
            light.Rotation = Quaternion.LookRotation(Vector3.down);
            light.Range = 50f;
            light.SpotAngle = 120;
            light.Intensity = 8;



            Quaternion otherBranch = rot * Quaternion.Euler(0, -90f, 0);

            float lengthbranch = 15f / 2f - 6f / 2f;


            return new HashSet<AdminToy>()
            {
                Primitive.Create(PrimitiveType.Cube,position, rot.eulerAngles, new Vector3(15f,1,6f), false),
                Primitive.Create(PrimitiveType.Cube,position + rot*Vector3.forward*lengthbranch, otherBranch.eulerAngles , new Vector3(lengthbranch,1,6f), false,Color.blue),
                light,
            };
        }
    }
}
