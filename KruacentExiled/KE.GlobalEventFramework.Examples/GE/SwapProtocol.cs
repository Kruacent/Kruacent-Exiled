using Player = Exiled.API.Features.Player;
using System.Collections.Generic;
using KE.GlobalEventFramework.GEFE.API.Features;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using MEC;
using System.Linq;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Enums;
using static UnityEngine.GraphicsBuffer;

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
                yield return Timing.WaitForSeconds(60*5);
                ChangingPlayer();
            }
        }

        private void ChangingPlayer()
        {
            // Liste des joueurs vivants
            List<Player> playerInServer = Player.List.Where(p => !p.IsNPC).ToList();

            if (playerInServer.Count < 2)
            {
                Log.Debug("Pas assez de joueurs vivants pour effectuer un échange !");
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

                var player = playerInServer[i];
                var target = playerInServer[nextIndex];

                // Récupération des objets
                List<ItemType> items1 = player.Items.Select(item => item.Type).ToList();
                List<ItemType> items2 = target.Items.Select(item => item.Type).ToList();

                // Sauvegarde et suppression des munitions
                Dictionary<AmmoType, ushort> ammo1 = new Dictionary<AmmoType, ushort>();
                Dictionary<AmmoType, ushort> ammo2 = new Dictionary<AmmoType, ushort>();

                for (int j = 0; j < player.Ammo.Count; j++)
                {
                    ammo1.Add(player.Ammo.ElementAt(j).Key.GetAmmoType(), player.Ammo.ElementAt(j).Value);
                    player.SetAmmo(ammo1.ElementAt(j).Key, 0);
                }
                for (int j = 0; j < target.Ammo.Count; j++)
                {
                    ammo2.Add(target.Ammo.ElementAt(j).Key.GetAmmoType(), target.Ammo.ElementAt(j).Value);
                    target.SetAmmo(ammo2.ElementAt(j).Key, 0);
                }

                // Changement de rôle
                player.Role.Set(playersRoles[nextIndex], PlayerRoles.RoleSpawnFlags.AssignInventory);

                // Attribution des inventaires
                target.ResetInventory(items1);
                player.ResetInventory(items2);

                // Attribution des munitions
                foreach (var ammo in ammo2)
                {
                    player.SetAmmo(ammo.Key, ammo.Value);
                }
                foreach (var ammo in ammo1)
                {
                    target.SetAmmo(ammo.Key, ammo.Value);
                }
            }
        }


    }
}