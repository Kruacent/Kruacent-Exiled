using Exiled.API.Features;
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
    public class MolotovPModel : PickupModel
    {

        public MolotovPModel(CustomItem customItem) : base(customItem) { }

        protected override bool HidePickup => true;
        private Color32 bottleColor = new Color32(110, 58, 13,230);
        protected override HashSet<AdminToyBlueprint> CreateModel()
        {
            HashSet<AdminToyBlueprint> model = new()
            {
                AdminToyBlueprint.Create(Primitive.Create(PrimitiveType.Cylinder, Vector3.zero, null, new Vector3(.1f, 0.1f, .1f),false,bottleColor)), //big
                AdminToyBlueprint.Create(Primitive.Create(PrimitiveType.Cylinder, new Vector3(0, 10f), null, new Vector3(.05f, .035f, .05f),false,bottleColor)),
            };
            return model;
            
        }
    }
}
