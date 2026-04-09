using Exiled.API.Features;
using Exiled.API.Interfaces;
using InteractableToy = LabApi.Features.Wrappers.InteractableToy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Mirror;

namespace KE.Items.API.Features.Complexes
{
    public class ComplexGatling : ComplexBase
    {
        protected override void OnShoot(Player player)
        {
            player.ShowHitMarker(Hitmarker.MaxSize);
        }
    }
}
