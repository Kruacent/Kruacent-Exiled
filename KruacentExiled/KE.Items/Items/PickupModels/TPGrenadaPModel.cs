using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using KE.Items.API.Core.Models;
using KE.Items.API.Features;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.Items.Items.PickupModels
{
    public class TPGrenadaPModel : PickupModel
    {
        private static Vector3 pillar = new Vector3(.15f, 1f, 0.15f);
        private static Vector3 support = new Vector3(1, .25f, 1);
        private static Vector3 glass = new Vector3(1, 1.75f, 1);
        private static float positionPillars = 2.6f;

        private static Color32 centralColor = new Color32(60, 255, 255, 255);
        private static Color32 colorSupport = new Color32(80, 80, 80, 255);
        private static Color32 glassColor = new Color32(74, 232, 255, 50);

        public TPGrenadaPModel(CustomItem customItem) : base(customItem)
        {
        }

        public override float Scale => .25f;

        protected override void CreateModel(Transform parent)
        {

            Primitive glassPrim = CreatePrimitive(parent, PrimitiveType.Cube, Vector3.zero, Quaternion.identity, glass, glassColor);
            //new PrimitiveBlueprint(PrimitiveType.Cube, Vector3.zero, Quaternion.identity, glassColor, glass),

            Primitive topSupport = CreatePrimitive(parent, PrimitiveType.Cube, Vector3.up, Quaternion.identity, support, colorSupport);
            Primitive bottomSupport = CreatePrimitive(parent, PrimitiveType.Cube, Vector3.down, Quaternion.identity, support, colorSupport);
            //new PrimitiveBlueprint(PrimitiveType.Cube, Vector3.up * 12.5f, Quaternion.identity, colorSupport, support),
            //new PrimitiveBlueprint(PrimitiveType.Cube, Vector3.down * 12.5f, Quaternion.identity, colorSupport, support),

            Primitive centralPillar = CreatePrimitive(parent, PrimitiveType.Cylinder, Vector3.zero, Quaternion.identity, pillar, centralColor);
            Primitive pillar1 = CreatePrimitive(centralPillar.Transform, PrimitiveType.Cylinder, positionPillars* new Vector3(1, 0, 1), Quaternion.identity, Vector3.one, colorSupport);
            Primitive pillar2 = CreatePrimitive(centralPillar.Transform, PrimitiveType.Cylinder, positionPillars * new Vector3(-1, 0, 1), Quaternion.identity, Vector3.one, colorSupport);
            Primitive pillar3 = CreatePrimitive(centralPillar.Transform, PrimitiveType.Cylinder, positionPillars * new Vector3(-1, 0, -1), Quaternion.identity, Vector3.one, colorSupport);
            Primitive pillar4 = CreatePrimitive(centralPillar.Transform, PrimitiveType.Cylinder, positionPillars * new Vector3(1, 0, -1), Quaternion.identity, Vector3.one, colorSupport);
            //new PrimitiveBlueprint(PrimitiveType.Cylinder, Vector3.up + Vector3.back * 6 + Vector3.right * 6, Quaternion.identity, colorSupport, pillar),
            //new PrimitiveBlueprint(PrimitiveType.Cylinder, Vector3.up + Vector3.forward * 6 + Vector3.right * 6, Quaternion.identity, colorSupport, pillar),
            //new PrimitiveBlueprint(PrimitiveType.Cylinder, Vector3.up + Vector3.back * 6 + Vector3.left * 6, Quaternion.identity, colorSupport, pillar),
            //new PrimitiveBlueprint(PrimitiveType.Cylinder, Vector3.up + Vector3.forward * 6 + Vector3.left * 6, Quaternion.identity, colorSupport, pillar),
            //new PrimitiveBlueprint(PrimitiveType.Cylinder, Vector3.up, Quaternion.identity, centralColor, pillar),

            Light light = CreateLight(parent, Vector3.zero, Quaternion.identity, Vector3.one, centralColor, LightType.Point, .1f);
            //new LightBlueprint(Vector3.zero, Quaternion.identity, centralColor, Vector3.one,),

        }
    }
}
