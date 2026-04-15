using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.CustomItems.API.Features;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using KE.Items.API.Features;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Features;
using KE.Utils.API.Features.SCPs;
using KE.Utils.API.GifAnimator;
using LiteNetLib4Mirror.Open.Nat;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KE.CustomRoles.Abilities
{
    public class Thief : KEAbilities, ICustomIcon
    {
        public override string Name { get; } = "Thief";

        public const string NoPlayer = "ThiefNoPlayer";
        public const string Fail = "ThiefFail";
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Steal",
                    [TranslationKeyDesc] = "Steal a random item from a player in the same room",
                    [NoPlayer] = "No player to steal from",
                    [Fail] = "I think this is a skill issue! Congrats!",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Voler",
                    [TranslationKeyDesc] = "Vole un object aléatoire à un joueur dans la même pièce",
                    [NoPlayer] = "Personne à qui voler",
                    [Fail] = "Wow faut get good là",
                }
            };
        }

        public override float Cooldown { get; } = 60f;

        public TextImage IconName => MainPlugin.Instance.icons[Name];

        protected override bool AbilityUsed(Player player)
        {

            IEnumerable<Player> sameRoom = Player.Enumerable.Where(p => p.CurrentRoom == player.CurrentRoom && !p.IsInventoryEmpty && p != player);



            KELog.Debug("scp");
            Player scp = GetClosest(sameRoom.Where(p => SCPTeam.IsSCP(p.ReferenceHub)), player.Position);


            if(scp != null)
            {
                KELog.Debug("steal scp");
                Steal(player, scp);
                return true;
            }

            KELog.Debug("enemy");
            Player enemy = GetClosest(sameRoom.Where(p => HitboxIdentity.IsEnemy(player.ReferenceHub, p.ReferenceHub)),player.Position);


            if(enemy != null)
            {
                KELog.Debug("steal enemy");
                Steal(player, enemy);
                return true;
            }

            KELog.Debug("other");
            Player ally = GetClosest(sameRoom, player.Position);


            if (ally != null)
            {
                KELog.Debug("steal ally");
                Steal(player, ally);
                return true;
            }

            MainPlugin.ShowEffectHint(player, "no player to steal from");
            return false;
        }

        private void Steal(Player player, Player thiefed)
        {

            Item item;

            if(player.CurrentItem is null || UnityEngine.Random.Range(0f,100f) < 50f)
            {
                item = thiefed.Items.GetRandomValue();
            }
            else
            {
                item = player.CurrentItem;
            }


            if(KECustomItem.TryGet(item,out CustomItem ci))
            {
                ci.Give(player);
            }
            else
            {
                Item newitem = item.Clone();
                newitem.Give(player);
            }
            
            thiefed.RemoveItem(item);
        }




        private Player GetClosest(IEnumerable<Player> players,Vector3 center)
        {
            return players.OrderBy(p => Vector3.Distance(center, p.Position)).FirstOrDefault();
        }
    
    
    
    }
}
