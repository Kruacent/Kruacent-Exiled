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
        public TPGrenadaPModel(CustomItem customItem) : base(customItem)
        {
        }

        public override float Scale => .15f;

        protected override void CreateModel(Transform parent)
        {
            // This code was auto-generated

            var Tpgrenada = CreateEmptyPrimitive(
                parent,
                new Vector3(0f, 0f, 0f),
                new Quaternion(0f, 0f, 0f, 1f),
                new Vector3(1.21f, 1.21f, 1.21f)
            );

            var bottomSupport = CreatePrimitive(
            Tpgrenada.transform,
            PrimitiveType.Cube,
            new Vector3(0f, -1f, 0f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(1f, 0.25f, 1f),
            new Color32(217, 217, 217, 255)
        );

            var topSupport = CreatePrimitive(
            Tpgrenada.transform,
            PrimitiveType.Cube,
            new Vector3(0f, 1f, 0f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(1f, 0.25f, 1f),
            new Color32(217, 217, 217, 255)
        );

            var pillars = CreateEmptyPrimitive(
                Tpgrenada.transform,
                new Vector3(0f, 0f, 0f),
                new Quaternion(0f, 0f, 0f, 1f),
                new Vector3(1f, 1f, 1f)
            );

            var pillar1 = CreatePrimitive(
            pillars.transform,
            PrimitiveType.Cylinder,
            new Vector3(-0.4f, 0f, 0.4f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.1f, 1f, 0.1f),
            new Color32(217, 217, 217, 255)
        );

            var pillar2 = CreatePrimitive(
            pillars.transform,
            PrimitiveType.Cylinder,
            new Vector3(0.4f, 0f, 0.4f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.1f, 1f, 0.1f),
            new Color32(217, 217, 217, 255)
        );

            var pillar3 = CreatePrimitive(
            pillars.transform,
            PrimitiveType.Cylinder,
            new Vector3(0.4f, 0f, -0.4f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.1f, 1f, 0.1f),
            new Color32(217, 217, 217, 255)
        );

            var pillar4 = CreatePrimitive(
            pillars.transform,
            PrimitiveType.Cylinder,
            new Vector3(-0.4f, 0f, -0.4f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.1f, 1f, 0.1f),
            new Color32(217, 217, 217, 255)
        );

            var centralPillar = CreatePrimitive(
            pillars.transform,
            PrimitiveType.Cylinder,
            new Vector3(0f, 0f, 0f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.3f, 0.8f, 0.3f),
            new Color32(0, 255, 255, 255)
        );

            var glass = CreatePrimitive(
            Tpgrenada.transform,
            PrimitiveType.Cube,
            new Vector3(0f, 0f, 0f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(1f, 1.75f, 1f),
            new Color32(54, 219, 243, 69)
        );

            var innerglass = CreatePrimitive(
            Tpgrenada.transform,
            PrimitiveType.Cylinder,
            new Vector3(0f, 0f, 0f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.7f, 1f, 0.7f),
            new Color32(54, 219, 243, 69)
        );

            var lights = CreateEmptyPrimitive(
                Tpgrenada.transform,
                new Vector3(0f, 0f, 0f),
                new Quaternion(0f, 0f, 0f, 1f),
                new Vector3(1f, 1f, 1f)
            );

            var top = CreateLight(
            lights.transform,
            new Vector3(0f, 0.5f, 0f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(1f, 1f, 1f),
            new Color32(0, 255, 255, 255),
            LightType.Point,
            1f,
            1.25f
        );

            var center = CreateLight(
            lights.transform,
            new Vector3(0f, 0f, 0f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(1f, 1f, 1f),
            new Color32(0, 255, 255, 255),
            LightType.Point,
            1f,
            1.25f
        );

            var bottom = CreateLight(
            lights.transform,
            new Vector3(0f, -0.5f, 0f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(1f, 1f, 1f),
            new Color32(0, 255, 255, 255),
            LightType.Point,
            1f,
            1.25f
        );

            var topbase = CreatePrimitive(
            Tpgrenada.transform,
            PrimitiveType.Cylinder,
            new Vector3(0f, 0.9f, 0f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.9f, 0.1f, 0.9f),
            new Color32(255, 255, 255, 255)
        );

            var botbase = CreatePrimitive(
            Tpgrenada.transform,
            PrimitiveType.Cylinder,
            new Vector3(0f, -0.9f, 0f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.9f, 0.1f, 0.9f),
            new Color32(255, 255, 255, 255)
        );
        }

    }
}
