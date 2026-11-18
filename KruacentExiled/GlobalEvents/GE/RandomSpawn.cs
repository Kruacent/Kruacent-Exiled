using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using KE.Utils.Extensions;
using GlobalEventFramework.API.Enums;
using GlobalEventFramework.API.Features;
using GlobalEventFramework.API.Interfaces;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KruacentExiled.GlobalEvents.GE
{
    /// <summary>
    /// All spawn are random at the start of the game (NTF & Chaos not included)
    /// Note: all role spawn with each other except SCPs
    /// </summary>
    public class RandomSpawn : GlobalEvent, IStart
    {
        ///<inheritdoc/>
        public override string Name { get; set; } = "RandomSpawn";
        ///<inheritdoc/>
        public override string Description { get; } = "Les spawns sont random";
        ///<inheritdoc/>
        public override int DefaultWeightedChance { get; } = 5;
        public override ImpactLevel ImpactLevel => ImpactLevel.High;
        public IEnumerable<RoomType> BlacklistedRooms { get; } = new List<RoomType>() { RoomType.EzGateA, RoomType.EzGateB };
        ///<inheritdoc/>
        public void Start()
        {
            Room room;
            foreach (RoleTypeId r in Enum.GetValues(typeof(RoleTypeId)))
            {
                room = Room.List.GetRandomValue(r => !BlacklistedRooms.Contains(r.Type));
                foreach (Player p in Player.List)
                {

                    if (p.Role == r)
                    {
                        p.Teleport(room.GetValidPosition());
                    }

                }
                
            }
        }

    }
}
