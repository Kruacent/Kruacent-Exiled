using Exiled.API.Features.Toys;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.Utils.Quality.Models.Base
{
    public class LightModel : BaseModel
    {

        /// <summary>
        /// Create a new Light for modeling
        /// </summary>
        public LightModel(Vector3? scale, Color? color, Vector3 localPositon, Quaternion localRotation) : base(localPositon, localRotation)
        {
            Light light = Light.Create(null, null, scale, false, color);
            _toy = light;
        }


        public override object Clone()
        {
            Light light = _toy as Light;
            return new LightModel(light.Scale, light.Color, LocalPosition, LocalRotation);
        }
    }
}
