using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Warhead;
using KE.Utils.API.Interfaces;

namespace KE.Misc.Misc
{
    internal class FriendlyFire : MiscFeature
    {
        public byte ChanceAtStart = 50;
        public byte ChanceAtScpDeath = 20;
        public byte ChanceAtWarheadStart = 100;



        public override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Dying += OnDying;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Warhead.Starting += OnStarting;
        }

        public override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Warhead.Starting -= OnStarting;

        }

        public void OnStarting(StartingEventArgs ev)
        {
            if (UnityEngine.Random.Range(0, 101) >= ChanceAtScpDeath) return;
            Server.FriendlyFire = !Server.FriendlyFire;
        }


        private void OnRoundStarted()
        {
            Server.FriendlyFire = UnityEngine.Random.Range(0, 101) < ChanceAtStart;
        }



        private void OnDying(DyingEventArgs ev)
        {
            if (!ev.Player.IsScp) return;
            if (UnityEngine.Random.Range(0, 101) >= ChanceAtScpDeath) return;
            Server.FriendlyFire = !Server.FriendlyFire;


        }






    }
}
