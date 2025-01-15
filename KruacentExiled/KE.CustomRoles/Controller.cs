using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;

namespace KE.CustomRoles
{
    internal class Controller
    {
        public static Controller controller = new Controller();

        private Controller() 
        {
            
        }

        internal void CustomRoleGiver()
        {
            foreach (Player player in Exiled.API.Features.Player.List)
            {
                CustomRole cr = CustomRole.Registered.GetRandomValue();

                cr.AddRole(player);
            }
        }
    }
}