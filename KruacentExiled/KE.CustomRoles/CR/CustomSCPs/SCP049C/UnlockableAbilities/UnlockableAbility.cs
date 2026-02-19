using KE.CustomRoles.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.CR.CustomSCPs.SCP049C.UnlockAbleAbilities
{
    public abstract class UnlockableAbility
    {

        public abstract byte Tier { get; }


        public abstract void Grant(ReferenceHub hub);


        public abstract void Remove(ReferenceHub hub);

    }
}
