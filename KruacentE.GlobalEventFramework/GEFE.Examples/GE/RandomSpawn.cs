using Exiled.API.Features;
using GEFExiled.GEFE.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEFExiled.GEFE.Examples.GE
{
    public class RandomSpawn : GlobalEvent
    {
        public override int Id { get; set; } = 32;
        public override string Name { get; set; } = "RandomSpawn";
        public override string Description { get; set; } = "Les spawns sont random";
        public override double Weight { get; set; } = 1;
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
