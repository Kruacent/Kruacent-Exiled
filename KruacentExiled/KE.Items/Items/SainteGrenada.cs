
using System;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Items.Features;
using KE.Items.Interface;
using UnityEngine;

namespace KE.Items.Items
{
    [CustomItem(ItemType.GrenadeHE)]
    public class SainteGrenada : KECustomGrenade, ILumosItem
    {
        public override uint Id { get; set; } = 1055;
        public override string Name { get; set; } = "Sainte Grenada";
        public override string Description { get; set; } = "Worms reference !?";
        public override float Weight { get; set; } = 1.5f;
        public override float FuseTime { get; set; } = 6f;
        public override bool ExplodeOnCollision { get; set; } = false;
        public float DamageModifier { get; set; } = 3f;
        public Color Color { get; set; } = Color.red;


        //
        public int NbGrenadeSpawned { get; set; } = 4;
        public float SpawnRadius { get; set; } = 5f;
        public float GrenadeSize { get; set; } = 4f;
        //


        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {

        };

        protected override void OnThrownProjectile(ThrownProjectileEventArgs ev)
        {
            MainPlugin.Instance.Sound.PlayClip("worms", ev.Projectile.GameObject,4,75);
        }
        protected override void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            ev.Projectile.Scale = new Vector3(GrenadeSize, GrenadeSize, GrenadeSize);

            for (int i = 0; i < NbGrenadeSpawned; i++)
            {
                float angle = UnityEngine.Random.Range(0f, 360f) * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * SpawnRadius;

                Vector3 spawnPosition = ev.Position + offset;

                ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                grenade.SpawnActive(spawnPosition).FuseTime = 0f;
            }


        }
    }
}
