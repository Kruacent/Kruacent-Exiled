using Player = Exiled.API.Features.Player;
using System.Collections.Generic;
using KE.GlobalEventFramework.GEFE.API.Features;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using MEC;
using System.Linq;
using Exiled.API.Extensions;
using Exiled.API.Features;

namespace KE.GlobalEventFramework.Examples.GE
{
    public class SwapProtocol : GlobalEvent, IStart
    {
        public override uint Id { get; set; } = 1051;
        public override string Name { get; set; } = "SwapProtocol";
        public override string Description { get; set; } = "EEEEEEEET c'est parti la roulette tourne";
        public override int Weight { get; set; } = 1;

        public IEnumerator<float> Start()
        {
            while (!Round.IsEnded)
            {
                // every 5 min
                yield return Timing.WaitForSeconds(300);
                ChangingPlayer();
            }
        }

        private void ChangingPlayer()
        {
            // Liste des joueurs vivants
            List<Player> playerInServer = Player.List.Where(p => !p.IsNPC).ToList();

            if (playerInServer.Count < 2)
            {
                Log.Warn("Pas assez de joueurs vivants pour effectuer un échange !");
                return;
            }

            Log.Debug("===== Liste des joueurs avant permutation =====");
            playerInServer.ForEach(p => Log.Debug($"{p.Nickname} ({p.Role})"));

            // Mélanger la liste des joueurs
            playerInServer.ShuffleList();

            // Copier les données actuelles des joueurs
            var playersRoles = playerInServer.Select(p => p.Role).ToList();

            // Permutation circulaire des rôles et pseudonymes
            for (int i = 0; i < playerInServer.Count; i++)
            {
                int nextIndex = (i + 1) % playerInServer.Count;
                playerInServer[i].Role.Set(playersRoles[nextIndex]);
            }
        }


    }
}