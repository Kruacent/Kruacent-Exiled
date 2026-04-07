using UnityEngine;
using Exiled.Events.EventArgs.Player;
using Exiled.API.Features.Spawn;
using KE.Items.API.Interface;
using KE.Items.Items.ItemEffects;
using KE.Items.API.Features;
using System.Collections.Generic;
using Exiled.API.Enums;
using KE.Items.API.Core.Models;
using KE.Items.Items.PickupModels;

namespace KE.Items.Items
{
    public class Mine : KECustomItem, ISwitchableEffect, ICustomPickupModel,IViolentItem
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Mine",
                    [TranslationKeyDesc] = "Drop to deploy the mine, little advice : don't step on it",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Mine",
                    [TranslationKeyDesc] = "Une Mine ! Lâcher pour la déployer",
                },
            };
        }
        public override ItemType ItemType => ItemType.KeycardJanitor;
        public bool IsViolent => true;
        public override string Name { get; set; } = "Mine";
        public override float Weight { get; set; } = 0.65f;
        public Color Color { get; set; } = Color.yellow;
        public CustomItemEffect Effect { get; set; }


        public override SpawnProperties SpawnProperties { get; set; } = null;
        /*new SpawnProperties()
        {
            Limit = 1,
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

        public PickupModel PickupModel { get; }

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
