using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using Utils.NonAllocLINQ;

namespace KE.CustomRoles
{
    internal class Controller
    {
        public const int Chance = 40;
        public static Controller controller = new Controller();

        private Controller() { }

        internal void CustomRoleGiver()
        {
            CustomRole.Registered.ForEach(x => Log.Debug("- " + x.ToString()));

            foreach (Player player in Exiled.API.Features.Player.List)
            {
                if (UnityEngine.Random.Range(0,100) < Chance)
                {
                    CustomRole cr;

                    do
                    {
                        cr = CustomRole.Registered.GetRandomValue();
                    }
                    while (cr.Role != player.Role);

                    cr.AddRole(player);
                }
            }
        }

    }
}