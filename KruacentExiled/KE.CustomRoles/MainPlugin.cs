using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.CR;
using System.ComponentModel;


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

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            
            CustomRole.UnregisterRoles();

            Instance = null;
            base.OnDisabled();
        }
    }
}
