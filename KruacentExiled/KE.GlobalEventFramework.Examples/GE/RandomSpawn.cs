using Exiled.API.Features;
using KE.GlobalEventFramework.GEFE.API.Features;
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
    public class RandomSpawn : GlobalEvent
    {
        ///<inheritdoc/>
        public override uint Id { get; set; } = 1043;
        ///<inheritdoc/>
        public override string Name { get; set; } = "RandomSpawn";
        ///<inheritdoc/>
        public override string Description { get; set; } = "Les spawns sont random";
        ///<inheritdoc/>
        public override int Weight { get; set; } = 1;
        ///<inheritdoc/>
        public override IEnumerator<float> Start()
        {
            Room room = Room.Random();
            foreach (RoleTypeId r in Enum.GetValues(typeof(RoleTypeId)))
            {
                foreach (Player p in Player.List)
                {

                    if (p.Role == r)
                    {
                        p.Teleport(room);
                    }

                }
                room = Room.Random();
            }
            yield return 0;
        }

    }
}
