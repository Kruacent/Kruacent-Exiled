using Exiled.API.Enums;
using Exiled.API.Features.Toys;
using UnityEngine;

namespace KE.Utils.API.Models.Blueprints
{
    public class PrimitiveBlueprint : AdminToyBlueprint
    {
        public Color Color { get; }
        public PrimitiveType Type { get; }
        public bool Collidable { get; }


        public PrimitiveBlueprint(Primitive p)
        {
            AdminToyType = AdminToyType.PrimitiveObject;

            Scale = p.Scale;
            Color = p.Color;
            Type = p.Type;
            Collidable = p.Collidable;
        }

        

        public override AdminToy Spawn(Vector3 center)
        {
            var p  = Primitive.Create(Type,Position+center, Rotation, Scale, false);
            p.Collidable = Collidable;
            p.Color = Color;


            return p;
        }

        protected override string Load(char separator)
        {
            return "#" + ColorUtility.ToHtmlStringRGBA(Color) + separator+Type+separator+Collidable;
        }
    }
}
