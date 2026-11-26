using Exiled.API.Features;
using KE.Utils.API.Interfaces;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.CR.ClassD;
using KruacentExiled.CustomRoles.CR.Scientist;
using KruacentExiled.CustomRoles.Events.EventArgs.ZoneManager;
using LabApi.Events.Arguments.PlayerEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Achievements.CustomRoles
{
    public class Employed : ServerAchievementFramework.API.Feature.Achievement, IUsingEvents
    {
        public override string Name => "The most employed of them all";
        public override string Description => "As a Zone Manager, open all checkpoints and get your well deserved keycard";
        public override bool IsHidden => false;


        public void SubscribeEvents()
        {
            ZoneManager.UpgradingKeycardZoneManager += OnUpgradingKeycardZoneManager;

        }

        public void UnsubscribeEvents()
        {
            ZoneManager.UpgradingKeycardZoneManager -= OnUpgradingKeycardZoneManager;
        }

        private void OnUpgradingKeycardZoneManager(UpgradedKeycardZoneManagerEventArgs ev)
        {
            GiveAchievement(ev.Player);
        }

    }
}
