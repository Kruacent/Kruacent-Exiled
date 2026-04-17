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

namespace KruacentExiled.Map.Surface.ElevatorGateA
{
    public class Panel : ModelBase, IContainPanel
    {
        public InteractableToy I_button { get; private set; }
        public PrimitiveObjectToy button { get; private set; }
        protected override void CreateModel(Transform parent)
        {
            // This code was auto-generated
            var panel = CreatePrimitive(
                parent,
                PrimitiveType.Sphere,
                new Vector3(0f, 0f, 0f),
                new Quaternion(0f, 0f, 0f, 1f),
                new Vector3(1f, 1f, 1f),
                new Color32(0, 0, 0, 0)
            );
            panel.PrimitiveFlags = PrimitiveFlags.None;

            var poteau = CreatePrimitive(
            panel.transform,
            PrimitiveType.Cube,
            new Vector3(0f, 0.757f, 0f),
            new Quaternion(0f, 0f, 0f, 1f),
            new Vector3(0.15f, 1.3f, 0.15f),
            new Color32(255, 255, 255, 255)
        );

            var panneau = CreatePrimitive(
            panel.transform,
            PrimitiveType.Cube,
            new Vector3(0f, 1.397f, 0f),
            new Quaternion(0.3775984f, 0f, 0f, 0.9259695f),
            new Vector3(1f, 1f, 0.15f),
            new Color32(255, 255, 255, 255)
        );

            button = CreatePrimitive(
            panel.transform,
            PrimitiveType.Cube,
            new Vector3(0f, 1.419f, 0f),
            new Quaternion(0.3775984f, 0f, 0f, 0.9259695f),
            new Vector3(0.8f, 0.8f, 0.15f),
            new Color32(255, 255, 255, 255)
        );

            I_button = CreateInteractable(
            panel.transform,
            new Vector3(0f, 1.46f, -0.042f),
            new Quaternion(0.3775984f, 0f, 0f, 0.9259695f),
            new Vector3(0.8f, 0.8f, 0.15f),
            InvisibleInteractableToy.ColliderShape.Box
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

        public void ChangeColor(Color newColor)
        {
            button.NetworkMaterialColor = newColor;
        }
    }
}
