using KE.Utils.API.Features.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Items.Items.Models
{
    public class MScanModel : ModelBase
    {

        protected override void CreateModel(Transform parent)
        {
            // This code was auto-generated

            var Mscan = CreateEmptyPrimitive(
                parent,
                new Vector3(0f, 0f, 0f),
                new Quaternion(0f, 0f, 0f, 1f),
                new Vector3(1f, 1f, 1f)
            );

            var bottom = CreatePrimitive(
                Mscan.transform,
                PrimitiveType.Cylinder,
                new Vector3(0f, 0f, 0f),
                new Quaternion(0f, 0f, 0f, 1f),
                new Vector3(5f, 0.5f, 5f),
                new Color32(115, 113, 115, 255),
                collidable:false
            );

            var middle = CreatePrimitive(
                Mscan.transform,
                PrimitiveType.Cylinder,
                new Vector3(0f, 1.69f, 0f),
                new Quaternion(0f, 0f, 0f, 1f),
                new Vector3(4.2f, 1.41875f, 4.2f),
                new Color32(57, 56, 57, 255),
                collidable: false
            );

            var topp = CreatePrimitive(
                Mscan.transform,
                PrimitiveType.Sphere,
                new Vector3(0f, 3f, 0f),
                new Quaternion(0f, 0f, 0f, 1f),
                new Vector3(4.2f, 4.2f, 4.2f),
                new Color32(57, 56, 57, 255),
                collidable: false
            );

            var contour = CreatePrimitive(
                Mscan.transform,
                PrimitiveType.Cylinder,
                new Vector3(0f, 2.81f, 0f),
                new Quaternion(0f, 0f, 0f, 1f),
                new Vector3(4.3f, 0.15f, 4.3f),
                new Color32(33, 33, 33, 255),
                collidable: false
            );

            var parentLight = CreateEmptyPrimitive(
                Mscan.transform,
                new Vector3(0f, -0.15f, 0f),
                new Quaternion(0f, 0f, 0f, 1f),
                new Vector3(1f, 1f, 1f)
            );

            var light1 = CreatePrimitive(
                parentLight.transform,
                PrimitiveType.Sphere,
                new Vector3(-0.7986f, 3.2654f, 0f),
                new Quaternion(0f, 0f, 0f, 1f),
                new Vector3(3.170261f, 3.587015f, 2.417214f),
                new Color32(175, 149, 16, 255),
                collidable: false
            );

            var contour1 = CreatePrimitive(
                light1.transform,
                PrimitiveType.Cylinder,
                new Vector3(-0.2495f, 0.1033f, 0f),
                new Quaternion(0f, 0f, 0.556803f, 0.8306445f),
                new Vector3(0.9123899f, 0.07510301f, 0.79f),
                new Color32(57, 56, 57, 255),
                collidable: false
            );

            var light2 = CreatePrimitive(
                parentLight.transform,
                PrimitiveType.Sphere,
                new Vector3(0.7986f, 3.2654f, 0f),
                new Quaternion(0f, 0f, 0f, 1f),
                new Vector3(3.170261f, 3.587015f, 2.417214f),
                new Color32(175, 149, 16, 255),
                collidable: false
            );

            var contour2 = CreatePrimitive(
                light2.transform,
                PrimitiveType.Cylinder,
                new Vector3(0.2495f, 0.1033f, 0f),
                new Quaternion(0f, 0f, -0.556803f, 0.8306445f),
                new Vector3(0.9123901f, 0.07510301f, 0.79f),
                new Color32(57, 56, 57, 255),
                collidable: false
            );
        }
    }
}
