using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Misc.Features.FriendlyFireConditions
{
    internal class StartRoundFFCC : FFChangingCondition
    {
        protected override float Chance => 50;

        public override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            base.SubscribeEvents();
        }

        public override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            base.UnsubscribeEvents();
        }

        private void OnRoundStarted() 
        {
            ForceFriendlyFireState(true);
        }
    }
}
