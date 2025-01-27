using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Server;


namespace KE.CustomRoles
{
    public class MainPlugin : Plugin<Config>
    {
        public override string Name { get; } = "KE.CustomRoles";
        public static MainPlugin Instance;

        public override void OnEnabled()
        {
            
            Instance = this;

            CustomRole.RegisterRoles(false,null);
            this.SubscribeEvents();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            
            CustomRole.UnregisterRoles();

            Instance = null;
            this.UnsubscribeEvents();

            base.OnDisabled();
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
            Controller.controller.GiveRole(Player.List);
        }

        public void CustomRoleRespawning(RespawnedTeamEventArgs ev)
        {
            Controller.controller.GiveRole(ev.Players);
        }
    }
}
