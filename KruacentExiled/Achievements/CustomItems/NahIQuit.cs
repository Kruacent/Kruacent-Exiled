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
    internal class NahIQuit : ServerAchievementFramework.API.Feature.Achievement, IUsingEvents
    {
        public override string Name => "Nahh I quit";

        public override string Description => "Use the divine pills while being the last human alive and die because of it";

        public void SubscribeEvents()
        {
            DivinePillsEffect.UsedDivinePillsEffect += OnUsedDivinePillsEffect;
        }

        public void UnsubscribeEvents()
        {
            DivinePillsEffect.UsedDivinePillsEffect -= OnUsedDivinePillsEffect;
        }


        private void OnUsedDivinePillsEffect(UsedDivinePillsEffectArgs ev)
        {
            int num = Player.Enumerable.Count(p => !SCPTeam.IsSCP(p.ReferenceHub) && p.Role.Type != PlayerRoles.RoleTypeId.Spectator);
            KELog.Debug("number human: " + num);
            if(ev.Death && num == 0)
            {
                GiveAchievement(ev.Player);
            }            
        }
    }
}
