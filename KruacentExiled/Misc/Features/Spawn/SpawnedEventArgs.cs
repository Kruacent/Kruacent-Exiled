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


        public List<Player> VanillaRoles { get; }
        public List<Player> CustomRoles { get; }



        public SpawnedEventArgs(List<Player> vanilla,List<Player> custom)
        {
            VanillaRoles = vanilla;
            CustomRoles = custom;
        }

        public SpawnedEventArgs()
        {
            VanillaRoles = new List<Player>();
            CustomRoles = new List<Player>();
        }
    }
}
