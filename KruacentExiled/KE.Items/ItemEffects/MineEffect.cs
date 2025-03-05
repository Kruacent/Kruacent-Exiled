using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Items.Interface;
using KE.Items.Models;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Items.ItemEffects
{
    public class MineEffect : CustomItemEffect
    {
        private const float RefreshRate = .01f;
        private const int MineActivationTime = 10;
        private const float MineRadius = 0.7f;
        public override void Effect(UsedItemEventArgs ev)
        {
            PlaceMine(ev.Player);
        }
        public override void Effect(DroppingItemEventArgs ev)
        {
            PlaceMine(ev.Player);
        }

        public override void Effect(ExplodingGrenadeEventArgs ev)
        {
            PlaceMine(ev.Player,ev.Position);
        }


        /// <summary>
        /// Place the mine at the feet of the player
        /// </summary>
        /// <param name="p"></param>
        private void PlaceMine(Player p)
        {
            SpawnMine(p, p.Position - new Vector3(0, p.Scale.y));
        }
        /// <summary>
        /// Place the mine at the specified position
        /// </summary>
        /// <param name="p"></param>
        /// <param name="pos"></param>
        private void PlaceMine(Player p,Vector3 pos)
        {

            SpawnMine(p, pos);
        }

        private void SpawnMine(Player p,Vector3 pos)
        {

            MineModel m = new MineModel();

            //put the mine on the floor
            m.Spawn(pos, new Quaternion());

            Timing.RunCoroutine(WaitAndActivateMine(p, m));
        }


        private IEnumerator<float> WaitAndActivateMine(Player player, MineModel mine)
        {
            int countdown = MineActivationTime;
            while (countdown > 0)
            {
                player.ShowHint($"The mine will be active in {countdown} seconds !", 1f);
                yield return Timing.WaitForSeconds(1f);
                countdown--;
            }

            // Message final lorsque la mine s'active
            player.ShowHint("Mine activated !");
            Timing.RunCoroutine(ActiveMine(mine, MineRadius));
        }

        private IEnumerator<float> ActiveMine(MineModel mine, float cylinderSize)
        {
            Timing.RunCoroutine(mine.Activate());
            bool endWhile = true;
            while (endWhile)
            {
                foreach (Player player in Player.List)
                {
                    if (IsPlayerInZone(player, mine.Position, cylinderSize, 3))
                    {
                        ((ExplosiveGrenade)Item.Create(ItemType.GrenadeHE)).SpawnActive(mine.Position).FuseTime = 0f;

                        // Delete the mine
                        mine.UnSpawn();
                        endWhile = false;
                        break;
                    }
                }

                yield return Timing.WaitForSeconds(RefreshRate);
            }
        }

        private bool IsPlayerInZone(Player player, Vector3 zonePosition, float radius, float height)
        {
            // Calculate the horizontal distance (x, z)
            float horizontalDistance = Vector3.Distance(
                new Vector3(player.Position.x, 0, player.Position.z),
                new Vector3(zonePosition.x, 0, zonePosition.z)
            );

            // Calculate the vertical difference (y)
            float verticalDifference = Mathf.Abs(player.Position.y - zonePosition.y);

            // Check if the player is in the 3d zone.
            return horizontalDistance <= (radius / 2) && verticalDifference <= (height / 2);
        }

    }
}
