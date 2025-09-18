using Exiled.CustomItems.API.Features;
using System;
using System.Collections.Generic;
using UnityEngine;
using Exiled.Events.EventArgs.Player;
using KE.Items.ItemEffects;
using KE.Utils.Quality.Models;
using KE.Utils.Quality.Models.Examples;
using MEC;
using KE.Items.Features;
using Exiled.API.Features.Toys;
using KE.Utils.API.Models.Blueprints;
using UnityEngine.Experimental.GlobalIllumination;
using Exiled.API.Interfaces;
using KE.Items.Items.PickupModels;

namespace KE.Items.Items
{
    [CustomItem(ItemType.KeycardJanitor)]
    public class Mine : KECustomItem, ILumosItem, ISwichableEffect, ICustomPickupModel
    {
        public override uint Id { get; set; } = 1053;
        public override string Name { get; set; } = "Mine";
        public override string Description { get; set; } = "Drop to deploy the mine, little advice : don't step on it";
        public override float Weight { get; set; } = 0.65f;
        public Color Color { get; set; } = Color.yellow;
        public PickupModel PickupModel { get; set; }
        public CustomItemEffect Effect { get; set; }


        public override SpawnProperties SpawnProperties { get; set; } = null;
        /*new SpawnProperties()
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

        };*/

        public Mine()
        {
            Effect = new MineEffect();
            PickupModel = new MinePModel(this);
        }

        protected override void SubscribeEvents()
        {
            PickupModel.SubscribeEvents();
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            PickupModel.UnsubscribeEvents();
            base.UnsubscribeEvents();
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
