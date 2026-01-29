using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Items.API.Extensions;
using KE.Items.API.Interface;
using PlayerRoles;
using System.Linq;
using Random = UnityEngine.Random;

namespace KE.Items.Items.ItemEffects
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



        private void EffectItem(Player player, IDeniableEvent ev = null)
        {
            if (Player.List.Count(x => x.Role == RoleTypeId.Spectator) == 0)
            {
                player.ItemEffectHint("No spectators to respawn");
                return;
            }
            var random = Random.Range(0, 100);

 
            if (random < 25)
            {
                player.Kill("unlucky bro");
                return;
            }
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

            if (random > 75)
            {
                Log.Debug("tp");
                respawning.Teleport(player);
            }
        }
    }
}
