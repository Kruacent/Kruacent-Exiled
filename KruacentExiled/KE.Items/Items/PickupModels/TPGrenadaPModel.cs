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
    public class TPGrenadaPModel : PickupModel
    {

        public TPGrenadaPModel(CustomItem customItem) : base(customItem) { }

        protected override bool HidePickup => true;


        private static Vector3 pillar = new Vector3(.025f, .125f, .025f);
        private static Vector3 support = new Vector3(.15f,.05f,.15f);
        private static Vector3 glass = new Vector3(.15f, .20f, .15f);

        private static Color32 centralColor = new Color32(60, 255, 255, 255);
        private static Color32 colorSupport = new Color32(80, 80, 80, 255);
        private static Color32 glassColor = new Color32(74, 232, 255, 50);

        protected override HashSet<AdminToyBlueprint> CreateModel()
        {
            HashSet<AdminToyBlueprint> model = new()
            {
                new PrimitiveBlueprint(PrimitiveType.Cube, Vector3.down*12.5f, Quaternion.identity, colorSupport,support),
                new PrimitiveBlueprint(PrimitiveType.Cube, Vector3.up*12.5f, Quaternion.identity, colorSupport,support),
                new PrimitiveBlueprint(PrimitiveType.Cube, Vector3.zero, Quaternion.identity, glassColor,glass),
                new PrimitiveBlueprint(PrimitiveType.Cylinder, Vector3.up + Vector3.back*6 + Vector3.right*6, Quaternion.identity,colorSupport, pillar),
                new PrimitiveBlueprint(PrimitiveType.Cylinder, Vector3.up + Vector3.forward*6 + Vector3.right*6, Quaternion.identity,colorSupport, pillar),
                new PrimitiveBlueprint(PrimitiveType.Cylinder, Vector3.up + Vector3.back*6 + Vector3.left*6, Quaternion.identity,colorSupport, pillar),
                new PrimitiveBlueprint(PrimitiveType.Cylinder, Vector3.up + Vector3.forward*6 + Vector3.left*6, Quaternion.identity,colorSupport, pillar),
                new PrimitiveBlueprint(PrimitiveType.Cylinder, Vector3.up, Quaternion.identity,centralColor, pillar),
                new LightBlueprint(Vector3.zero, Quaternion.identity, centralColor,Vector3.one,.1f),

            };
            return model;

        }
    }
}
