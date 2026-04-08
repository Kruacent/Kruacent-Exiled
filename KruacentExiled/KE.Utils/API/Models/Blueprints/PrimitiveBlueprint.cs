using Exiled.API.Enums;
using Exiled.API.Features.Toys;
using UnityEngine;

namespace KE.Utils.API.Models.Blueprints
{
    public class PrimitiveBlueprint : AdminToyBlueprint
    {
        public Color Color { get; }
        public PrimitiveType Type { get; }


        public PrimitiveBlueprint(PrimitiveType type, Vector3 position,Quaternion rotation,Color color,Vector3 scale,Vector3? center = null) : base(position,rotation, center)
        {
            AdminToyType = AdminToyType.PrimitiveObject;
            Scale = scale;
            Color = color;
            Type = type;
        }

        public PrimitiveBlueprint(Primitive p) : base(p.Position,p.Rotation)
        {
            AdminToyType = AdminToyType.PrimitiveObject;
            
            Scale = p.Scale;
            Color = p.Color;
            Type = p.Type;
        }

        

        public override AdminToy Spawn(Vector3 center)
        {
            var p  = Primitive.Create(Type,Position+center, Rotation, Scale, false);
            p.Collidable = false;
            p.Color = Color;


            return p;
        }

        protected override string Load(char separator)
        {
            return "#" + ColorUtility.ToHtmlStringRGBA(Color) + separator+Type;
        }
    }
}
