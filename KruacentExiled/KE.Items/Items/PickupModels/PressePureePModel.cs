/*using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using KE.Items.API.Features;
using KE.Utils.API.Models.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Items.Items.PickupModels
{
    public class PressePureePModel : PickupModel
    {

        public PressePureePModel(CustomItem customItem) : base(customItem) { }

        protected override bool HidePickup => true;

        private static Vector3 manche = new Vector3(.05f, .1f, .05f);
        private static Vector3 explosifCharge = new Vector3(.1f, .04f, .1f);

        public static readonly Color32 colorManche = new Color32(84, 43, 11, 255);
        public static readonly Color32 colorExplosif = new Color32(31, 31, 31, 255);

        protected override HashSet<AdminToyBlueprint> CreateModel()
        {
            HashSet<AdminToyBlueprint> model = new()
            {
                AdminToyBlueprint.Create(Primitive.Create(PrimitiveType.Cylinder, Vector3.zero, null, manche,false,colorManche)),
                AdminToyBlueprint.Create(Primitive.Create(PrimitiveType.Cylinder, new Vector3(0, 6f), null, explosifCharge,false,colorExplosif)),
            };
            return model;
            
        }
    }
}
*/