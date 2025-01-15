using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;


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
        }

        /// <inheritdoc/>
        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= CustomRoleImplement;
        }

        public void CustomRoleImplement()
        {
            Controller.controller.CustomRoleGiver();
        }
    }
}
