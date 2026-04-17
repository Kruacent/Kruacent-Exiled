using System.Collections.Generic;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KruacentExiled.CustomItems.API.Core.Models;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Interface;
using KruacentExiled.CustomItems.Items.PickupModels;
using UnityEngine;

namespace KruacentExiled.CustomItems.Items
{
    public class SainteGrenada : KECustomGrenade, ICustomPickupModel, IViolentItem
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Holy Grenade",
                    [TranslationKeyDesc] = "HOLY SHIT WORMS????",
                },
                ["fr"] = new Dictionary<string, string>()
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
