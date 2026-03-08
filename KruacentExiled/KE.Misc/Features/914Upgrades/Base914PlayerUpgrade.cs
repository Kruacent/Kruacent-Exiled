using Exiled.Events.EventArgs.Scp914;
using PlayerRoles;
using Scp914;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features._914Upgrades
{
    public abstract class Base914PlayerUpgrade : Base914Upgrade
    {

        protected override abstract bool OnUpgradingPlayer(UpgradingPlayerEventArgs ev);




    }
}
