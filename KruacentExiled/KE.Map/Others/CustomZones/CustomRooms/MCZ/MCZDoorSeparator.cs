using Interactables.Interobjects.DoorUtils;
using KE.Map.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Map.Others.CustomZones.CustomRooms.MCZ
{
    public class MCZDoorSeparator : DoorSeparator
    {
        public override CustomFacilityZone FacilityZone => CustomFacilityZone.MediumContainment;
        private Vector3 doorpos;
        protected override IEnumerable<GameObject> Create(Vector3 position, Vector3 rotation)
        {
            Quaternion rot = Quaternion.Euler(rotation);
            DoorVariant door = StructureSpawner.CreateRawDoor(ProjectMER.Features.Enums.DoorType.Ez, position + Vector3.up / 2f, Quaternion.Euler(rotation), Vector3.one);

            doorpos = door.transform.position+Vector3.up*1.5f;
            Vector3 scale = new Vector3(2.5f, 3f, .2f);

            return new HashSet<GameObject>()
            {
                door.gameObject,
                CustomRoom.CreatePrimitive(PrimitiveType.Cube,doorpos+rot*Vector3.right*2,rotation,scale).GameObject,
                CustomRoom.CreatePrimitive(PrimitiveType.Cube,doorpos+rot*Vector3.left*2,rotation,scale).GameObject,
            };

            
        }
    }
}
