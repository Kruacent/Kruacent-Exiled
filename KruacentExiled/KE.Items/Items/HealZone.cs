using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using KE.Items.API.Interface;
using KE.Items.Items.ItemEffects;
using KE.Items.API.Features;
using Scp914;
using KE.Items.API.Core.Upgrade;

namespace KE.Items.Items
{
    public class HealZone : KECustomGrenade, ILumosItem, ISwitchableEffect, IUpgradableCustomItem
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Heal Zone",
                    [TranslationKeyDesc] = "Allow to heal you and your ally",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Heal Zone",
                    [TranslationKeyDesc] = "Créer une zone pour soigner",
                },
            };
        }
        public override ItemType ItemType => ItemType.GrenadeFlash;
        public override string Name { get; set; } = "HealZone";
        public override string Description { get; set; } = "Allow to heal you and your ally";
        public override float Weight => 0.65f;
        public override float FuseTime => 5f;
        public override bool ExplodeOnCollision => true;
        public UnityEngine.Color Color { get; set; } = UnityEngine.Color.green;
        public CustomItemEffect Effect { get; set; }
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 3,
            LockerSpawnPoints = new List<LockerSpawnPoint>
            {
                new LockerSpawnPoint()
                {
                    Chance = 75,
                    UseChamber = true,
                    Type = LockerType.Medkit,
                    Zone = ZoneType.Entrance,
                },
                new LockerSpawnPoint()
                {
                    Chance = 50,
                    UseChamber = true,
                    Type = LockerType.Medkit,
                    Zone = ZoneType.LightContainment,
                },
                new LockerSpawnPoint()
                {
                    Chance = 100,
                    UseChamber = true,
                    Type = LockerType.Medkit,
                    Zone = ZoneType.HeavyContainment,
                },
            },

            RoomSpawnPoints = new List<RoomSpawnPoint>
            {
                new RoomSpawnPoint()
                {
                    Chance = 75,
                    Room = RoomType.HczHid,
                },
                new RoomSpawnPoint()
                {
                    Chance = 50,
                    Room = RoomType.HczNuke,
                },
            },
        };

        public IReadOnlyDictionary<Scp914KnobSetting, UpgradeProperties> Upgrade => new Dictionary<Scp914KnobSetting, UpgradeProperties>()
        {
            [Scp914KnobSetting.OneToOne] = new UpgradeProperties(100, "CocktailMolotov")
        };

        public HealZone()
        {
            Effect = new HealZoneEffect();
        }

        protected override void OnExplodingGrenade(ExplodingGrenadeEventArgs ev)
        {
            
            Effect.Effect(ev);
            ev.TargetsToAffect.Clear();
        }
    }
}