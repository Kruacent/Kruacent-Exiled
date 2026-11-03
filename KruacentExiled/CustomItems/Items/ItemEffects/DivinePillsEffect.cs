using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Features;
using KruacentExiled.Audio;
using KruacentExiled.CustomItems.API.Extensions;
using KruacentExiled.CustomItems.API.Interface;
using KruacentExiled.Misc.Utils;
using MEC;
using PlayerRoles;
using System.Linq;
using Random = UnityEngine.Random;

namespace KruacentExiled.CustomItems.Items.ItemEffects
{
    public class DivinePillsEffect : CustomItemEffect
    {
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

        private void EffectItem(Player player, IDeniableEvent ev = null)
        {
            if (Player.List.Count(x => x.Role == RoleTypeId.Spectator) == 0)
            {
                player.ItemEffectHint("No spectators to respawn");
                return;
            }

            bool killPlayer = RandomNumberGenerator.RollChance(25, player, false);
            bool spawnToPlayer = RandomNumberGenerator.RollChance(33, player, true);

            if (killPlayer)
            {
                AudioHandler.Instance.PlayToAll(SoundType.Noise, NoiseDeath, player.Position, 40);
                player.Kill("unlucky bro");
                return;
            }

            RoleTypeId newRole;
            switch (player.Role.Side)
            {
                case Side.ChaosInsurgency:
                    newRole = RoleTypeId.ChaosRifleman;
                    break;
                case Side.Mtf:
                    newRole = RoleTypeId.NtfPrivate;
                    break;
                default: return;
            }

            Player respawning = Player.List.GetRandomValue(x => x.Role == RoleTypeId.Spectator);
            respawning.Role.Set(newRole);

            if (spawnToPlayer)
            {
                KELog.Debug("tp");

                Timing.CallDelayed(1, () => {
                    if(respawning.IsAlive && player.IsAlive)
                    {
                        respawning.Teleport(player);
                    }
                });
                
            }
        }
    }
}
