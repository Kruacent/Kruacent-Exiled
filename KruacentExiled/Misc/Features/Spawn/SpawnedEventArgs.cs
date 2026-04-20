using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization.ObjectGraphVisitors;

namespace KruacentExiled.Misc.Features.Spawn
{
    public class SpawnedEventArgs
    {


        public List<Player> VanillaSCPs { get; }
        public List<Player> CustomRolesSCPs { get; }



        public SpawnedEventArgs(List<Player> vanilla,List<Player> custom)
        {
            VanillaSCPs = vanilla;
            CustomRolesSCPs = custom;
        }

        public SpawnedEventArgs()
        {
            VanillaSCPs = new List<Player>();
            CustomRolesSCPs = new List<Player>();
        }
    }
}
