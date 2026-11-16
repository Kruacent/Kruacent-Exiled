using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Interfaces;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.CR.Scientist;
using KruacentExiled.CustomRoles.Events.EventArgs.ZoneManager;
using KruacentExiled.Misc.Events.EventsArgs.GamblingCoinsEventArgs;
using KruacentExiled.Misc.Events.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Achievements.CustomRoles
{
    public class StrongerThanTheCasino : ServerAchievementFramework.API.Feature.Achievement, IUsingEvents
    {
        public override string Name => "Stronger than the casino";
        public override string Description => "As a Gamble Addict, flip 15 coins without dying";
        public override bool IsHidden => false;

        public KECustomRole GambleAddict = KECustomRole.Get<GambleAddict>();

        private Dictionary<Player, int> _numberFlips;
        public void SubscribeEvents()
        {
            _numberFlips = new Dictionary<Player, int>();
            GamblingCoins.Gambled += OnGambled;
            Exiled.Events.Handlers.Player.Died += OnDied;
        }

        public void UnsubscribeEvents()
        {
            GamblingCoins.Gambled -= OnGambled;
            Exiled.Events.Handlers.Player.Died -= OnDied;
            _numberFlips = null;
        }

        private void OnGambled(GambledEventArgs ev)
        {
            Player player = ev.Player;
            if (GambleAddict.Check(player))
            {
                if (!_numberFlips.ContainsKey(player))
                {
                    _numberFlips[player] = 1;
                }
                else
                {
                    _numberFlips[player]++;
                }

                if (_numberFlips[player] >= 15)
                {
                    GiveAchievement(player);
                }
            }
        }


        private void OnDied(DiedEventArgs ev)
        {
            Player player = ev.Player;
            if (_numberFlips.ContainsKey(player))
            {
                _numberFlips.Remove(player);
            }
        }

    }
}
