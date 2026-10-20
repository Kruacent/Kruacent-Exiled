using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.Extensions
{
    public static class Vector3Extensions
    {

        public static Vector3 Round(this Vector3 vector3)
        {
            return new Vector3(Mathf.Round(vector3.x), Mathf.Round(vector3.y), Mathf.Round(vector3.z));
        }



    }
}
