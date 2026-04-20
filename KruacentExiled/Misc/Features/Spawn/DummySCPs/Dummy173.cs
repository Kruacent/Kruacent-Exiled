using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Misc.Features.Spawn.DummySCPs
{
    public class Dummy173: DummySCP
    {

        public override string SCPId => "SCP-173";

        public override bool IsSupport => false;
        public override RoleTypeId Role => RoleTypeId.Scp173;

    }
}
