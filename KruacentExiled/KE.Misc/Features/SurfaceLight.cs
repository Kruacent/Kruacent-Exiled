using Exiled.API.Enums;
using Exiled.API.Features;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Extensions;

namespace KE.Misc.Features
{
    /// <summary>
    /// Everything about Surface Light
    /// </summary>
    internal class SurfaceLight : MiscFeature
    {

        private HashSet<Color> _colors = new()
        {
            Color.cyan,
            Color.red,
            Color.green,
            Color.white,
            Color.blue
        };


        public override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        }

        public override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;

        }


        private void OnRoundStarted()
        {
            if(Random.value < .25f)
                ChangeSurfaceLight();
        }

        private void ChangeSurfaceLight()
        {

            // Select a random color
            Color randomColor = _colors.GetRandomValue();
            foreach (var room in Room.List.Where(r => r.Type == RoomType.Surface))
            {
                room.Color = randomColor;
            }

            Log.Debug($"Changed Surface light color to {randomColor}.");
        }
    }
}
