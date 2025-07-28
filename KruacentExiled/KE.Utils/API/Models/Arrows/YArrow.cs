using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Utils.API.Models.Arrows
{
    internal class YArrow : Arrow
    {
        internal override Vector3 Offset => new Vector3(0, scale.x / 2);
        internal override Vector3 Rotation => new Vector3(0, 0f, 90f);
        internal override Color Color => Color.green;
    }
}
