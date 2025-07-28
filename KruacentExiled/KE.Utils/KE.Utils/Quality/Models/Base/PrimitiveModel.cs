using Exiled.API.Features.Toys;
using UnityEngine;
using Color = UnityEngine.Color;

namespace KE.Utils.Quality.Models.Base
{
    public class PrimitiveModel : BaseModel
    {
        /// <summary>
        /// Create a new Primitive for modeling
        /// </summary>
        public PrimitiveModel(PrimitiveType type, Vector3? scale, Color? color,Vector3 localPosition,Quaternion localRotation) : base(localPosition, localRotation)
        {
            
            Primitive prim = Primitive.Create(type, null, null, scale, false, color);
            prim.Collidable = false;
            _toy = prim;
        }

        public override object Clone()
        {
            Primitive prim = _toy as Primitive;
            return new PrimitiveModel(prim.Type, prim.Scale, prim.Color, LocalPosition, LocalRotation);
        }
    }
}
