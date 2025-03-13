using Exiled.CustomRoles.API.Features;
using KE.Utils.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.API
{
    public abstract class KECustomRole : CustomRole
    {
        public override bool IgnoreSpawnSystem { get; set; } = true;
        protected override void ShowMessage(Exiled.API.Features.Player player)
        {
            DisplayPlayer.Get(player).Hint((float)HintPlacement.CustomItem, "<align=left>"+ string.Format(Exiled.CustomRoles.CustomRoles.Instance.Config.GotRoleHint.Content+ "</align>", Name, Description), Exiled.CustomRoles.CustomRoles.Instance.Config.GotRoleHint.Duration);

        }
    }
}
