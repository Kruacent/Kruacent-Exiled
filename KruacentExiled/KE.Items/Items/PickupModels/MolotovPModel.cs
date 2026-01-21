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
        private static Color32 bottleColor = new Color32(110, 58, 13,230);
        private static Color32 meche = new Color32(255, 255, 255,255);

        public override float Scale => .15f;

        protected override void CreateModel(Transform parent)
        {
            Primitive base1 = CreatePrimitive(parent, PrimitiveType.Cylinder, Vector3.zero, Quaternion.identity, Vector3.one, bottleColor);
            Primitive base2 = CreatePrimitive(parent, PrimitiveType.Cylinder, Vector3.up *1.25f, Quaternion.identity, new(0.7f,0.28f,0.7f), bottleColor);
            Primitive base3 = CreatePrimitive(parent, PrimitiveType.Cylinder, Vector3.up *1.67f, Quaternion.identity, new(0.5f,0.3f,0.5f), bottleColor);

            Primitive empty = CreatePrimitive(parent, PrimitiveType.Sphere, new(0,2.4f,0.22f),Quaternion.identity,Vector3.one, meche);
            empty.Visible = false;

            Primitive meche1 = CreatePrimitive(empty.Transform, PrimitiveType.Cylinder, new Vector3(0, -.36f, -.16f), Quaternion.Euler(28.3f, 0, 0), new(0.3f, .2f, .3f), meche);
            Primitive meche2 = CreatePrimitive(empty.Transform, PrimitiveType.Cylinder, new Vector3(0, -.125f, .1f), Quaternion.Euler(62.5f, 0, 0), new(0.2f, .2f, .2f), meche);
            Primitive meche3 = CreatePrimitive(empty.Transform, PrimitiveType.Cylinder, new Vector3(0, 0, -.44f), Quaternion.Euler(79.5f, 0, 0), new(0.1f, .2f, .1f), meche);




        }
    }
}
