using Exiled.API.Features.Toys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Items.Models
{
    internal abstract class Model
    {
        internal Vector3 Position { get; set; }
        protected List<AdminToy> Toys { get; set; }
        internal abstract void Spawn(Vector3 spawnPos);

        internal void Unspawn()
        {
            foreach (AdminToy primitive in Toys)
            {
                primitive.Destroy();
            }
        }
    }
}
