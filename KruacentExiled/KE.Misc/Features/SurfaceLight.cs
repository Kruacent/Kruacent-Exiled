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

        public static readonly HashSet<Color> _colors = new HashSet<Color>()
        {
            Color.cyan,
            Color.red,
            Color.green,
            Color.blue
        };

        private float _chance = 5;

        public float Chance
        {
            get { return _chance; }
            set
            {
                _chance = Mathf.Clamp(value, 0f, 100f);
            }
        }


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
            if (!MainPlugin.Configs.SurfaceLight)
            {
                return;
            }

            if(Random.Range(0f,100f) < Chance)
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
        }
    }
}
