using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Roles;
using KE.Misc.Features.Auto079.Jobs;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Camera = Exiled.API.Features.Camera;
using NPCFeature = Exiled.API.Features.Npc;
namespace KE.Misc.Features.Auto079
{
    public class NPC079
    {
        public Npc Npc;
        //the player that 079 will try to follow
        public Player targetPlayer;

        public Player Scp;

        private JobManager JobManager;
        public Scp079Role Role => Npc.Role.As<Scp079Role>();

        public Dictionary<Player, List<Item>> InventoryGuess = new();
        public NPC079()
        {
            Npc = Npc.Spawn("SCP-079-AI", RoleTypeId.Scp079);
            Scp = Player.List.Where(p => p.IsScp && p.Role != RoleTypeId.Scp0492).FirstOrDefault();
            if (Scp is null) return;

            JobManager = new(this);
            Scp079Role role = Npc.Role.As<Scp079Role>();


            Timing.CallDelayed(NPCFeature.SpawnSetRoleDelay, JobManager.StartLoop);
            

        }


        public bool TryPing(Vector3 position, PingType pingType = PingType.Default)
        {
            if (!Role.PingAbility.IsReady)
            {
                return false;
            }

            Role.Ping(position, pingType,true);
            return true;

        }


        public bool MoveCamToScp()
        {
            Camera camera = Scp.CurrentRoom.Cameras.GetRandomValue();

            int cost = Role.GetSwitchCost(camera);

            
            if(cost > Role.Energy)
            {
                return false;
            }

            Role.Energy -= cost;
            Role.Camera = camera;
            return true;
        }



    }
}
