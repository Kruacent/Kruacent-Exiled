using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Warhead;
using KE.Utils.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features.FriendlyFireConditions
{
    internal class WarheadStartingFFCC : FFChangingCondition
    {
        protected override float Chance => 50;

        public override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Warhead.Starting += OnStarting;
            base.SubscribeEvents();
        }

        public override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Warhead.Starting -= OnStarting;
            base.UnsubscribeEvents();
        }

        private void OnStarting(StartingEventArgs ev) 
        {
            ForceFriendlyFireState(true);
        }
    }
}
