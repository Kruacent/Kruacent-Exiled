using Exiled.API.Features.Toys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.Items.Items.Models
{
    internal class Scp514Model : Model
    {
        internal override void Create(Vector3 spawnPos, Quaternion rotation)
        {
            Toys.Add(Light.Create(spawnPos, null, Vector3.one / 3, true));
            var p = Primitive.Create(PrimitiveType.Cylinder, spawnPos, null, new Vector3(Scp514.Radius, 5f, Scp514.Radius), true);
            p.Color = new(0, 1, 0, .5f);
            p.Collidable = false;
            Toys.Add(p);
        }
    }
}
