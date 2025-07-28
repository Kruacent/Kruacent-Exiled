using Exiled.API.Features.Toys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Map.Doors.KEDoorTypes
{
    public class NormalKEDoor : KEDoorType
    {
        public override IEnumerable<Primitive> Spawn(Vector3 position, Quaternion rotation)
        {



            return new HashSet<Primitive>()
            {
                Primitive.Create(PrimitiveType.Cube,position,rotation.eulerAngles),
            };
            
        }
    }
}
