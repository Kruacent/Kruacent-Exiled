using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Interfaces;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.CR.ChaosInsurgency;
using KruacentExiled.CustomRoles.CR.ClassD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Achievements.CustomRoles
{
    internal class Pyrorca : ServerAchievementFramework.API.Feature.Achievement, IUsingEvents
    {
        public override string Name => "Pyrorca";
        public override bool IsHidden => true;
        public override string Description => "As SCP-106, teleport a kid into the pocket dimension";
        private KECustomRole Enfant => KECustomRole.Get<Enfant>();
        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.EnteringPocketDimension += OnEnteringPocketDimension;
        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.EnteringPocketDimension -= OnEnteringPocketDimension;
        }

        public void OnEnteringPocketDimension(EnteringPocketDimensionEventArgs ev)
        {
            if ( ev.IsAllowed && Enfant.Check(ev.Player))
            {
                GiveAchievement(ev.Scp106);
            }

        }
    }
}
