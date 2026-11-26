using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Interfaces;
using KruacentExiled.CustomItems.Events.CustomItem.EventArgs;
using KruacentExiled.CustomItems.Items;
using LabApi.Events.Arguments.PlayerEvents;
using MEC;
using ServerAchievementFramework.API.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Achievements
{
    public class Gachis : Achievement, IUsingEvents
    {
        public override string Name => "Unoptimal resource management";
        public override string Description => "Dying ";
        public override bool IsHidden => false;

        public const float Time = 10;
        public void SubscribeEvents()
        {
            Defibrillator.RevivedByDefibrilator += OnRevivedByDefibrilator;
        }       

        public void UnsubscribeEvents()
        {
            Defibrillator.RevivedByDefibrilator -= OnRevivedByDefibrilator;
        }
        private void OnRevivedByDefibrilator(RevivedByDefibrilatorArgs ev)
        {
            Player player = ev.Player;

            Timing.CallDelayed(Time, () =>
            {
                if(player != null && player.ReferenceHub != null && !player.IsAlive)
                {
                    GiveAchievement(player);
                }
            });
        }

        
    }
}
