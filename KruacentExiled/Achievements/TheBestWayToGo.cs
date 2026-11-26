using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Interfaces;
using KruacentExiled.Map.Heavy;
using LabApi.Events.Arguments.PlayerEvents;
using ServerAchievementFramework.API.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.Achievements
{
    public class TheBestWayToGo : Achievement, IUsingEvents
    {
        public override string Name => "The Best Way To Go";
        public override string Description => "Die by the best door on the server";
        public override bool IsHidden => false;
        private Bounds bound;
        public void SubscribeEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.Death += OnDeath;
            
        }

        public void UnsubscribeEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.Death -= OnDeath;
        }

        private void OnDeath(PlayerDeathEventArgs ev)
        {
            DamageType damageType = DamageTypeExtensions.GetDamageType(ev.DamageHandler);
            //BulkDoor049.DoorPosition;

            if (bound == default)
            {
                bound = new Bounds(BulkDoor049.DoorPosition, Vector3.one * 5);
            }

            if (damageType == DamageType.Crushed && bound.Contains(ev.OldPosition))
            {
                GiveAchievement(ev.Player);
            }
        }
    }
}
