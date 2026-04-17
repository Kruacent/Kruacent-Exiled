
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Spawn;
using KE.Utils.API.Features;
using KruacentExiled.CustomItems.API.Core.Models;
using KruacentExiled.CustomItems.API.Core.Upgrade;
using KruacentExiled.CustomItems.API.Events;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Interface;
using KruacentExiled.CustomItems.Items.PickupModels;
using Scp914;
using System.Collections.Generic;

namespace KruacentExiled.CustomItems.Items
{
    public class PressePuree : KECustomGrenade, IUpgradableCustomItem, ICustomPickupModel
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Presse Purée",
                    [TranslationKeyDesc] = "explode at impact but does less damage",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Presse Purée",
                    [TranslationKeyDesc] = "Explosion à l'impact mais moins efficace",
                },
            };
        }
        public override string Name { get; set; } = "PressePuree";

        public override ItemType ItemType => ItemType.GrenadeHE;
        public override float Weight { get; set; } = 0.65f;
        public override float FuseTime => 5f;
        public override bool ExplodeOnCollision => true;
        public override float DamageModifier => .3f;
        public PickupModel PickupModel { get; }
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 5,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new DynamicSpawnPoint()
                {
                    Chance = 50,
                    Location = SpawnLocationType.InsideHczArmory,
                },
                new DynamicSpawnPoint()
                {
                    Chance = 5,
                    Location = SpawnLocationType.Inside914,
                },
                new DynamicSpawnPoint()
                {
                    Chance= 50,
                    Location = SpawnLocationType.Inside049Armory,
                },
                new DynamicSpawnPoint()
                {
                    Chance= 50,
                    Location = SpawnLocationType.InsideLczArmory,
                }
            },
        };

        public PressePuree()
        {
            PickupModel = new PressePureePModel(this);
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




        protected override void OnExplodeDestructible(OnExplodeDestructibleEventsArgs ev)
        {
            KELog.Debug("old dmagea="+ev.Damage);
            Player player = Player.Get(ev.Destructible.NetworkId);
            if (!Check(ev.ExplosionGrenade)) return;

            
            if (ev.Damage <= 0f) return;

            if (player != null && player.IsScp)
            {
                ev.Damage /= 3f;
            }

            KELog.Debug("new daamager="+ev.Damage);

        }

        public IReadOnlyDictionary<Scp914KnobSetting, UpgradeProperties> Upgrade { get; private set; } = new Dictionary<Scp914KnobSetting, UpgradeProperties>()
        {
            { Scp914KnobSetting.VeryFine,new UpgradeProperties(5, "SainteGrenada")}
        };
    }
}
