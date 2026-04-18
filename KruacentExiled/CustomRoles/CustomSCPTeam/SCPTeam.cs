using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;

namespace KruacentExiled.CustomRoles.CustomSCPTeam
{
    public static class SCPTeam
    {
        private static HashSet<ReferenceHub> _scps = new HashSet<ReferenceHub>();
        public static IReadOnlyCollection<ReferenceHub> SCPs => _scps;

        public static void AddSCP(ReferenceHub hub)
        {
            Log.Info("adding player");
            _scps.Add(hub);
        }

        public static void RemoveSCP(ReferenceHub hub)
        {
            Log.Info("removeign player");
            _scps.Remove(hub);
        }

        public static bool IsSCP(ReferenceHub hub)
        {
            return SCPs.Contains(hub);
        }
    }
}
