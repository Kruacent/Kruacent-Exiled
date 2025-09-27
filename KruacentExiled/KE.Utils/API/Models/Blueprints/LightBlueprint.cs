using Exiled.API.Enums;
using Exiled.API.Features.Toys;
using System;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.Utils.API.Models.Blueprints
{
    public class LightBlueprint : AdminToyBlueprint
    {
        public Color Color { get; }
        public float Intensity { get; }


        public LightBlueprint(Vector3 position, Quaternion rotation, Color color, Vector3 scale,float intensity, Vector3? center = null) : base(position, rotation, center)
        {
            
            AdminToyType = AdminToyType.LightSource;
            Scale = scale;
            Color = color;
            Intensity = intensity;
        }

        public LightBlueprint(Light l) : base(l.Position,l.Rotation)
        {
            AdminToyType = AdminToyType.PrimitiveObject;
            Scale = l.Scale;
            Color = l.Color;
            Intensity = l.Intensity;
        }


        public override AdminToy Spawn(Vector3 center)
        {
            var l = Light.Create(Position+center, Rotation, Scale, false);
            l.Color = Color;
            l.Intensity = Intensity;


            return l;
        }

        protected override string Load(char separator)
        {
            return "#" + ColorUtility.ToHtmlStringRGBA(Color) + separator + Intensity;
        }
    }
}
