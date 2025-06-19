using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Items.Extensions;
using KE.Items.Interface;
using KE.Items.Items.Models;
using KE.Utils.API.Interfaces;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Items.ItemEffects
{
    public class MineEffect : CustomItemEffect, IUsingEvents
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

        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Map.ExplodingGrenade += OnExplodingGrenade;
        }
        public void UnsubscribeEvents()
        {

        }

        private void OnExplodingGrenade(ExplodingGrenadeEventArgs ev)
        {
            
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
            m.Create(pos, new Quaternion());

            Timing.RunCoroutine(WaitAndActivateMine(p, m));
        }

        private ExplosiveGrenade _grenade;
        private ExplosiveGrenade Grenade
        {
            get
            {
                if (_grenade == null)
                {
                    _grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                    _grenade.MaxRadius = 3;
                    _grenade.ScpDamageMultiplier = 1f;
                    _grenade.FuseTime = 0f;
                }
                return _grenade;
            }
        }


        private IEnumerator<float> WaitAndActivateMine(Player player, MineModel mine)
        {
            int countdown = MineActivationTime;
            while (countdown > 0)
            {
                player.ItemEffectHint($"The mine will be active in {countdown} seconds !");
                yield return Timing.WaitForSeconds(1f);
                countdown--;
            }

            // Message final lorsque la mine s'active
            player.ItemEffectHint("Mine activated !");
            Timing.RunCoroutine(ActiveMine(mine, MineRadius));
        }

        private IEnumerator<float> ActiveMine(MineModel mine, float cylinderSize)
        {
            Timing.RunCoroutine(mine.Activate());
            bool isActivated = true;
            while (isActivated)
            {

                foreach (IWorldSpace p in Pickup.List)
                {
                    if (IsPositionInZone(p.Position,mine.Position, cylinderSize, 3))
                    {
                        Grenade.SpawnActive(mine.Position);
                        DestroyMine(mine);
                        isActivated = false;
                        break;
                    }
                }

                foreach (Player player in Player.List)
                {

                    if (isActivated && IsPlayerInZone(player, mine.Position, cylinderSize, 3))
                    {
                        Grenade.SpawnActive(mine.Position);
                        DestroyMine(mine);
                        isActivated = false;
                        break;
                    }
                }

                yield return Timing.WaitForSeconds(RefreshRate);
            }
        }

        private void DestroyMine(MineModel mine)
        {
            mine.Destroy();
            UnsubscribeEvents();
        }


        private bool IsPlayerInZone(Player player, Vector3 zonePosition, float radius, float height)
        {
            return IsPositionInZone(player.Position, zonePosition, radius, height);

        }

        private bool IsPositionInZone(Vector3 position, Vector3 zonePosition, float radius, float height)
        {
            // Calculate the horizontal distance (x, z)
            float horizontalDistance = Vector3.Distance(
                new Vector3(position.x, 0, position.z),
                new Vector3(zonePosition.x, 0, zonePosition.z)
            );

            // Calculate the vertical difference (y)
            float verticalDifference = Mathf.Abs(position.y - zonePosition.y);

            // Check if the player is in the 3d zone.
            return horizontalDistance <= (radius / 2) && verticalDifference <= (height / 2);
        }

    }
}
