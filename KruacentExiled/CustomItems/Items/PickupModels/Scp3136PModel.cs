using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Core.Models;
using UnityEngine;

namespace KruacentExiled.CustomItems.Items.PickupModels
{
    public class Scp3136PModel : PickupModel
    {

        public Scp3136PModel(CustomItem customItem) : base(customItem) { }


        public override float Scale => .5f;

        protected override void CreateModel(Transform parent)
        {





            Primitive land = Primitive.Get(CreatePrimitive(parent, PrimitiveType.Cube, Vector3.up * -0.035f,Quaternion.identity,Vector3.one,new Color32()));
            land.Flags = AdminToys.PrimitiveFlags.None;



            #region land




            #endregion


        }
    }
}
