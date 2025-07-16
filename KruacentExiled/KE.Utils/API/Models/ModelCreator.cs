using AdminToys;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Interfaces;
using KE.Utils.API.Models.Arrows;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Utils.API.Models
{
    public class ModelCreator : IUsingEvents
    {


        public const ItemType item = ItemType.GunCOM18;

        public Model SelectedModel
        {
            get
            {
                return ModelHandler.SelectedModel;
            }
        }
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
        internal MovementHandler MovementHandler { get; private set; }
        internal ModelSelection ModelHandler { get; private set; }

        private bool mode = false;
        private const float MAX_DISTANCE = 50;

        public static event Action<Player> IsAiming;
        public static event Action StoppedAiming;

        

        public ModelCreator()
        {

        }

        public void SubscribeEvents()
        {

            ModelHandler = new();
            MovementHandler = new();

            ModelLoader.LoadAll();
            MovementHandler.SubscribeEvents();
            Exiled.Events.Handlers.Player.DryfiringWeapon += OnDryfiringWeapon;
            Exiled.Events.Handlers.Player.TogglingWeaponFlashlight += OnTogglingWeaponFlashlight;
            Exiled.Events.Handlers.Server.RoundStarted += Test;
            Exiled.Events.Handlers.Player.AimingDownSight += OnAimingDownSight;

        }
        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DryfiringWeapon -= OnDryfiringWeapon;
            Exiled.Events.Handlers.Player.TogglingWeaponFlashlight -= OnTogglingWeaponFlashlight;
            Exiled.Events.Handlers.Player.AimingDownSight -= OnAimingDownSight;
            Exiled.Events.Handlers.Server.RoundStarted -= Test;
            MovementHandler.UnsubscribeEvents();

            ModelHandler = null;
            MovementHandler = null;
        }

        private void Test()
        {
            foreach (var p in Player.List)
            {
                p.Role.Set(PlayerRoles.RoleTypeId.ChaosConscript);
                Timing.CallDelayed(.1f, () =>
                {
                    var a = p.AddItem(item);
                    var b = a as Firearm;
                    b.MagazineAmmo = 2;

                    Primitive.Create(p.Position + Vector3.back, null, null, true, Color.red);
                    Primitive.Create(p.Position + Vector3.forward, null, null, true, Color.green);
                });
            }
        }




        private void OnAimingDownSight(AimingDownSightEventArgs ev)
        {
            if (ev.AdsIn)
            {
                IsAiming?.Invoke(ev.Player);
            }
            else
            {
                StoppedAiming?.Invoke();
            }
        }

        private void OnDryfiringWeapon(DryfiringWeaponEventArgs ev)
        {
            if (ev.Firearm.Type != item) return;
            Log.Info("new mode = " + mode);

            if (!mode)
            {
       
                mode = !mode;
            }



        }

        private void OnTogglingWeaponFlashlight(TogglingWeaponFlashlightEventArgs ev)
        {
            if (ev.Firearm.Type != item) return;



            Primitive p = GetFacingPrimitive(ev.Player);
            if (!Arrow.IsPrimitiveArrow(p))
            {
                ModelHandler.ChangedSelectedPrim(p);
            }

        }

        internal static Primitive GetFacingPrimitive(Player player)
        {
            Transform cam = player.CameraTransform;
            
            Vector3 origin = cam.position + cam.forward * 0.5f;
            Ray r = new Ray(origin, cam.forward);
            if (Physics.Raycast(r, out RaycastHit hit, MAX_DISTANCE))
            {
                Log.Info($"hit ({hit.collider.name})");
                PrimitiveObjectToy pot = hit.collider.GetComponentInParent<PrimitiveObjectToy>() ?? hit.collider.GetComponentInChildren<PrimitiveObjectToy>();
                if (pot != null)
                {
                    return Primitive.Get(pot);


                }
            }
            return null;
        }



    }
    public enum MovementMode
    {
        Move,
        Scale,
        Rotate
    }
}
