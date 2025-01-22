using Exiled.API.Enums;
using Exiled.API.Features;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace KE.Misc
{
    /// <summary>
    /// Everything about Surface Light
    /// </summary>
    internal class SurfaceLight
    {
        /// <summary>
        /// Change Surface Light Color
        /// </summary>
        internal void ChangeSurfaceLight()
        {
            List<UnityEngine.Color> colors = new [] 
            {
                Color.cyan, 
                Color.red, 
                Color.green, 
                Color.white, 
                Color.blue
            }.ToList();

            // Select a random color
            Color randomColor = colors[UnityEngine.Random.Range(0, colors.Count)];

            foreach (var room in Room.List.Where(r => r.Type == RoomType.Surface))
            {
                room.Color = randomColor; 
            }

            Log.Info($"Changed Surface light color to {randomColor}.");
        }
    }
}
