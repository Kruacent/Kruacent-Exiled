using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.Extensions
{
    public static class StringExtensions
    {

        public static Quaternion ToQuaternion(this string s)
        {
            s = s.Trim('(', ')').Replace(" ", "");
            string[] array = s.Split(',');
            float x = float.Parse(array[0], CultureInfo.InvariantCulture);
            float y = float.Parse(array[1], CultureInfo.InvariantCulture);
            float z = float.Parse(array[2], CultureInfo.InvariantCulture);
            float w = float.Parse(array[3], CultureInfo.InvariantCulture);
            return new Quaternion(x, y, z, w);
        }

    }
}
