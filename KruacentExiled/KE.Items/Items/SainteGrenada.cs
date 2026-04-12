
using System;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Items.API.Core.Models;
using KE.Items.API.Features;
using KE.Items.API.Interface;
using KE.Items.Items.PickupModels;
using MEC;
using UnityEngine;

namespace KE.Items.Items
{
    public class SainteGrenada : KECustomGrenade, ICustomPickupModel, IViolentItem
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Holy Grenade",
                    [TranslationKeyDesc] = "HOLY SHIT WORMS????",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Sainte Grenade",
                    [TranslationKeyDesc] = "Worms reference !?",
                },
            };
        }
        public bool IsViolent => false;
        public override ItemType ItemType => ItemType.GrenadeHE;
        public override string Name { get; set; } = "SainteGrenada";
        public override float Weight { get; set; } = 1.5f;
        public override float FuseTime => 6f;
        public override bool ExplodeOnCollision => false;
        public override float DamageModifier => 3f;
        public Color Color { get; set; } = Color.red;


        public int NbGrenadeSpawned { get; set; } = 3;
        public float SpawnRadius { get; set; } = 5f;
        public float GrenadeSize { get; set; } = 4f;


        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {

        };

        public PickupModel PickupModel { get; }


        public SainteGrenada()
        {
            PickupModel = new HolyGrenadePModel(this);
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


        protected override void OnThrownProjectile(ThrownProjectileEventArgs ev)
        {
            KE.Utils.API.Sounds.SoundPlayer.Instance.Play("worms", ev.Projectile.Position, 50,20f);
        }
        protected override void OnExplodingGrenade(ExplodingGrenadeEventArgs ev)
        {
            ev.Projectile.Scale = new Vector3(GrenadeSize, GrenadeSize, GrenadeSize);

            for (int i = 0; i < NbGrenadeSpawned; i++)
            {

                Vector3 spawnPosition = ev.Position;

                ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                grenade.SpawnActive(spawnPosition).FuseTime = 0f;
            }


        }
    }
}
