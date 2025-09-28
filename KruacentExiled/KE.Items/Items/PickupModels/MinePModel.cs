using Exiled.API.Features;
using Exiled.API.Features.Toys;
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
    public class MinePModel : PickupModel
    {

        public MinePModel(KECustomItem customItem) : base(customItem) { }

        protected override HashSet<AdminToyBlueprint> CreateModel()
        {
            HashSet<AdminToyBlueprint> model = new HashSet<AdminToyBlueprint>()
            {
                AdminToyBlueprint.Create(Primitive.Create(PrimitiveType.Cylinder, Vector3.zero, null, new Vector3(.3f, 0.05f, .3f),false,Color.black)),
                AdminToyBlueprint.Create(Primitive.Create(PrimitiveType.Sphere, new Vector3(0, 0.05f), null, new Vector3(.05f, .05f, .05f),false,Color.red)),
            };
            return model;
            
        }
    }
}
