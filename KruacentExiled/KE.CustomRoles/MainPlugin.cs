using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Server;
using KE.Utils.API.Displays.DisplayMeow;
using MEC;
using System;


namespace KE.CustomRoles
{
    public class MainPlugin : Plugin<Config>
    {
        public override string Name { get; } = "KE.CustomRoles";
        public override string Author => "Patrique & OmerGS";
        public override Version Version => new(1, 1, 0);
        public static MainPlugin Instance;
        private Controller _controller;
        public static readonly HintPlacement CRHint = new(0, 750);
        public static readonly HintPlacement CREffect = new(700, 300);

        public override void OnEnabled()
        {
            
            Instance = this;
            _controller = new Controller();

            CustomRole.RegisterRoles(false,null,true,Assembly);
            this.SubscribeEvents();

        }

        public override void OnDisabled()
        {
            
            CustomRole.UnregisterRoles();
           


            this.UnsubscribeEvents();
            _controller = null;
            Instance = null;
        }

        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted += CustomRoleImplement;
            Exiled.Events.Handlers.Server.RespawnedTeam += CustomRoleRespawning;
        }
        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= CustomRoleImplement;
            Exiled.Events.Handlers.Server.RespawnedTeam -= CustomRoleRespawning;

        }

        public void CustomRoleImplement()
        {
            _controller.GiveRole(Player.List);
            
        }

        public void CustomRoleRespawning(RespawnedTeamEventArgs ev)
        {
            _controller.GiveRole(ev.Players);
        }
    }
}
