using AdminToys;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Utils.Extensions
{
    public static class AdminToyExtension
    {
        public static void SetFakePrimitive(this AdminToy at, Player playertoshow)
        {
            AdminToyBase atb = at.AdminToyBase;
            NetworkIdentity identity = atb.netIdentity;
            if (playertoshow == null)
            {
                identity.RemoveClientAuthority();
                return;
            }
            NetworkIdentity playerIdentity = playertoshow.NetworkIdentity;

            if (identity != null && playerIdentity != null)
            {
                // Remove authority from previous owners (if any)
                identity.RemoveClientAuthority();

                // Assign authority to the target player
                identity.AssignClientAuthority(playerIdentity.connectionToClient);
            }
        }

    }
}
