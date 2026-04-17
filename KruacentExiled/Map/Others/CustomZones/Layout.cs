using MapGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static KruacentExiled.Map.Others.CustomZones.AltasReader;

namespace KruacentExiled.Map.Others.CustomZones
{
    public struct Layout
    {
        public static HashSet<Layout> Layouts = new HashSet<Layout>();
        public string Name { get; }
        public readonly Dictionary<Vector2Int, RoomShapeRotation> coordtoroom;
        public Layout(Dictionary<Vector2Int, RoomShapeRotation> coordtoroom, string name)
        {
            Name = name;
            this.coordtoroom = new Dictionary<Vector2Int, RoomShapeRotation>(coordtoroom);
            Layouts.Add(this);
        }


    }

}
