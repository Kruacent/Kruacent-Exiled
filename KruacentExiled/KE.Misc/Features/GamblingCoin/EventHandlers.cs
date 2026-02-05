using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.Misc.Events.EventsArgs.GamblingCoinsEventArgs;
using KE.Misc.Features.GamblingCoin.Interfaces;
using KE.Misc.Features.GamblingCoin.Types;
using MEC;

namespace KE.Misc.Features.GamblingCoin
{
    public class EventHandlers
    {
        private static Config Config => MainPlugin.Instance.Config;

        private readonly Dictionary<string, DateTime> _cooldowns = new();
        public static Dictionary<ushort, int> CoinUses = new();

        public void OnCoinFlip(FlippingCoinEventArgs ev)
        {
            Player player = ev.Player;
            Item item = ev.Item;

            if (CustomItem.TryGet(item, out _)) return;


            GamblingEventArgs ev1 = new(player, item, true);

            Events.Handlers.GamblingCoins.OnGambling(ev1);

            if (!ev1.IsAllowed) return;

            if (_cooldowns.TryGetValue(player.UserId, out var lastFlip) &&
                (DateTime.UtcNow - lastFlip).TotalSeconds < Config.GamblingCoinCooldown)
            {
                ev.IsAllowed = false;
                PlayerUtils.SendBroadcast(player, "You must wait before flipping again");
                return;
            }

            _cooldowns[player.UserId] = DateTime.UtcNow;


            if (!CoinUses.ContainsKey(player.CurrentItem.Serial))
            {
                CoinUses[player.CurrentItem.Serial] = UnityEngine.Random.Range(Config.GamblingCoinMinUse, Config.GamblingCoinMaxUse);

                Log.Debug($"Registered new coin: {CoinUses[player.CurrentItem.Serial]} uses left.");
            }

            CoinUses[player.CurrentItem.Serial]--;

            int remainingUses = CoinUses[player.CurrentItem.Serial];

            bool shouldBreak = remainingUses <= 0;

            EffectType type = ev.IsTails ? EffectType.Negative : EffectType.Positive;

            ICoinEffect effect = GamblingCoinManager.GetRandomEffect(type);
            
            if (effect == null)
            {
                Log.Warn($"No {type} effect found in GamblingCoinManager!");
                player.Broadcast(5, "This coin is empty.");
                return;
            }


            effect.ExecuteEffect(player);

            if (!string.IsNullOrEmpty(effect.Message))
            {
                PlayerUtils.SendBroadcast(player, effect.Message);
            }

            if (shouldBreak)
            {
                CoinUses.Remove(ev.Player.CurrentItem.Serial);
                ev.Player.RemoveHeldItem();
                ev.Player.Broadcast(5, "no more coin");
                item = null;
            }
            GambledEventArgs ev2 = new(ev.Player, item, effect, remainingUses, shouldBreak);

            Events.Handlers.GamblingCoins.OnGambled(ev2);

            CoinUses[ev.Player.CurrentItem.Serial] = ev2.RemainingUses;
        }
    }
}