using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using KE.CustomRoles.API.Features;
using KE.Utils.API.Displays.DisplayMeow;
using System.Collections.Generic;
using System.Linq;

namespace KE.CustomRoles.Abilities
{
    public class Thief : KEAbilities
    {
        public override string Name { get; } = "Thief";
        public override string PublicName { get; } = "Thief";

        public override string Description { get; } = "Avec 1548 heures de jeu sur Thief Simulator fallait s'y attendre un peu.";


        public override float Cooldown { get; } = 120f;

        protected override bool AbilityUsed(Player player)
        {
            List<Player> playerList = Player.Enumerable.Where(p => !p.IsScp && p.CurrentRoom == player.CurrentRoom).ToList();
            playerList.Remove(player);

            Log.Debug("Player list :");
            playerList.ForEach(p => Log.Debug(p.Nickname));

            Player thiefed = playerList.GetRandomValue();
            if(thiefed is null)
            {
                MainPlugin.ShowEffectHint(player, "no player to steal from");
                return false;
            }

            Log.Debug($"Thiefed player : {thiefed.Nickname}");

            Item inv = thiefed.Items.GetRandomValue();

            Log.Debug($"Thiefed item : {inv}");

            if (inv == null)
            {
                Log.Debug("No item to thiefed, null, returning.");
                MainPlugin.ShowEffectHint(player, "I think this is a skill issue ! Congrats !");
                return true;
            }

            var thiefBool = thiefed.RemoveItem(inv);
            Log.Debug($"Item deleted {thiefBool}.");

            inv.Give(player);
            Log.Debug($"Item given to {player.Nickname}.");
            return base.AbilityUsed(player);
        }
    }
}
