using Exiled.API.Features;
using KruacentExiled.CustomRoles.API.Interfaces;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KruacentExiled.Misc.Features.Spawn.DummySCPs
{
    /// <summary>
    /// only here to make compatible the vanilla scp and the custom one with the spawn system
    /// </summary>
    public abstract class DummySCP : ISCPPreferences
    {
        public abstract string SCPId { get; }

        public abstract bool IsSupport { get; }

        public abstract RoleTypeId Role { get; }

        public void Set(Player player)
        {
            player.Role.Set(Role);
        }
        

        public int GetPreferences(Player player)
        {
            if (player.IsNPC)
            {
                return 0;
            }

            return player.ScpPreferences.Preferences[Role];
        }

    }
}
