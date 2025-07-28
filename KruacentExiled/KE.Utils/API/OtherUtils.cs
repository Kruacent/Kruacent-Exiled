using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Utils.API
{
    public static class OtherUtils
    {

        public static bool IsInCircle(Vector3 pos,Vector3 centerCircle,float radius)
        {
            return (pos-centerCircle).sqrMagnitude <= Math.Pow(radius, 2);
        }
    }
}
