using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using KE.Items.API.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Items.Items.PickupModels
{
    public class HolyGrenadePModel : PickupModel
    {
        public HolyGrenadePModel(CustomItem customItem) : base(customItem)
        {
        }

        private static Color32 red = new Color32(255, 0, 0, 255);
        private static Color32 gold = new Color32(255, 215, 0, 255);
        private static Color32 white = new Color32(255, 255, 255, 255);


        public override float Scale => 0.2f;

        protected override void CreateModel(Transform parent)
        {


            var sphere = CreatePrimitive(parent, PrimitiveType.Sphere, Vector3.zero, Quaternion.identity, Vector3.one, white);

            var gem = CreatePrimitive(parent, PrimitiveType.Sphere, new Vector3(0,0.8f,0), Quaternion.identity, new Vector3(.15f,.1f,.1f), red);

            var crossV = CreatePrimitive(parent, PrimitiveType.Cube, new Vector3(0, 0.74f, 0), Quaternion.identity, new Vector3(.1f, .5f, .1f), white);
            var crossH = CreatePrimitive(parent, PrimitiveType.Cube, new Vector3(0, 0.8f, 0), Quaternion.identity, new Vector3(.1f, .1f, .37f), white);

            var ring1 = CreatePrimitive(parent, PrimitiveType.Cylinder, Vector3.zero, Quaternion.Euler(90,0,0), new Vector3(1.05f, .05f, 1.05f), gold);
            var ring2 = CreatePrimitive(parent, PrimitiveType.Cylinder, Vector3.zero, Quaternion.identity, new Vector3(1.05f, .05f, 1.05f), gold);
            var ring3 = CreatePrimitive(parent, PrimitiveType.Cylinder, Vector3.zero, Quaternion.Euler(90, 90, 0), new Vector3(1.05f, .05f, 1.05f), gold);

            var littleBase = CreatePrimitive(parent, PrimitiveType.Cylinder, new Vector3(0, 0.5f, 0), Quaternion.identity, new Vector3(.3f, .05f, .3f), gold);

        }
    }
}
