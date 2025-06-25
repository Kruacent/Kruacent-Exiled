using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using KE.GlobalEventFramework.GEFE.API.Features;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.GlobalEventFramework.Examples.GE
{
    /// <summary>
    /// All spawn are random at the start of the game (NTF & Chaos not included)
    /// Note: all role spawn with each other except SCPs
    /// </summary>
    public class RandomSpawn : GlobalEvent,IStart
    {
        ///<inheritdoc/>
        public override uint Id { get; set; } = 1043;
        ///<inheritdoc/>
        public override string Name { get; set; } = "RandomSpawn";
        ///<inheritdoc/>
        public override string Description { get; set; } = "Les spawns sont random";
        ///<inheritdoc/>
        public override int WeightedChance { get; set; } = 1;
        public IEnumerable<RoomType> BlacklistedRooms { get; } = new HashSet<RoomType>() { RoomType.EzShelter,RoomType.HczTestRoom,RoomType.EzCollapsedTunnel};
        ///<inheritdoc/>
        public IEnumerator<float> Start()
        {
            Room room;
            foreach (RoleTypeId r in Enum.GetValues(typeof(RoleTypeId)))
            {
                room = Room.List.GetRandomValue(r => !BlacklistedRooms.Contains(r.Type));
                foreach (Player p in Player.List)
                {

                    if (p.Role == r)
                    {
                        p.Teleport(room);
                    }

                }
                
            }
            yield return 0;
        }

    }
}
