using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Utils.API.Models
{
    public class ModelCreator : IUsingEvents
    {
        public const ItemType item = ItemType.GunShotgun;
        private static ModelCreator _inst;
        public static ModelCreator Instance
        {
            get
            {
                if (_inst == null)
                    _inst = new();
                return _inst;
            }
        }


        private bool mode = false;

        private Primitive _primitiveSelected;


        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DryfiringWeapon -= OnDryfiringWeapon;
            Exiled.Events.Handlers.Player.TogglingWeaponFlashlight += OnTogglingWeaponFlashlight;
        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DryfiringWeapon -= OnDryfiringWeapon;
            Exiled.Events.Handlers.Player.TogglingWeaponFlashlight -= OnTogglingWeaponFlashlight;
        }

        private void OnDryfiringWeapon(DryfiringWeaponEventArgs ev)
        {
            if (ev.Firearm.Type != item) return;
            



        }

        private void OnTogglingWeaponFlashlight(TogglingWeaponFlashlightEventArgs ev)
        {
            if (ev.Firearm.Type != item) return;

            mode = !mode;

        }


    }
}
