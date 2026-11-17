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
            _scps.Add(hub);
            Log("adding player, new count : "+SCPs.Count);
        }

        public static void RemoveSCP(ReferenceHub hub)
        {
            _scps.Remove(hub);
            Log("removing player, new count : " + SCPs.Count);
        }

        public static bool IsSCP(ReferenceHub hub)
        {
            return SCPs.Contains(hub);
        }


        private static void Log(string msg)
        {
            Exiled.API.Features.Log.SendRaw($"[SCPTeam] " + msg,System.ConsoleColor.DarkYellow);
        }
    }
}
