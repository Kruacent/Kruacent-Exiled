using Exiled.API.Enums;
using Exiled.API.Features.Toys;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.Utils.API.Models.Blueprints
{
    public class LightBlueprint : AdminToyBlueprint
    {
        public Color Color { get; }
        public float Intensity { get; }

        public LightBlueprint(Light l)
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
