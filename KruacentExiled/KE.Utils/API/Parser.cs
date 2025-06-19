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
            return new Vector3(float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]));
        }
    }
}
