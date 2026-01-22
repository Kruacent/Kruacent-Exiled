using Exiled.API.Features;
using Exiled.API.Features.Toys;
using KE.Items.API.Core.Models;
using KE.Items.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.Items.Items.PickupModels
{
    public class MinePModel : PickupModel
    {

        public MinePModel(KECustomItem customItem) : base(customItem) { }

        public override float Scale => 0.25f;


        public static Color32 lightColor = new Color32(255, 0, 0, 0);
        protected override void CreateModel(Transform parent)
        {
            Primitive baseMine = CreatePrimitive(parent, PrimitiveType.Cylinder, Vector3.zero, Quaternion.identity, new Vector3(1, .05f, 1), new Color32(0, 0, 0, 255));
            Primitive baseLight = CreatePrimitive(parent, PrimitiveType.Sphere, Vector3.zero, Quaternion.identity, Vector3.one*0.2f, new Color32(255, 0, 0, 100));
            Light light = CreateLight(baseLight.Transform, Vector3.down/2f, Quaternion.identity, Vector3.one, lightColor, LightType.Point, .1f);
            


        }
    }
}
