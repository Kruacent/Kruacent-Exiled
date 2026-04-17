using Exiled.API.Features;
using Exiled.API.Features.Toys;
using KE.Utils.API.Features;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KruacentExiled.Map.Surface.ElevatorGateA
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
        private static Primitive helpPlatform;

        private const float Scale = 0.8f;
        public static void Create()
        {
            //Vector3 pos = new(18.24f, 290.65f, -46.07f);
            //prim = Primitive.Create(pos, null, Vector3.one * Scale);
            //prim.Flags = AdminToys.PrimitiveFlags.None;


            //prim.GameObject.AddComponent<KECustomElevator>();

            try
            {
                Destroy();
            }
            catch(Exception e)
            {
                Log.Error(e);
            }
            
            model = new ElevatorModel();

            toppanel = new Panel();
            bottompanel = new Panel();

            CreatePrimitives();

            CreateModels();
            step = Primitive.Create(PrimitiveType.Cube, new Vector3(18.4f, 300.35f, -49.27f),null,new Vector3(2,.5f,1f));


            model.SendingElevator += SendingElevator;
            bottompanel.SendingElevator += SendingElevator;
            toppanel.SendingElevator += SendingElevator;
        }

        private static void CreateModels()
        {
            KELog.Debug("creating elevator");
            model.Create(prim.Transform);
            KELog.Debug("creating top panel");
            toppanel.Create(primtop.Transform);
            KELog.Debug("creating bototm panel");
            bottompanel.Create(primbottom.Transform);
            

            model.button.NetworkMaterialColor = Color.blue;
            bottompanel.button.NetworkMaterialColor = Color.blue;
            toppanel.button.NetworkMaterialColor = Color.blue;
        }

        private static void CreatePrimitives()
        {
            Vector3 pos = new Vector3(18.24f, 290.65f, -46.07f);
            Vector3 helppos = new Vector3(18.24f, 255.65f, -46.07f);
            Vector3 postop = new Vector3(19.92f, 299.97f, -48.95f);
            Vector3 posbot = new Vector3(15.62f, 290.65f, -45.19f);
            prim = Primitive.Create(pos, null, Vector3.one * Scale);
            prim.Flags = AdminToys.PrimitiveFlags.None;
            prim.MovementSmoothing = 0;
            primtop = Primitive.Create(postop, null, Vector3.one * Scale);
            primtop.Flags = AdminToys.PrimitiveFlags.None;

            primbottom = Primitive.Create(posbot, null, Vector3.one * Scale);
            primbottom.Flags = AdminToys.PrimitiveFlags.None;


            help = Primitive.Create(PrimitiveType.Cube,helppos,null,new Vector3(50,50,50),false);
            help.Flags = AdminToys.PrimitiveFlags.Visible;
            help.Spawn();
            help.GameObject.AddComponent<CustomKillerCollision>();

            helpPlatform = Primitive.Create(PrimitiveType.Cube, helppos, null, new Vector3(50, 1, 50), false);
            helpPlatform.Flags = AdminToys.PrimitiveFlags.Visible | AdminToys.PrimitiveFlags.Collidable;
            helpPlatform.Spawn();
        }

        private static void SendingElevator()
        {
            Send();
        }

        public static void Destroy()
        {
            if(CheckPrimitive(prim))
            {
                model.SendingElevator -= SendingElevator;
                model.Destroy(prim.Transform);
                prim = null;
            }

            if(CheckPrimitive(step))
            {
                step.Destroy();
                step = null;
            }
            if (CheckPrimitive(help))
            {
                help.Destroy();
                help = null;
            }
            if (CheckPrimitive(helpPlatform))
            {
                helpPlatform.Destroy();
                helpPlatform = null;
            }



            if (CheckPrimitive(primbottom))
            {
                
                bottompanel.SendingElevator -= SendingElevator;
                bottompanel.Destroy(primbottom.Transform);
                primbottom = null;
            }

            if(CheckPrimitive(primtop))
            {
                toppanel.SendingElevator -= SendingElevator;
                toppanel.Destroy(primtop.Transform);
                
                primtop = null;
            }
        }

        private static bool CheckPrimitive(Primitive primitive)
        {
            return primitive != null && primitive.Base != null && primitive.GameObject != null;
        }

        public static void Send()
        {
            if (prim == null) return;
            if (!prim.GameObject.TryGetComponent<CustomElevatorComp>(out var comp))
            {
                comp = prim.GameObject.AddComponent<CustomElevatorComp>();
                comp.Init(prim, new List<IContainPanel>() { model, bottompanel, toppanel });
            }
            comp.Send();
        }
    }
}
