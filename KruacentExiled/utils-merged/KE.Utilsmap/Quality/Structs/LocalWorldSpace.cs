using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Utils.Quality.Structs
{
    public readonly struct LocalWorldSpace : IWorldSpace
    {

        public Vector3 Position { get; }
        public Quaternion Rotation { get; }

        public LocalWorldSpace(Vector3 position, Quaternion rotation)
        {
            Position = position; 
            Rotation = rotation;
        }
    }
}
