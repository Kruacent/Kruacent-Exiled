using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Utils.API
{
    public static class Parser
    {
        public static Vector3 Vector3(string s)
        {
            string[] temp = s.Substring(1, s.Length - 3).Split(',');

            if(!float.TryParse(temp[0],out float x))
            {
                return UnityEngine.Vector3.zero;
            }

            if (!float.TryParse(temp[1], out float y))
            {
                return UnityEngine.Vector3.zero;
            }
            if (!float.TryParse(temp[1], out float z))
            {
                return UnityEngine.Vector3.zero;
            }



            return new Vector3(x, y, z);
        }
    }
}
