using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Misc.Features.Spawn.DummySCPs
{
    public class Dummy096: DummySCP
    {

        public override string SCPId => "SCP-096";

        public override bool IsSupport => true;

        public override RoleTypeId Role => RoleTypeId.Scp096;
    }
}
