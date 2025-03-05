using Exiled.API.Features.Toys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Items.Items.Models
{
    internal abstract class Model
    {
        internal Vector3 Position { get; set; }
        protected List<AdminToy> Toys { get; set; } = new List<AdminToy> { };
        internal abstract void Spawn(Vector3 spawnPos, Quaternion rotation);

        internal void UnSpawn()
        {
            foreach (AdminToy primitive in Toys)
            {
                primitive.Destroy();
            }
        }
    }
}
