using Exiled.API.Features;
using KE.Utils.API.Interfaces;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.CR.ChaosInsurgency;
using KruacentExiled.CustomRoles.CR.ClassD;
using LabApi.Events.Arguments.PlayerEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Achievements.CustomRoles
{
    public class NakedSpeed : ServerAchievementFramework.API.Feature.Achievement, IUsingEvents
    {
        public override string Name => "Naked Speed";
        public override string Description => "As a DBoyInShape, escape without taking anything in your inventory (ammo excluded)";
        public override bool IsHidden => false;

        public HashSet<Player> Blacklisted = new HashSet<Player>();
        private KECustomRole DBoyInShape => KECustomRole.Get<DBoyInShape>();

        public void SubscribeEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.PickedUpItem += OnPickedUpItem;
            LabApi.Events.Handlers.PlayerEvents.PickedUpArmor += OnPickedUpArmor;
            LabApi.Events.Handlers.PlayerEvents.PickedUpScp330 += OnPickedUpScp330;
            LabApi.Events.Handlers.PlayerEvents.Escaped += OnEscaped;
            LabApi.Events.Handlers.PlayerEvents.Death += OnDeath;

        }

        public void UnsubscribeEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.PickedUpItem -= OnPickedUpItem;
            LabApi.Events.Handlers.PlayerEvents.PickedUpArmor -= OnPickedUpArmor;
            LabApi.Events.Handlers.PlayerEvents.PickedUpScp330 -= OnPickedUpScp330;
            LabApi.Events.Handlers.PlayerEvents.Escaped -= OnEscaped;
            LabApi.Events.Handlers.PlayerEvents.Death -= OnDeath;
        }
        private void OnDeath(PlayerDeathEventArgs ev)
        {
            Blacklisted.Remove(ev.Player);
        }

        private void OnPickedUpItem(PlayerPickedUpItemEventArgs ev)
        {
            OnPickedUp(ev.Player);
        }

        private void OnPickedUpArmor(PlayerPickedUpArmorEventArgs ev)
        {
            OnPickedUp(ev.Player);
        }
        private void OnPickedUpScp330(PlayerPickedUpScp330EventArgs ev)
        {
            OnPickedUp(ev.Player);
        }


        private void OnPickedUp(Player player)
        {
            if (DBoyInShape.Check(player))
            {
                Blacklisted.Add(player);
            }
        }

        private void OnEscaped(PlayerEscapedEventArgs ev)
        {
            Player player = ev.Player;
            if (!Blacklisted.Contains(player) && DBoyInShape.Check(player))
            {
                GiveAchievement(player);
            }


        }

    }
}
