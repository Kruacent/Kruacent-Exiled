using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using KE.Items.Interface;
using System.Collections.Generic;
using UnityEngine;
using Exiled.Events.EventArgs.Player;
using Exiled.API.Features.Toys;
using Player = Exiled.API.Features.Player;
using MEC;
using Exiled.API.Features.Items;
using Model = KE.Items.Models.Model;
using KE.Items.Models;

namespace KE.Items.Items
{
    [CustomItem(ItemType.KeycardJanitor)]
    public class Mine : CustomItem, ILumosItem
    {
        public override uint Id { get; set; } = 1413;
        public override string Name { get; set; } = "Mine";
        public override string Description { get; set; } = "Drop to deploy the mine, little advice : don't step on it";
        public override float Weight { get; set; } = 0.65f;
        public UnityEngine.Color Color { get; set; } = UnityEngine.Color.yellow;

        private const float RefreshRate = .01f;
        private const int MineActivationTime = 10;
        private const float MineRadius = 0.7f;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 2,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new DynamicSpawnPoint()
                {
                    Chance= 25,
                    Location = SpawnLocationType.Inside049Armory,
                },
                new DynamicSpawnPoint()
                {
                    Chance= 25,
                    Location = SpawnLocationType.InsideEscapeSecondary,
                },
                new DynamicSpawnPoint()
                {
                    Chance= 25,
                    Location = SpawnLocationType.InsideGateA,
                },
                new DynamicSpawnPoint()
                {
                    Chance= 25,
                    Location = SpawnLocationType.InsideGateB,
                }
            },
            LockerSpawnPoints = new List<LockerSpawnPoint>
            {
                new LockerSpawnPoint()
                {
                    Chance= 20,
                    Type = LockerType.Misc,
                },
                new LockerSpawnPoint()
                {
                    Chance= 20,
                    Type = LockerType.RifleRack,
                },
            }

        };

        [System.Obsolete]
        protected override void OnDropping(DroppingItemEventArgs ev)
        {
            if(!Check(ev.Item)) 
                return;
            if (ev.IsThrown)
            {
                ev.IsAllowed = true;
                return;
            }
            
            ev.IsAllowed = false;
            ev.Player.RemoveItem(ev.Item);

            Model m = new MineModel();

            m.Spawn(new Vector3(ev.Player.Position.x, ev.Player.Position.y - .8f, ev.Player.Position.z));

            //SpawnMine(ev.Player, new Vector3(ev.Player.Position.x,ev.Player.Position.y - .8f,ev.Player.Position.z));

        }

        private void SpawnMine(Player player, Vector3 playerPosition)
        {
            Vector3 minePosition = playerPosition;

            // The base part of mine
            Primitive mine = Primitive.Create(PrimitiveType.Cylinder, minePosition, null, new Vector3(MineRadius, 0.01f, MineRadius), true);
            mine.Collidable = true;
            mine.Visible = true;
            mine.Color = Color.black;

            Timing.RunCoroutine(WaitAndActivateMine(player, mine));
        }


        private IEnumerator<float> WaitAndActivateMine(Player player, Primitive mine)
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

        private IEnumerator<float> ActiveMine(Primitive mine, float cylinderSize)
        {
            bool endWhile = true;
            while (endWhile)
            {
                foreach (Player player in Player.List)
                {
                    if (IsPlayerInZone(player, mine.Position, cylinderSize, 2))
                    {
                        ((ExplosiveGrenade)Item.Create(ItemType.GrenadeHE)).SpawnActive(mine.Position).FuseTime = 0f;

                        // Delete the mine
                        mine.UnSpawn();
                        endWhile = false;
                        mine.Destroy();
                        yield break;
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
