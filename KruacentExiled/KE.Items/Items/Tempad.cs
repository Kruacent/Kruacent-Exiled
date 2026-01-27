using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Player;
using KE.Items.API.Features;
using KE.Utils.API.Features.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Items.Items
{

    //[CustomItem(ItemType.Flashlight)]
    public class Tempad : KECustomItem
    {

        public override uint Id { get; set; } = 1845;
        public override string Name { get; set; } = "Tempad";
        public override string Description { get; set; } = "drop to set a point; toggle the flashlight to activate the portal";
        public override float Weight { get; set; } = 0.65f;
        public override SpawnProperties SpawnProperties { get; set; } = null;





        protected override void OnDroppingItem(DroppingItemEventArgs ev)
        {
            Primitive prim = null;

            Collider c = prim.Base.gameObject.GetComponent<Collider>();

            
        }


    }



    public class TempadModel : ModelBase
    {
        protected override void CreateModel(Transform parent)
        {
            
        }
    }
}
