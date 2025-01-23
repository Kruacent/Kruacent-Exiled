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
            Controller.controller.CustomRoleGiver();

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

        /// <inheritdoc/>
        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= CustomRoleImplement;
            Exiled.Events.Handlers.Server.RespawnedTeam -= CustomRoleRespawning;

        }

        public void CustomRoleImplement()
        {
            Controller.controller.CustomRoleGiver();
        }

        public void CustomRoleRespawning(RespawnedTeamEventArgs _)
        {
            Controller.controller.CustomRoleGiver();
        }
    }
}
