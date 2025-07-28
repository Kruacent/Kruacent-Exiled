using Exiled.API.Features.Toys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Map.Doors.KEDoorTypes
{
    public abstract class KEDoorType
    {


        public abstract IEnumerable<Primitive> Spawn(Vector3 position, Quaternion rotation);

    }
}
