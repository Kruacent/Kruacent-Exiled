using AdminToys;
using Exiled.API.Extensions;
using Exiled.API.Features.Toys;
using Exiled.API.Interfaces;
using KE.Utils.API.Interfaces;
using LabApi.Features.Wrappers;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.Map.EasterEggs
{
    internal class Capybaras : IUsingEvents
    {
        private HashSet<SpinnyBaras> _spinnyBaras = new();
        public readonly Vector3 _capy1 = new Vector3(129, 293, 18);

        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Map.Generated += OnGenerated;

        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Map.Generated -= OnGenerated;
        }


        private void OnGenerated()
        {
            //e
            _spinnyBaras.Add(new SpinnyBaras(_capy1));


            var l = LabApi.Features.Wrappers.TextToy.Create(_capy1);
            l.TextFormat = "Celui qui trouve tous les easters eggs gagne 5€ Paypal";
        }
    }
}
