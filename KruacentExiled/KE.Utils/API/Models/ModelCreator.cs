using AdminToys;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Interfaces;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Utils.API.Models
{
    public class ModelCreator : IUsingEvents
    {


        public const ItemType item = ItemType.GunCOM18;


        public MovementMode MovementMode
        {
            get
            {
                return MovementHandler.Mode;
            }
            set
            {
                MovementHandler.Mode = value;
            }
        }
        private MovementHandler MovementHandler;
        private SelectedModel SelectedModel;

        private bool mode = false;
        private const float MAX_DISTANCE = 50;


        public Model ModelSelected;


        public ModelCreator()
        {

        }

        public void SubscribeEvents()
        {

            SelectedModel = new();
            MovementHandler = new();


            MovementHandler.SubscribeEvents();
            Exiled.Events.Handlers.Player.DryfiringWeapon += OnDryfiringWeapon;
            Exiled.Events.Handlers.Player.TogglingWeaponFlashlight += OnTogglingWeaponFlashlight;
            Exiled.Events.Handlers.Player.AimingDownSight += OnAimingDownSight;
            Exiled.Events.Handlers.Server.RoundStarted += Test;

        }
        private void Test()
        {
            foreach (var p in Player.List)
            {
                p.Role.Set(PlayerRoles.RoleTypeId.ChaosConscript);
                var a = p.AddItem(item);
                var b = a as Firearm;
                b.MagazineAmmo = 2;
                
            }
        }


        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DryfiringWeapon -= OnDryfiringWeapon;
            Exiled.Events.Handlers.Player.TogglingWeaponFlashlight -= OnTogglingWeaponFlashlight;
            Exiled.Events.Handlers.Player.AimingDownSight -= OnAimingDownSight;
            Exiled.Events.Handlers.Server.RoundStarted -= Test;
            //Exiled.Events.Handlers.Player.Shooting -= OnShooting;
            MovementHandler.UnsubscribeEvents();

            SelectedModel = null;
            MovementHandler = null;
        }
        private void OnAimingDownSight(AimingDownSightEventArgs ev)
        {

        }

        private void OnDryfiringWeapon(DryfiringWeaponEventArgs ev)
        {
            if (ev.Firearm.Type != item) return;
            Log.Info("new mode = " + mode);

            if (!mode)
            {
                Primitive.Create(ev.Player.Position + Vector3.back, null, null, true, Color.red);
                Primitive.Create(ev.Player.Position + Vector3.forward, null, null, true, Color.green);
                mode = !mode;
            }



        }

        private void OnTogglingWeaponFlashlight(TogglingWeaponFlashlightEventArgs ev)
        {
            if (ev.Firearm.Type != item) return;
            


            Transform cam = ev.Player.CameraTransform;

            Vector3 origin = cam.position + cam.forward * 0.5f;
            Ray r = new Ray(origin, cam.forward);
            if (Physics.Raycast(r, out RaycastHit hit, MAX_DISTANCE))
            {
                Log.Info($"hit ({hit.collider.name})");
                PrimitiveObjectToy pot = hit.collider.GetComponentInParent<PrimitiveObjectToy>() ?? hit.collider.GetComponentInChildren<PrimitiveObjectToy>();
                if(pot != null)
                {
                    Primitive p = Primitive.Get(pot);
                    SelectedModel.ChangedSelectedPrim(p);
                }
            }

        }



    }
    public enum MovementMode
    {
        Move,
        Scale,
        Rotate
    }
}
