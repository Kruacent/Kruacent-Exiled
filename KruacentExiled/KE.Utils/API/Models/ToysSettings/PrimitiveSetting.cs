using AdminToys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Utils.API.Models.ToysSettings
{
    public class PrimitiveSetting : ToySetting
    {

        public PrimitiveType PrimitiveType { get; }


        public PrimitiveFlags Flags { get; }


        public Color Color { get; }

        public Vector3 Position { get; }


        public Vector3 Rotation { get; }


        public Vector3 Scale { get; }

    }
}
