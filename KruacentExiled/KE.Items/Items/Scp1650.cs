using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.Items.Features;
using KE.Items.Interface;
using KE.Items.ItemEffects;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using PlayerHandle = Exiled.Events.Handlers.Player;


namespace KE.Items.Items
{
    [CustomItem(ItemType.Painkillers)]
    public class Scp1650 : KECustomItem, ISwichableEffect
    {
        //mamie
        public override uint Id { get; set; } = 1056;
        public override string Name { get; set; } = "SCP-1650";
        public override string Description { get; set; } = "ויאמר ה' ל— סח השמן על בשרך ולך בינות ה— כששם אלוהים על שפתיך, וזעם ה' וחרון אפו ו— דם המכבים יטהר בית מקדשו אחריך, לנצח נצחים";
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
