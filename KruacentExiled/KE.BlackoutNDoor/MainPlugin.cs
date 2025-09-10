using KE.BlackoutNDoor.API.Features;
using KE.BlackoutNDoor.Handlers;
using Exiled.API.Features;
using System;
using System.ComponentModel;
using Server = Exiled.Events.Handlers.Server;
using KE.BlackoutNDoor.API.Features.RoundEffects;

namespace KE.BlackoutNDoor
{
    public class MainPlugin : Plugin<Config>
    {
        public override string Author => "Patrique";
        public override Version Version => new Version(1,0,0);
        public override string Name => "KE.BlackoutDoor";
        internal static MainPlugin Instance;

        [Obsolete()]
        public ServerHandler ServerHandler { get; } = null;

        public override void OnEnabled()
        {
            Instance = this;
            this.RegisterEvent();
        }
        public override void OnDisabled()
        {

            Instance = null;
            this.UnregisterEvent();
        }

        private void RegisterEvent() 
        {
            RoundEffect.SubscribeEvents();

        }
        private void UnregisterEvent() 
        {
            RoundEffect.UnsubscribeEvents();
        }





    }
}
