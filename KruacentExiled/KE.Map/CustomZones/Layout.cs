using MapGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static KE.Map.CustomZones.AltasReader;

namespace KE.Map.CustomZones
{
    public struct Layout
    {
        public static HashSet<Layout> Layouts = new();
        public readonly Dictionary<Vector2Int, RoomShapeRotation> coordtoroom = new();
        public Layout(Dictionary<Vector2Int, RoomShapeRotation> coordtoroom)
        {
            this.coordtoroom = new Dictionary<Vector2Int, RoomShapeRotation>(coordtoroom);
            Layouts.Add(this);
        }


    }

}
