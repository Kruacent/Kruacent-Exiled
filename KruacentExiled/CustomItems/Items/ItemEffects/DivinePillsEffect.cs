using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Features;
using KruacentExiled.Audio;
using KruacentExiled.CustomItems.API.Extensions;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Interface;
using KruacentExiled.CustomItems.Events.CustomItem.EventArgs;
using KruacentExiled.Map.Others.BlackoutNDoor.Events.EventArgs;
using KruacentExiled.Misc.Utils;
using MEC;
using PlayerRoles;
using System;
using System.Linq;
using Random = UnityEngine.Random;

namespace KruacentExiled.CustomItems.Items.ItemEffects
{
    public class DivinePillsEffect : CustomItemEffect
    {

        public static Event<UsedDivinePillsEffectArgs> UsedDivinePillsEffect { get; set; } = new Event<UsedDivinePillsEffectArgs>();
        public static void OnUsedDivinePillsEffect(UsedDivinePillsEffectArgs ev)
        {
            UsedDivinePillsEffect?.InvokeSafely(ev);
        }
        public override void Effect(UsedItemEventArgs ev)
        {
            EffectItem(ev.Player);
        }
        public override void Effect(DroppingItemEventArgs ev)
        {
            EffectItem(ev.Player, ev);
        }

        public override void Effect(ExplodingGrenadeEventArgs ev)
        {
            foreach (Player p in ev.TargetsToAffect)
            {
                EffectItem(p);
            }
        }

        public const string NoiseDeath = "divinepills_death";

        private void EffectItem(Player player, IDeniableEvent _ = null)
        {
            if (Player.List.Count(x => x.Role == RoleTypeId.Spectator) == 0)
            {
                player.ItemEffectHint(KECustomItem.GetTranslation(player, DivinePills.NoSpec));
                return;
            }

            bool itemActivation = RandomNumberGenerator.RollChance(75, player, false);
            bool spawnToPlayer = RandomNumberGenerator.RollChance(33, player, true);
            UsedDivinePillsEffectArgs ev = new UsedDivinePillsEffectArgs(player, itemActivation);
            if (itemActivation)
            {
                AudioHandler.Instance.PlayToAll(SoundType.Noise, NoiseDeath, player.Position, 40);
                player.Kill("unlucky bro");
            }
            else
            {
                Player respawning = Player.List.GetRandomValue(x => x.Role == RoleTypeId.Spectator);
                switch (player.Role.Side)
                {
                    case Side.ChaosInsurgency:
                        respawning.Role.Set(RoleTypeId.ChaosRifleman);
                        break;
                    case Side.Mtf:
                        respawning.Role.Set(RoleTypeId.NtfPrivate);
                        break;
                }

                if (spawnToPlayer)
                {
                    Log.Debug("tp");
                    Timing.CallDelayed(1, () => respawning.Teleport(player));

                }
            }
            OnUsedDivinePillsEffect(ev);
        }
    }
}
