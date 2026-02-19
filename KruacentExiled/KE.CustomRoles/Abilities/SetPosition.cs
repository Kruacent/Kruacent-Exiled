using Exiled.API.Features;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using KE.Utils.API.KETextToy;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.Abilities
{
    public class SetPosition : KEAbilities, ICustomIcon
    {
        public override string Name { get; } = "SetPosition";
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Set Position",
                    [TranslationKeyDesc] = "Select the current position for another ability",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Selection de position",
                    [TranslationKeyDesc] = "Selectionne la position pour une autre abilité",
                }
            };
        }
        
        public override float Cooldown { get; } = 5f;

        private static Dictionary<Player, Vector3> SelectedTarget = new();
        private static Dictionary<Player, FollowingTextToy> SelectedTextToys = new();

        public Utils.API.GifAnimator.TextImage IconName => MainPlugin.Instance.icons[Name];

        protected override bool AbilityUsed(Player player)
        {

            Vector3 position = player.Position;


            SelectedTarget[player] = position;


            if (SelectedTextToys.ContainsKey(player))
            {
                SelectedTextToys[player].Destroy();
            }



            SelectedTextToys[player] = new FollowingTextToy([player], position, Quaternion.identity, Vector3.one);

            FollowingTextToy followingTextToy = SelectedTextToys[player];



            followingTextToy.OnlyMoveY = true;
            followingTextToy.Toy.TextFormat = "↓";


            Log.Debug("set position at " + position);
            return base.AbilityUsed(player);

        }

        public static bool TryGetTarget(Player p, out Vector3 target)
        {
            return SelectedTarget.TryGetValue(p, out target);
        }


    }
}
