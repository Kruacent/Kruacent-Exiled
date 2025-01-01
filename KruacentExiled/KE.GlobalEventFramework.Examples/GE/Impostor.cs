using Player = Exiled.API.Features.Player;
using System.Collections.Generic;
using KE.GlobalEventFramework.GEFE.API.Features;
using MEC;
using System.Linq;
using Exiled.API.Extensions;
using Exiled.API.Features;

namespace KE.GlobalEventFramework.Examples.GE
{
    public class Impostor : GlobalEvent
    {
        public override int Id { get; set; } = 1044;
        public override string Name { get; set; } = "Impostor";
        public override string Description { get; set; } = "Ne vous fiez pas aux apparences !";
        public override int Weight { get; set; } = 1;

        public override IEnumerator<float> Start()
        {
            while (!Round.IsEnded)
            {
                int randomNumber = UnityEngine.Random.Range(180, 300);
                yield return Timing.WaitForSeconds(randomNumber);
                ChangingPlayer();
            }
        }

        private void ChangingPlayer()
        {
            // Liste des joueurs vivants
            List<Player> playerInServer = Player.List.Where(p => !p.IsNPC && p.IsAlive).ToList();

            if (playerInServer.Count < 2)
            {
                Log.Warn("Pas assez de joueurs vivants pour effectuer un échange !");
                return;
            }

            // Debug : afficher la liste initiale des joueurs avec leurs rôles
            Log.Debug("===== Liste des joueurs avant permutation =====");

            playerInServer.ForEach(p => Log.Debug($"{p.Nickname} ({p.Role})"));

            // Mélanger la liste des joueurs
            playerInServer.ShuffleList();

            // Copier les données actuelles des joueurs
            var originalNicknames = playerInServer.Select(p => p.Nickname).ToList();
            var originalRoles = playerInServer.Select(p => p.Role).ToList();

            // Permutation circulaire des rôles et pseudonymes
            for (int i = 0; i < playerInServer.Count; i++)
            {
                int nextIndex = (i + 1) % playerInServer.Count;
                playerInServer[i].ChangeAppearance(originalRoles[nextIndex]);
                playerInServer[i].DisplayNickname = originalNicknames[nextIndex];
            }

            // Debug : afficher les correspondances après permutation
            Log.Debug("===== Correspondances après permutation =====");
            for (int i = 0; i < playerInServer.Count; i++)
            {
                int nextIndex = (i + 1) % playerInServer.Count;
                Log.Debug($"{originalNicknames[i]} ({originalRoles[i]}) -> {originalNicknames[nextIndex]} ({originalRoles[nextIndex]})");
            }
        }


    }
}