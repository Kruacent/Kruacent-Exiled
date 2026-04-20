using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Interfaces;
using KruacentExiled.GlobalEventFramework.GEFE.API.Features.Hints;
using System.Collections.Generic;
using System.Linq;

namespace KruacentExiled.GlobalEventFramework.Examples.API.Feature.CassieGoCrazy
{
    // Turns off the lights in the Heavy Containment zone.
    public class AntagonisticCassie : ICGCEffect, IUsingEvents
    {
        private Player target;
        /// <summary>
        /// Percentage for rare event to occur
        /// </summary>
        public int RareEvent { get; set; } = 2;
        private bool flag = false;

        public IReadOnlyCollection<ItemType> Reward { get; } = new HashSet<ItemType>()
        {
            ItemType.ParticleDisruptor,
            ItemType.Coin,
            ItemType.KeycardO5,
            ItemType.SCP268
        };



        public void Effect()
        {
            List<Player> nonScpPlayers = Player.List.Where(player => !player.IsScp).ToList();

            /*if (nonScpPlayers.Count > 0)
            {
                target = nonScpPlayers[UnityEngine.Random.Range(0, nonScpPlayers.Count)];

                Cassie.Message("New target : " + target.Nickname, true, true, true);

                flag = true;


            }*/
        }

        private void OnPlayerDeath(DyingEventArgs ev)
        {
            if (!flag) return;


            if (ev.Player == target && ev.Attacker != null && ev.Attacker != target)
            {
                if (!ev.Attacker.IsScp)
                {
                    GiveRandomRewardPlayer(ev.Attacker);
                }
                flag = false;
            }
        }

        private void GiveRandomRewardPlayer(Player player)
        {

            ItemType randomItem = Reward.GetRandomValue();

            player.AddItem(randomItem);

            if (UnityEngine.Random.Range(0, 100) < RareEvent)
            {
                player.MaxHealth += 25;
                DisplayHints.AddHintEffect(player, "Another gift for you!", 5);
            }
        }

        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Dying += OnPlayerDeath;
        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Dying -= OnPlayerDeath;
        }
    }
}
