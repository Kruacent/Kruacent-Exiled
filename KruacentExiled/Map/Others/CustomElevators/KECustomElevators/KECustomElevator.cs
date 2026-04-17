using Exiled.API.Features;
using Exiled.API.Features.Objectives;
using Exiled.API.Features.Toys;
using KE.Utils.API.Features;
using KE.Utils.Quality.Models;
using KruacentExiled.Map.Surface.ElevatorGateA;
using MEC;
using ProjectMER.Commands.Modifying.Scale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Profiling;
using UnityEngine;

namespace KruacentExiled.Map.Others.CustomElevators.KECustomElevators
{
    public class KECustomElevator : MonoBehaviour
    {


        public float BottomHeight;
        public float TopHeight = 301f;

        private float objective;


        private ElevatorModel model;
        private Panel toppanel;
        private Primitive primtop;
        private Panel bottompanel;
        private Primitive primbottom;


        private static Primitive step;
        private static Primitive help;


        private void Awake()
        {

            SpawnModel();
            CreatePrimitives();
            CreateModel();

            BottomHeight = transform.localPosition.y;

            objective = TopHeight;
            model.SendingElevator += TrySend;
            bottompanel.SendingElevator += TrySend;
            toppanel.SendingElevator += TrySend;
        }


        public const float Scale = 0.8f;

        private void SpawnModel()
        {
            model = new ElevatorModel();
            toppanel = new Panel();
            bottompanel = new Panel();

        }
        private void CreateModel()
        {
            model.Create(transform);
            toppanel.Create(transform);
            bottompanel.Create(transform);


            model.button.NetworkMaterialColor = Color.blue;
            bottompanel.button.NetworkMaterialColor = Color.blue;
            toppanel.button.NetworkMaterialColor = Color.blue;

        }

        private void CreatePrimitives()
        {
            Vector3 helppos = new Vector3(18.24f, 255.65f, -46.07f);
            Vector3 postop = new Vector3(19.92f, 299.97f, -48.95f);
            Vector3 posbot = new Vector3(15.62f, 290.65f, -45.19f);

            primtop = Primitive.Create(postop, null, Vector3.one * Scale);
            primtop.Flags = AdminToys.PrimitiveFlags.None;
            primbottom.Position = postop;

            primbottom = Primitive.Create(posbot, null, Vector3.one * Scale);
            primbottom.Position = posbot;
            primbottom.Flags = AdminToys.PrimitiveFlags.None;


            help = Primitive.Create(PrimitiveType.Cube, helppos, null, new Vector3(50, 50, 50), false);
            help.Flags = AdminToys.PrimitiveFlags.Visible;
            help.Spawn();
            help.GameObject.AddComponent<CustomKillerCollision>();
        }



        public void Destroy()
        {
            Destroy(this);
        }

        private void OnDestroy()
        {
            model.SendingElevator -= TrySend;
            bottompanel.SendingElevator -= TrySend;
            toppanel.SendingElevator -= TrySend;
            model.Destroy(transform);
            toppanel.Destroy(primtop.Transform);
            bottompanel.Destroy(primbottom.Transform);
            step.Destroy();
            help.Destroy();
            primbottom = null;
            primtop = null;
            step = null;
            help = null;
        }

        public void TrySend()
        {
            if (isMoving) return;

            startY = transform.position.y;
            elapsed = 0f;

            objective = Mathf.Approximately(startY, BottomHeight)
                ? TopHeight
                : BottomHeight;

            isMoving = true;
        }


        private bool isMoving = false;

        private float duration = 5f;

        private bool sending = false;
        private float elapsed = 0f;
        private float startY;
        private void SendingElevator()
        {
            if (isMoving)
            {
                model.button.NetworkMaterialColor = Color.yellow;
                bottompanel.button.NetworkMaterialColor = Color.yellow;
                toppanel.button.NetworkMaterialColor = Color.yellow;

                elapsed += Time.deltaTime;

                float t = Mathf.Clamp01(elapsed / duration);
                t = t * t * (3f - 2f * t); // smoothstep

                float newY = Mathf.Lerp(startY, objective, t);

                transform.position = new Vector3(
                    transform.position.x,
                    newY,
                    transform.position.z
                );

                if (elapsed >= duration)
                {
                    transform.position = new Vector3(
                        transform.position.x,
                        objective,
                        transform.position.z
                    );

                    isMoving = false;
                }
            }
            else
            {
                model.button.NetworkMaterialColor = Color.blue;
                bottompanel.button.NetworkMaterialColor = Color.blue;
                toppanel.button.NetworkMaterialColor = Color.blue;
            }
        }
        private void Update()
        {
            SendingElevator();
        }



    }
}
