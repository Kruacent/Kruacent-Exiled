using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using System.Collections.Generic;
using UnityEngine;
using Exiled.Events.EventArgs.Player;
using KE.Items.API.Interface;
using KE.Items.Items.ItemEffects;
using KE.Items.API.Features;

namespace KE.Items.Items
{
    [CustomItem(ItemType.KeycardJanitor)]
    public class DeployableWall : KECustomItem, ILumosItem, ISwichableEffect
    {
        
        public override uint Id { get; set; } = 1048;
        public override string Name { get; set; } = "Deployable Wall";
        public override string Description { get; set; } = "Drop to deploy a wall, and throw to just throw the card";
        public override float Weight { get; set; } = 0.65f;
        public Color Color { get; set; } = Color.green;

        public CustomItemEffect Effect { get; set; }
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 2,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new DynamicSpawnPoint()
                {
                    Chance=25,
                    Location = SpawnLocationType.Inside049Armory,
                },
                new DynamicSpawnPoint()
                {
                    Chance=25,
                    Location = SpawnLocationType.InsideLczArmory,
                }
            },
            LockerSpawnPoints = new List<LockerSpawnPoint>
            {
                new LockerSpawnPoint()
                {
                    Chance=50,
                    Type = LockerType.RifleRack,
                },
            }

        };

        public DeployableWall()
        {
            Effect = new DeployableWallEffect();
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
