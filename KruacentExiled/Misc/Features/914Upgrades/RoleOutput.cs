using PlayerRoles;
using Scp914;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Misc.Features._914Upgrades
{
    public struct RoleOutput
    {

        public RoleTypeId role;
        public float chance;


        public RoleOutput(RoleTypeId role, float chance)
        {
            this.chance = Mathf.Clamp(chance, 0f, 100f);
            this.role = role;
        }

    }
}
