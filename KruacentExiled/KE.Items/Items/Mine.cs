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
using KE.Items.ItemEffects;

namespace KE.Items.Items
{
    [CustomItem(ItemType.KeycardJanitor)]
    public class Mine : CustomItem, ILumosItem, ISwichableEffect
    {
        public override uint Id { get; set; } = 1053;
        public override string Name { get; set; } = "Mine";
        public override string Description { get; set; } = "Drop to deploy the mine, little advice : don't step on it";
        public override float Weight { get; set; } = 0.65f;
        public Color Color { get; set; } = Color.yellow;

        public CustomItemEffect Effect { get; set; }


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

        public Mine()
        {
            Effect = new MineEffect();
        }

        protected override void OnDroppingItem(DroppingItemEventArgs ev)
        {
            

            if (ev.IsThrown)
            {
                ev.IsAllowed = true;
                return;
            }

            ev.IsAllowed = false;
            ev.Player.RemoveItem(ev.Item);
            Effect.Effect(ev);

        }


        
    }
}
