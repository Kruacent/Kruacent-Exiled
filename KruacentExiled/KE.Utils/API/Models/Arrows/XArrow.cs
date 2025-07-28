using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Utils.API.Models.Arrows
{
    internal class XArrow : Arrow
    {
        internal override Vector3 Offset => new Vector3(scale.x / 2,0);
        internal override Vector3 Rotation => new Vector3(0, 0f, 0f);
        internal override Color Color => Color.red;
    }
}
