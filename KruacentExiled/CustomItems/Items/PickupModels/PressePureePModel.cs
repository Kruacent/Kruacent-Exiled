using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Core.Models;
using UnityEngine;

namespace KruacentExiled.CustomItems.Items.PickupModels
{
    public class PressePureePModel : PickupModel
    {

        public PressePureePModel(CustomItem customItem) : base(customItem) { }


        private static Vector3 mancheScale = new Vector3(.5f, .885f, .5f);

        public static readonly Color32 colorManche = new Color32(84, 43, 11, 255);
        public static readonly Color32 colorExplosif = new Color32(31, 31, 31, 255);

        public override float Scale => 0.15f;

        protected override void CreateModel(Transform parent)
        {
            var manche = CreatePrimitive(parent,PrimitiveType.Cylinder,new Vector3(0,.006f,0),Quaternion.identity, mancheScale, colorManche);
            var manche2 = CreatePrimitive(parent,PrimitiveType.Cylinder,new Vector3(0,.9463f,0),Quaternion.identity, new Vector3(0.45f,0.05f,0.45f), colorExplosif);
            var explosif1 = CreatePrimitive(parent,PrimitiveType.Cylinder,new Vector3(0,1.2113f,0),Quaternion.identity, new Vector3(0.7f,0.2274f,0.7f), colorExplosif);
            var explosif2 = CreatePrimitive(parent,PrimitiveType.Cylinder,new Vector3(0,1,0),Quaternion.identity, new Vector3(0.8f,0.02f,0.8f), colorExplosif);
        }
    }
}
