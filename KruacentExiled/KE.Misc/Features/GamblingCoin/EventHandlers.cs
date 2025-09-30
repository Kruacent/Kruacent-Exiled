using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.Misc.Features.GamblingCoin.Interfaces;
using KE.Misc.Features.GamblingCoin.Types;
using MEC;

namespace KE.Misc.Features.GamblingCoin
{
    public class EventHandlers
    {
        private static Config Config => MainPlugin.Instance.Config;

        private readonly System.Random _rd = new();
        private readonly Dictionary<string, DateTime> _cooldowns = new();
        public static Dictionary<ushort, int> CoinUses = new();

        public void OnCoinFlip(FlippingCoinEventArgs ev)
        {
            if (_cooldowns.TryGetValue(ev.Player.UserId, out var lastFlip) &&
                (DateTime.UtcNow - lastFlip).TotalSeconds < Config.GamblingCoinCooldow)
            {
                ev.IsAllowed = false;
                PlayerUtils.SendBroadcast(ev.Player, "You must wait before flipping again");
                return;
            }

            _cooldowns[ev.Player.UserId] = DateTime.UtcNow;

            if (!CoinUses.ContainsKey(ev.Player.CurrentItem.Serial))
            {
                CoinUses[ev.Player.CurrentItem.Serial] =
                    _rd.Next(Config.GamblingCoinMinUse, Config.GamblingCoinMaxUse);

                Log.Debug($"Registered new coin: {CoinUses[ev.Player.CurrentItem.Serial]} uses left.");
            }

            CoinUses[ev.Player.CurrentItem.Serial]--;

            bool shouldBreak = CoinUses[ev.Player.CurrentItem.Serial] <= 0;

            var type = ev.IsTails ? EffectType.Negative : EffectType.Positive;

            var effect = GamblingCoinManager.GetRandomEffect(type);
            
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
                Timing.CallDelayed(durationEffect.Duration, () =>
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
            }
        }
    }
}