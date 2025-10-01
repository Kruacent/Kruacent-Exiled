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
    public class Scp3136PModel : PickupModel
    {

        public Scp3136PModel(CustomItem customItem) : base(customItem) { }

        protected override bool HidePickup => true;


        private static Vector3 paper = new Vector3(.5f, .1f, .5f);
        private static Vector3 pen = new Vector3(.2f,.5f,.2f);

        protected override HashSet<AdminToyBlueprint> CreateModel()
        {
            HashSet<AdminToyBlueprint> model = new()
            {

                new PrimitiveBlueprint(PrimitiveType.Cube,Vector3.zero,Quaternion.identity,Color.white,paper),
                new PrimitiveBlueprint(PrimitiveType.Cylinder,new Vector3(paper.x, paper.y+pen.x*2,-paper.z),Quaternion.Euler(Vector3.left),Color.red,pen),
            };
            return model;
            
        }
    }
}
