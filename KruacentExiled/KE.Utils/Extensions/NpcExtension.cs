using CentralAuth;
using Exiled.API.Features;
using Mirror;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Utils.Extensions
{
    public static class NpcExtension
    {


        public static void Hide(this Npc npc)
        {
            npc.ReferenceHub.authManager.NetworkSyncedUserId = "ID_Dedicated";
            npc.ReferenceHub.authManager.syncMode = (SyncMode)ClientInstanceMode.DedicatedServer;
        }
    }
}
