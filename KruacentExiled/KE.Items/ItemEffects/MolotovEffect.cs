using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Map;
using KE.Items.Interface;
using MEC;
using PlayerRoles;
using Exiled.API.Enums;
using System.Collections.Generic;
using UnityEngine;
using Exiled.Events.EventArgs.Player;

namespace KE.Items.ItemEffects
{
    public class MolotovEffect : CustomItemEffect
    {
        public const float RefreshRate = 0.5f;
        public const float Duration = 20f;
        public float CylinderSize { get; set; } = 5;


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
            SetZone(ev.Player, ev.Player.Position,ev.TargetsToAffect);
        }


        private void SetZone(Player player,Vector3 position,HashSet<Player> targets = null)
        {
            float cylinderSize = CylinderSize;

            targets?.Clear();

            Player playerThrowingGrenade = player;
            Vector3 molotovPosition = position;
            Primitive wall = Primitive.Create(PrimitiveType.Cylinder, molotovPosition, null, new Vector3(cylinderSize, 0.01f, cylinderSize), true);
            wall.Collidable = false;
            wall.Visible = true;

            wall.Color = Color.red;

            var coroutineHandler = Timing.RunCoroutine(DamageInMolotovZone(wall.Position, cylinderSize, playerThrowingGrenade));

            Timing.CallDelayed(Duration, () => {
                wall.UnSpawn();
                Timing.KillCoroutines(coroutineHandler);
                wall.Destroy();
            });
        }



        private IEnumerator<float> DamageInMolotovZone(Vector3 wallPosition, float cylinderSize, Player playerThrowingGrenade)
        {
            // Dictionary that stores the time each player has spent inside the zone (in seconds).
            Dictionary<Player, float> playerTimeInZone = new Dictionary<Player, float>();

            while (true)
            {
                foreach (Player player in Player.List)
                {
                    if (IsPlayerInZone(player, wallPosition, cylinderSize))
                    {
                        if (Exiled.API.Features.Server.FriendlyFire || playerThrowingGrenade.Role.Team != player.Role.Team || playerThrowingGrenade == player)
                        {
                            if (player.IsHuman || player.Role == RoleTypeId.Scp0492)
                            {
                                if (playerTimeInZone.ContainsKey(player))
                                {
                                    // increase time each frame.
                                    playerTimeInZone[player] += Time.deltaTime;
                                }
                                else
                                {
                                    // Init the time in dictionnary of the player.
                                    playerTimeInZone[player] = Time.deltaTime;
                                }

                                // time of player spend inside of molotov zone.
                                float timeInZone = playerTimeInZone[player];

                                // Beginning it willbe 5dm/s, after it will be linearly higher the damage until 20dm/s. 
                                float damage = Mathf.Lerp(2.5f, 10f, timeInZone / 20f);

                                // double damage if it's zombie cuz it has more hp.
                                if (player.Role == RoleTypeId.Scp0492)
                                {
                                    damage *= 2.5f;
                                }

                                player.Hurt(damage, DamageType.Bleeding);
                            }
                            else if (player.IsScp)
                            {
                                player.Hurt(player.Health / 150, DamageType.Bleeding);
                            }

                        }
                    }
                }

                yield return Timing.WaitForSeconds(RefreshRate);
            }
        }


        private bool IsPlayerInZone(Player player, Vector3 zonePosition, float radius)
        {
            float distance = Vector3.Distance(new Vector3(player.Position.x, 0, player.Position.z),
                                               new Vector3(zonePosition.x, 0, zonePosition.z));
            return distance <= (radius / 2);
        }
    }
}
