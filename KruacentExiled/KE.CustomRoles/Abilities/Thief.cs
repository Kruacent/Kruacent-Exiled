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

            Player thiefed = Player.Enumerable.GetRandomValue(p => !p.IsScp && p.CurrentRoom == player.CurrentRoom && p != player);
            if(thiefed is null)
            {
                MainPlugin.ShowEffectHint(player, "no player to steal from");
                return false;
            }

            Log.Debug($"Thiefed player : {thiefed.Nickname}");

            Item item = thiefed.Items.GetRandomValue();


            if (item == null)
            {
                MainPlugin.ShowEffectHint(player, "I think this is a skill issue ! Congrats !");
                return true;
            }



            Item newitem = item.Clone();
            newitem.Give(player);
            thiefed.RemoveItem(item);

            return base.AbilityUsed(player);
        }
    }
}
