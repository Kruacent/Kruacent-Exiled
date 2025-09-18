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
    public class MolotovPModel : PickupModel
    {

        public MolotovPModel(CustomItem customItem) : base(customItem) { }

        private Color bottleColor = Color.red;
        protected override HashSet<AdminToyBlueprint> CreateModel()
        {
            HashSet<AdminToyBlueprint> model = new()
            {
                AdminToyBlueprint.Create(Primitive.Create(PrimitiveType.Cylinder, Vector3.zero, null, new Vector3(.1f, 0.1f, .1f),false,bottleColor)),
                AdminToyBlueprint.Create(Primitive.Create(PrimitiveType.Cylinder, new Vector3(0, 0.1f), null, new Vector3(.05f, .04f, .05f),false,bottleColor)),
            };
            return model;
            
        }
    }
}
