using AdminToys;
using Exiled.API.Features.Toys;
using KE.Utils.API.Features.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UserSettings.ServerSpecific;

namespace KE.Map.Surface.ElevatorGateA
{
    public class ElevatorModel : ModelBase
    {
        public InteractableToy I_button { get; private set; }
        public PrimitiveObjectToy button { get; private set; }
        protected override void CreateModel(Transform parent)
        {
            // This code was auto-generated

            var ElevatorCabin = CreatePrimitive(
                parent,
                PrimitiveType.Sphere,
                new Vector3(0f, 0f, 0f),
                new Quaternion(0f, 0f, 0f, 1f),
                new Vector3(1f, 1f, 1f),
                new Color32(0, 0, 0, 0)
            );
            ElevatorCabin.PrimitiveFlags = AdminToys.PrimitiveFlags.None;

            var top = CreatePrimitive(
                ElevatorCabin.transform,
                PrimitiveType.Sphere,
                new Vector3(0f, 5f, 0f),
                new Quaternion(0f, 0f, 0f, 1f),
                new Vector3(1f, 1f, 1f),
                new Color32(0, 0, 0, 0)
            );
            top.PrimitiveFlags = AdminToys.PrimitiveFlags.None;

            var right = CreatePrimitive(
            top.transform,
            PrimitiveType.Cube,
            new Vector3(1.666667f, 0f, 0f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(1.666667f, 0.2f, 5f),
            new Color32(255, 255, 255, 255)
        );

            var left = CreatePrimitive(
            top.transform,
            PrimitiveType.Cube,
            new Vector3(-1.666667f, 0f, 0f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(1.666667f, 0.2f, 5f),
            new Color32(255, 255, 255, 255)
        );

            var up = CreatePrimitive(
            top.transform,
            PrimitiveType.Cube,
            new Vector3(0f, 0f, 1.666667f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(1.666667f, 0.2f, 1.666667f),
            new Color32(255, 255, 255, 255)
        );

            var down = CreatePrimitive(
            top.transform,
            PrimitiveType.Cube,
            new Vector3(0f, 0f, -1.666667f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(1.666667f, 0.2f, 1.666667f),
            new Color32(255, 255, 255, 255)
        );

            var panel = CreatePrimitive(
                ElevatorCabin.transform,
                PrimitiveType.Sphere,
                new Vector3(0f, 0f, 0f),
                new Quaternion(0f, 0f, 0f, 1f),
                new Vector3(1f, 1f, 1f),
                new Color32(0, 0, 0, 0)
            );
            panel.PrimitiveFlags = AdminToys.PrimitiveFlags.None;

            var poteau = CreatePrimitive(
            panel.transform,
            PrimitiveType.Cube,
            new Vector3(0f, 0.757f, 1.667f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.15f, 1.3f, 0.15f),
            new Color32(255, 255, 255, 255)
        );

            var panneau = CreatePrimitive(
            panel.transform,
            PrimitiveType.Cube,
            new Vector3(0f, 1.397f, 1.667f),
            new Quaternion(0.3775984f, 0f, 0f, 0.9259695f),
            new Vector3(1f, 1f, 0.15f),
            new Color32(255, 255, 255, 255)
        );

            button = CreatePrimitive(
            panel.transform,
            PrimitiveType.Cube,
            new Vector3(0f, 1.419f, 1.644f),
            new Quaternion(0.3775984f, 0f, 0f, 0.9259695f),
            new Vector3(0.8f, 0.8f, 0.15f),
            new Color32(255, 255, 255, 255)
        );

            I_button = CreateInteractable(
            panel.transform,
            new Vector3(0f, 1.434f, 1.629f),
            new Quaternion(0.3775984f, 0f, 0f, 0.9259695f),
            new Vector3(0.8f, 0.8f, 0.15f),
            InvisibleInteractableToy.ColliderShape.Box
        );

            var bottom = CreatePrimitive(
                ElevatorCabin.transform,
                PrimitiveType.Sphere,
                new Vector3(0f, 0f, 0f),
                new Quaternion(0f, 0f, 0f, 1f),
                new Vector3(1f, 1f, 1f),
                new Color32(0, 0, 0, 0)
            );
            bottom.PrimitiveFlags = AdminToys.PrimitiveFlags.None;

            var floor = CreatePrimitive(
            bottom.transform,
            PrimitiveType.Cube,
            new Vector3(0f, 0f, 0f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(5f, 0.2f, 5f),
            new Color32(255, 255, 255, 255)
        );

            var fence = CreatePrimitive(
                bottom.transform,
                PrimitiveType.Sphere,
                new Vector3(0f, 0f, 0f),
                new Quaternion(0f, 0f, 0f, 1f),
                new Vector3(1f, 1f, 1f),
                new Color32(0, 0, 0, 0)
            );
            fence.PrimitiveFlags = AdminToys.PrimitiveFlags.None;

            var pillar1 = CreatePrimitive(
            fence.transform,
            PrimitiveType.Cube,
            new Vector3(-2.3f, 2.5f, -2.3f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.1f, 4.9f, 0.1f),
            new Color32(255, 255, 255, 255)
        );

            var pillar2 = CreatePrimitive(
            fence.transform,
            PrimitiveType.Cube,
            new Vector3(-2.3f, 0.79f, -0.9f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.1f, 1.4798f, 0.1f),
            new Color32(255, 255, 255, 255)
        );

            var pillar3 = CreatePrimitive(
            fence.transform,
            PrimitiveType.Cube,
            new Vector3(-2.3f, 0.79f, 0.9f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.1f, 1.4798f, 0.1f),
            new Color32(255, 255, 255, 255)
        );

            var pillar4 = CreatePrimitive(
            fence.transform,
            PrimitiveType.Cube,
            new Vector3(-2.3f, 2.5f, 2.3f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.1f, 4.9f, 0.1f),
            new Color32(255, 255, 255, 255)
        );

            var pillar5 = CreatePrimitive(
            fence.transform,
            PrimitiveType.Cube,
            new Vector3(2.3f, 2.5f, -2.3f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.1f, 4.9f, 0.1f),
            new Color32(255, 255, 255, 255)
        );

            var pillar6 = CreatePrimitive(
            fence.transform,
            PrimitiveType.Cube,
            new Vector3(2.3f, 0.79f, 0f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.1f, 1.4798f, 0.1f),
            new Color32(255, 255, 255, 255)
        );

            var pillar8 = CreatePrimitive(
            fence.transform,
            PrimitiveType.Cube,
            new Vector3(2.3f, 2.5f, 2.3f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.1f, 4.9f, 0.1f),
            new Color32(255, 255, 255, 255)
        );

            var pillar9 = CreatePrimitive(
            fence.transform,
            PrimitiveType.Cube,
            new Vector3(0f, 0.79f, 2.3f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.1f, 1.4798f, 0.1f),
            new Color32(255, 255, 255, 255)
        );

            var pillar10 = CreatePrimitive(
            fence.transform,
            PrimitiveType.Cube,
            new Vector3(-0.9f, 0.79f, -2.3f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.1f, 1.4798f, 0.1f),
            new Color32(255, 255, 255, 255)
        );

            var pillar11 = CreatePrimitive(
            fence.transform,
            PrimitiveType.Cube,
            new Vector3(0.9f, 0.79f, -2.3f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.1f, 1.4798f, 0.1f),
            new Color32(255, 255, 255, 255)
        );

            var fenceback = CreatePrimitive(
            fence.transform,
            PrimitiveType.Cube,
            new Vector3(0f, 1.439f, 2.3f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(5f, 0.2f, 0.2f),
            new Color32(255, 255, 255, 255)
        );

            var fenceright = CreatePrimitive(
            fence.transform,
            PrimitiveType.Cube,
            new Vector3(2.3f, 1.439f, 0f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.2f, 0.2f, 5f),
            new Color32(255, 255, 255, 255)
        );

            var fenceleft1 = CreatePrimitive(
            fence.transform,
            PrimitiveType.Cube,
            new Vector3(-2.3f, 1.439f, 1.6f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.2f, 0.2f, 1.7f),
            new Color32(255, 255, 255, 255)
        );

            var fenceleft2 = CreatePrimitive(
            fence.transform,
            PrimitiveType.Cube,
            new Vector3(-2.3f, 1.439f, -1.6f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.2f, 0.2f, 1.7f),
            new Color32(255, 255, 255, 255)
        );

            var fencefront1 = CreatePrimitive(
            fence.transform,
            PrimitiveType.Cube,
            new Vector3(1.6f, 1.439f, -2.35f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(1.7f, 0.2f, 0.2f),
            new Color32(255, 255, 255, 255)
        );

            var fencefront2 = CreatePrimitive(
            fence.transform,
            PrimitiveType.Cube,
            new Vector3(-1.6f, 1.439f, -2.3f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(1.7f, 0.2f, 0.2f),
            new Color32(255, 255, 255, 255)
        );

            var waypoint = CreateWaypoint(
            ElevatorCabin.transform,
            new Vector3(0f, 3.96f, 0f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(4.9f, 7.870179f, 4.9f)
        );
            I_button.Base.OnInteracted += Base_OnInteracted;
        }

        public override void Destroy(Transform parent)
        {
            I_button.Base.OnInteracted -= Base_OnInteracted;
            base.Destroy(parent);
        }


        public event Action SendingElevator = delegate { };

        private void Base_OnInteracted(ReferenceHub obj)
        {
            SendingElevator?.Invoke();
        }
    }
}
