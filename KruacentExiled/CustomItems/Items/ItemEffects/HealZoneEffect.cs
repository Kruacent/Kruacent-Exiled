using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KruacentExiled.CustomItems.API.Interface;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KruacentExiled.CustomItems.Items.ItemEffects
{
    public class HealZoneEffect : CustomItemEffect
    {

        public override void Effect(UsedItemEventArgs ev)
        {
            SetZone(ev.Player, ev.Player.Position);
        }
        public override void Effect(DroppingItemEventArgs ev)
        {
            SetZone(ev.Player, ev.Player.Position);
        }

        public override void Effect(ExplodingGrenadeEventArgs ev)
        {
            SetZone(ev.Player, ev.Position);
        }

        private void SetZone(Player player, Vector3 position)
        {
            float cylinderSize = 5;

            Player playerThrowingGrenade = player;
            Vector3 healZonePosition = position;
            Primitive wall = Primitive.Create(PrimitiveType.Cylinder, healZonePosition, null, new Vector3(cylinderSize, 0.01f, cylinderSize), true);
            wall.Collidable = false;
            wall.Visible = true;

            wall.Color = Color.green;

            var coroutineHandler = Timing.RunCoroutine(HealZoneHeal(wall.Position, cylinderSize, playerThrowingGrenade));

            Timing.CallDelayed(20, () =>
            {
                wall.UnSpawn();
                Timing.KillCoroutines(coroutineHandler);
                wall.Destroy();
            });
        }

        private IEnumerator<float> HealZoneHeal(Vector3 wallPosition, float cylinderSize, Player playerThrowingGrenade)
        {
            Dictionary<Player, int> playerHealedAmounts = new Dictionary<Player, int>();


            Team team = playerThrowingGrenade.Role.Team;


            while (true)
            {
                foreach (Player player in Player.List)
                {

                    // Check if a player is in the zone.
                    if (IsPlayerInZone(player, wallPosition, cylinderSize))
                    {
                        if (team == player.Role.Team)
                        {
                            if (!playerHealedAmounts.ContainsKey(player))
                            {
                                playerHealedAmounts[player] = 0;
                            }



                            if (playerHealedAmounts[player] <= 100)
                            {
                                player.Heal(1);
                                playerHealedAmounts[player] += 1;
                            }
                        }
                    }
                }

                // Waiting 0.5s before re-check.
                yield return Timing.WaitForSeconds(0.1f);
            }
        }

        private bool IsPlayerInZone(Player player, Vector3 zonePosition, float radius)
        {
            float distance = Vector3.Distance(new Vector3(player.Position.x, 0, player.Position.z),
                                               new Vector3(zonePosition.x, 0, zonePosition.z));
            return distance <= radius / 2;
        }
    }
}
