using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using KE.Items.API.Core.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KE.Items.Items.PickupModels
{
    public class MolotovPModel : PickupModel
    {

        public MolotovPModel(CustomItem customItem) : base(customItem) { }
        private static Color32 bottleColor = new Color32(148, 37, 1,233);

        public override float Scale => .15f;

        protected override void CreateModel(Transform parent)
        {
            var base1 = CreatePrimitive(parent, PrimitiveType.Cylinder, Vector3.up*.7f, Quaternion.identity, new Vector3(0.6f, 0.09f, 0.6f), bottleColor);
            var base2 = CreatePrimitive(parent, PrimitiveType.Cylinder, Vector3.down *.08f, Quaternion.identity, new Vector3(0.8f,0.7f,0.8f), bottleColor);
            var base3 = CreatePrimitive(parent, PrimitiveType.Cylinder, Vector3.up *.9f, Quaternion.identity, new Vector3(0.4f,0.1f,0.4f), bottleColor);
            var base4 = CreatePrimitive(parent, PrimitiveType.Cylinder, Vector3.up *1.4f, Quaternion.identity, new Vector3(0.3f,0.4f,0.3f), bottleColor);
            var base5 = CreatePrimitive(parent, PrimitiveType.Cylinder, Vector3.down *.8f, Quaternion.identity, new Vector3(0.6f,0.06f,0.6f), bottleColor);
        }
    }
}
