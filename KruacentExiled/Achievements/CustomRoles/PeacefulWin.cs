using Achievements;
using Exiled.API.Features;
using KE.Utils.API.Interfaces;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.CR.ChaosInsurgency;
using LabApi.Events.Arguments.PlayerEvents;
using System.Collections.Generic;

namespace KruacentExiled.Achievements.CustomRoles
{
    public class PeacefulWin : ServerAchievementFramework.API.Feature.Achievement,IUsingEvents
    {
        public override string Name => "Peaceful Win";
        public override string Description => "As a Negotiator, win a game without dealing damage or firing a weapon (except the FriendMaker)";
        public override bool IsHidden => false;

        public HashSet<Player> Blacklisted = new HashSet<Player>();
        private KECustomRole Negotiator => KECustomRole.Get<Negotiator>();

        public void SubscribeEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.Hurt += OnHurt;
            LabApi.Events.Handlers.PlayerEvents.ShotWeapon += OnShotWeapon;
            LabApi.Events.Handlers.ServerEvents.RoundEnded += OnRoundEnded;
            LabApi.Events.Handlers.PlayerEvents.Death += OnDeath;

        }

        public void UnsubscribeEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.Hurt -= OnHurt;
            LabApi.Events.Handlers.PlayerEvents.ShotWeapon -= OnShotWeapon;
            LabApi.Events.Handlers.ServerEvents.RoundEnded -= OnRoundEnded;
            LabApi.Events.Handlers.PlayerEvents.Death -= OnDeath;
        }
        private void OnDeath(PlayerDeathEventArgs ev)
        {
            Blacklisted.Remove(ev.Player);
        }
        private void OnHurt(PlayerHurtEventArgs ev)
        {
            if (ev.Player.ReferenceHub == null) return;

            if (Negotiator.Check(ev.Attacker))
            {
                Blacklisted.Add(ev.Attacker);
            }
        }
        private void OnShotWeapon(PlayerShotWeaponEventArgs ev)
        {
            if (Negotiator.Check(ev.Player))
            {
                Blacklisted.Add(ev.Player);
            }
        }

        private void OnRoundEnded(LabApi.Events.Arguments.ServerEvents.RoundEndedEventArgs ev)
        {
            foreach(Player player in Player.List)
            {
                if(!Blacklisted.Contains(player) && Negotiator.Check(player))
                {
                    GiveAchievement(player);
                }
            }

            
        }

    }
}
