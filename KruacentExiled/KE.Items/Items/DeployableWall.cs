using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using KE.Items.Interface;
using System.Collections.Generic;
using UnityEngine;
using Exiled.Events.EventArgs.Player;
using Exiled.API.Features.Toys;
using MEC;

namespace KE.Items.Items
{
    [CustomItem(ItemType.KeycardJanitor)]
    public class DeployableWall : CustomItem, ILumosItem
    {
        
        public override uint Id { get; set; } = 1048;
        public override string Name { get; set; } = "Deployable Wall";
        public override string Description { get; set; } = "Drop to deploy a wall, and throw to just throw the card";
        public override float Weight { get; set; } = 0.65f;
        public UnityEngine.Color Color { get; set; } = UnityEngine.Color.green;
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
            ev.Player.ShowHint("You have dropped a deployable wall");
            ev.Player.RemoveItem(ev.Item);
            SpawnWall(ev.Player.Position,ev.Player.Rotation);

        }

        private void SpawnWall(Vector3 pos, Quaternion rotation)
        {
            float distance = 2;
            Vector3 forward = rotation * Vector3.forward;
            Vector3 spawnPos = pos + forward * distance;
            Vector3 rotat = new Vector3(0, rotation.eulerAngles.y, 0);

            MainPlugin.Instance.Sound.PlayClip("build", spawnPos);
            Primitive wall = Primitive.Create(PrimitiveType.Cube, spawnPos, rotat, new Vector3(4, 4, 0.2f),true);
            wall.Collidable = true;
            wall.Visible = true;
            Timing.CallDelayed(10, () => {
                wall.UnSpawn();
                wall.Destroy();
            });
            Timing.CallDelayed(5, () =>
            {
                wall.Color= Color.yellow;
            });
            Timing.CallDelayed(8, () =>
            {
                wall.Color = Color.red;
            }); 


        }

    }

}
