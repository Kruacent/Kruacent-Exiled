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


            GamblingEventArgs ev1 = new(ev.Player, item, true);

            Events.Handlers.GamblingCoins.OnGambling(ev1);

            if (!ev1.IsAllowed) return;

            if (_cooldowns.TryGetValue(ev.Player.UserId, out var lastFlip) &&
                (DateTime.UtcNow - lastFlip).TotalSeconds < Config.GamblingCoinCooldown)
            {
                ev.IsAllowed = false;
                PlayerUtils.SendBroadcast(ev.Player, "You must wait before flipping again");
                return;
            }

            _cooldowns[ev.Player.UserId] = DateTime.UtcNow;


            if (!CoinUses.ContainsKey(ev.Player.CurrentItem.Serial))
            {
                CoinUses[ev.Player.CurrentItem.Serial] = UnityEngine.Random.Range(Config.GamblingCoinMinUse, Config.GamblingCoinMaxUse);

                Log.Debug($"Registered new coin: {CoinUses[ev.Player.CurrentItem.Serial]} uses left.");
            }

            CoinUses[ev.Player.CurrentItem.Serial]--;

            int remainingUses = CoinUses[ev.Player.CurrentItem.Serial];

            bool shouldBreak = remainingUses <= 0;

            EffectType type = ev.IsTails ? EffectType.Negative : EffectType.Positive;

            ICoinEffect effect = GamblingCoinManager.GetRandomEffect(type);
            
            if (effect == null)
            {
                Log.Warn($"No {type} effect found in GamblingCoinManager!");
                ev.Player.Broadcast(5, "This coin is empty.");
                return;
            }

            Log.Debug("Effect chosen : " + effect.Name);

            effect.Execute(ev.Player);

            if (effect is IDurationEffect durationEffect && durationEffect.Duration > 0)
            {
                float duration = durationEffect.Duration;
                if (durationEffect.Duration == -1) duration = 99999;

                Timing.CallDelayed(duration, () =>
                {
                    durationEffect.ExecuteAfterDuration(ev.Player);
                });
            }

            if (!string.IsNullOrEmpty(effect.Message))
            {
                PlayerUtils.SendBroadcast(ev.Player, effect.Message);
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