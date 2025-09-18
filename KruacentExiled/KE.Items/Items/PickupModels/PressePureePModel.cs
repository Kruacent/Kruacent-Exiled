using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using KE.Items.Features;
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

        protected override Vector3 PickupSize { get; set; } = new Vector3(.5f, 1.5f, .5f);


        private static Vector3 manche = new Vector3(.05f, .06f, .1f);
        private static Vector3 explosifCharge = new Vector3(.1f, .04f, .1f);

        protected override HashSet<AdminToyBlueprint> CreateModel()
        {
            HashSet<AdminToyBlueprint> model = new()
            {
                AdminToyBlueprint.Create(Primitive.Create(PrimitiveType.Cylinder, Vector3.zero, null, manche,false,Color.red)),
                AdminToyBlueprint.Create(Primitive.Create(PrimitiveType.Cylinder, new Vector3(0, .1f), null, explosifCharge,false,Color.black)),
            };
            return model;
            
        }
    }
}
