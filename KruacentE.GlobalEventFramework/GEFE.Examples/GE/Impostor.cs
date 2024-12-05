using Player = Exiled.API.Features.Player;
using GEFExiled.GEFE.API.Features;
using System.Collections.Generic;
using MEC;
using System.Linq;
using Exiled.API.Extensions;
using Exiled.API.Features;

namespace GEFExiled.GEFE.Examples.GE
{
    public class Impostor : GlobalEvent
    {
        public override int Id { get; set; } = 30;
        public override string Name { get; set; } = "Impostor";
        public override string Description { get; set; } = "Ne vous fiez pas aux apparences !";
        public override double Weight { get; set; } = 1;

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
            List<Player> playerInServer = Player.List.ToList();
            playerInServer.ShuffleList();

            //Affichage des joueurs pour le debug
            playerInServer.ForEach(x => Log.Debug("Joueur : " + x.Nickname));

            foreach (Player player in playerInServer)
            {
                Player otherPlayer = playerInServer.RandomItem();


                Log.Debug("Joueur Target : " + otherPlayer.Nickname);
                Log.Debug("Role du Target : " + otherPlayer.Role);

                player.ChangeAppearance(otherPlayer.Role);
                player.DisplayNickname = otherPlayer.Nickname;

                playerInServer.Remove(otherPlayer);
            }
        }

    }
}
