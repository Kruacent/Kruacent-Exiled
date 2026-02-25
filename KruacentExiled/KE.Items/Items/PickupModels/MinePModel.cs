using Exiled.API.Features;
using Exiled.API.Features.Toys;
using KE.Items.API.Core.Models;
using KE.Items.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.Items.Items.PickupModels
{
    public class MinePModel : PickupModel
    {

        public MinePModel(KECustomItem customItem) : base(customItem) { }

        public override float Scale => 0.25f;


        public static Color32 lightColor = new Color32(255, 0, 0, 0);
        protected override void CreateModel(Transform parent)
        {
            // This code was auto-generated

            var Mine = CreateEmptyPrimitive(
                parent,
                new Vector3(0, 0, 0),
                new Quaternion(0f, 0f, 0f, 1f),
                new Vector3(1f, 1f, 1f)
            );

            var botomCylinder = CreatePrimitive(
            Mine.transform,
            PrimitiveType.Cylinder,
            new Vector3(0f, 0f, 0f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(1f, 0.07f, 1f),
            new Color32(0, 101, 6, 255)
        );

            var middleCylinder = CreatePrimitive(
            Mine.transform,
            PrimitiveType.Cylinder,
            new Vector3(0f, 0.084f, 0f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.7f, 0.03f, 0.7f),
            new Color32(0, 101, 6, 255)
        );

            var topCylinder = CreatePrimitive(
            Mine.transform,
            PrimitiveType.Cylinder,
            new Vector3(0f, 0.121f, 0f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.6f, 0.03f, 0.6f),
            new Color32(0, 101, 6, 255)
        );

            var light = CreatePrimitive(
            Mine.transform,
            PrimitiveType.Sphere,
            new Vector3(0f, 0.135f, 0f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.08f, 0.08f, 0.08f),
            new Color32(255, 0, 0, 112)
        );

            var Point_Light = CreateLight(
            light.transform,
            new Vector3(0f, 0.9f, 0f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(12.5f, 12.5f, 12.5f),
            new Color32(255, 0, 0, 255),
            LightType.Point,
            3.48f,
            0.4f
        );
        }

    }
}
