using Exiled.API.Features.Objectives;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Firearms.Modules.Scp127;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Map.Surface.ElevatorGateA
{
    public static class CustomElevatorGateA
    {

        private static ElevatorModel model;
        private static Primitive prim;
        

        private static Primitive step;
        private static Primitive help;

        private static Panel toppanel;
        private static Primitive primtop;
        private static Panel bottompanel;
        private static Primitive primbottom;

        private const float Scale = 0.8f;
        public static void Create()
        {
            Destroy();
            model = new();

            toppanel = new();
            bottompanel = new();

            CreatePrimitives();

            CreateModels();
            baseheight = prim.Position.y;

            objective = increase;
            step = Primitive.Create(PrimitiveType.Cube, new(18.4f, 300.35f, -49.27f),null,new Vector3(2,.5f,1f));


            model.SendingElevator += SendingElevator;
            bottompanel.SendingElevator += SendingElevator;
            toppanel.SendingElevator += SendingElevator;
        }

        private static void CreateModels()
        {
            model.Create(prim.Transform);
            toppanel.Create(primtop.Transform);
            bottompanel.Create(primbottom.Transform);
            

            model.button.NetworkMaterialColor = Color.blue;
            bottompanel.button.NetworkMaterialColor = Color.blue;
            toppanel.button.NetworkMaterialColor = Color.blue;
        }

        private static void CreatePrimitives()
        {
            Vector3 pos = new(18.24f, 290.65f, -46.07f);
            Vector3 helppos = new(18.24f, 255.65f, -46.07f);
            Vector3 postop = new(19.92f, 299.97f, -48.95f);
            Vector3 posbot = new(15.62f, 290.65f, -45.19f);
            prim = Primitive.Create(pos, null, Vector3.one * Scale);
            prim.Flags = AdminToys.PrimitiveFlags.None;

            primtop = Primitive.Create(postop, null, Vector3.one * Scale);
            primtop.Flags = AdminToys.PrimitiveFlags.None;

            primbottom = Primitive.Create(posbot, null, Vector3.one * Scale);
            primbottom.Flags = AdminToys.PrimitiveFlags.None;


            help = Primitive.Create(PrimitiveType.Cube,helppos,null,new Vector3(50,50,50),false);
            help.Flags = AdminToys.PrimitiveFlags.Visible;
            help.Spawn();
            help.GameObject.AddComponent<CustomKillerCollision>();
        }

        private static void SendingElevator()
        {
            Send();
        }

        public static void Destroy()
        {
            if(prim != null)
            {
                model.SendingElevator -= SendingElevator;
                bottompanel.SendingElevator -= SendingElevator;
                toppanel.SendingElevator -= SendingElevator;
                model.Destroy(prim.Transform);
                toppanel.Destroy(primtop.Transform);
                bottompanel.Destroy(primbottom.Transform);
                step.Destroy();
                help.Destroy();
                prim = null;
                primbottom = null;
                primtop = null;
                step = null;
                help = null;
            }
            
        }

        private static bool isMoving = false;

        public static void Send()
        {
            if (isMoving) return;
            if (prim == null) return;

            Timing.RunCoroutine(Sending());

        }


        private static float baseheight;
        private static float increase = 301f;
        private static float objective;
        private static float duration = 5f;
        private static IEnumerator<float> Sending()
        {

            isMoving = true;
            model.button.NetworkMaterialColor = Color.yellow;
            bottompanel.button.NetworkMaterialColor = Color.yellow;
            toppanel.button.NetworkMaterialColor = Color.yellow;

            float startY = prim.Position.y;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;

                float t = elapsed / duration;
                t = t * t * (3f - 2f * t);

                float newY = Mathf.Lerp(startY, objective, t);

                prim.Position = new Vector3(
                    prim.Position.x,
                    newY,
                    prim.Position.z
                );
                yield return Timing.WaitForOneFrame;
            }

            prim.Position = new Vector3(
                prim.Position.x,
                objective,
                prim.Position.z
            );

            objective = Mathf.Approximately(objective, baseheight)
                ? increase
                : baseheight;

            model.button.NetworkMaterialColor = Color.blue;
            bottompanel.button.NetworkMaterialColor = Color.blue;
            toppanel.button.NetworkMaterialColor = Color.blue;
            isMoving = false;
        }



    }
}
