using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.GifAnimator;
using System.Collections.Generic;
using System.Linq;

namespace KE.CustomRoles.Abilities
{
    public class Thief : KEAbilities, ICustomIcon
    {
        public override string Name { get; } = "Thief";


        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Thief",
                    [TranslationKeyDesc] = "Steal a random item from a player in the same room",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Voler",
                    [TranslationKeyDesc] = "Vole un object aléatoire à un joueur dans la même pièce",
                }
            };
        }

        public override float Cooldown { get; } = 120f;

        public TextImage IconName => MainPlugin.Instance.icons[Name];

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
