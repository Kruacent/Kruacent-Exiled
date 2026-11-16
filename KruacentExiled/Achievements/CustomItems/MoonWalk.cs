using Exiled.API.Features;
using KE.Utils.API.Features;
using KE.Utils.API.Interfaces;
using KruacentExiled.CustomItems.Events.CustomItem.EventArgs;
using KruacentExiled.CustomItems.Items.ItemEffects;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Achievements.CustomItems
{
    internal class MoonWalk : ServerAchievementFramework.API.Feature.Achievement
    {
        public override string Name => "Moon Walk";

        public override string Description => "Apply the low gravity grenade effect to all players";

        public static MoonWalk Instance { get; private set; }
        public MoonWalk()
        {
            Instance = this;
        }


        internal static void GiveAchievementStatic(Player player)
        {
            Instance.GiveAchievement(player);
        }
    }
}
