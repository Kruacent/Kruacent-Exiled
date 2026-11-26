using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Interfaces;
using LabApi.Events.Arguments.PlayerEvents;
using ServerAchievementFramework.API.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Achievements
{
    public class ALongWay : CounterAchievement, IUsingEvents
    {
        public override string Name => "A Long Way";
        public override string Description => "Open 1500 doors";
        public override bool IsHidden => false;
        public override uint Objective => 1500;

        public void SubscribeEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.InteractedDoor += OnInteractedDoor;
        }

        public void UnsubscribeEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.InteractedDoor -= OnInteractedDoor;
        }

        private void OnInteractedDoor(PlayerInteractedDoorEventArgs ev)
        {
            if (ev.CanOpen)
            {
                AddProgress(ev.Player);
            }
        }
    }
}
