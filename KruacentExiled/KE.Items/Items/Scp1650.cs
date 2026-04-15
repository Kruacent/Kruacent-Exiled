using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.Items.API.Features;
using KE.Items.API.Interface;
using KE.Items.Items.ItemEffects;
using System.Collections.Generic;
using PlayerHandle = Exiled.Events.Handlers.Player;


namespace KE.Items.Items
{
    public class Scp1650 : KECustomItem, ISwitchableEffect
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "SCP-1650",
                    [TranslationKeyDesc] = "ויאמר ה' ל— סח השמן על בשרך ולך בינות ה— כששם אלוהים על שפתיך, וזעם ה' וחרון אפו ו— דם המכבים יטהר בית מקדשו אחריך, לנצח נצחים",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "SCP-1650",
                    [TranslationKeyDesc] = "ויאמר ה' ל— סח השמן על בשרך ולך בינות ה— כששם אלוהים על שפתיך, וזעם ה' וחרון אפו ו— דם המכבים יטהר בית מקדשו אחריך, לנצח נצחים",
                },
            };
        }
        public override ItemType ItemType => ItemType.Painkillers;

        public override string Name { get; set; } = "SCP-1650";
        public override float Weight { get; set; } = 0.65f;
        public CustomItemEffect Effect { get; set; }

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
            LockerSpawnPoints = new List<LockerSpawnPoint>()
            {
                new LockerSpawnPoint()
                {
                    Type = Exiled.API.Enums.LockerType.Scp207Pedestal,
                    UseChamber = true,
                }
            }
        };

        public Scp1650()
        {
            Effect = new Scp1650Effect();
        }

        protected override void SubscribeEvents()
        {
            PlayerHandle.UsedItem += OnUsedItem;
        }
        protected override void UnsubscribeEvents()
        {
            PlayerHandle.UsedItem -= OnUsedItem;
        }

        private void OnUsedItem(UsedItemEventArgs ev)
        {
            if (!Check(ev.Item)) return;
            Effect.Effect(ev);
        }
    }
}
