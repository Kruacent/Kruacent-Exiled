using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
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
    public class Scp1650 : CustomItem
    {
        public override uint Id { get; set; } = 1056;
        public override string Name { get; set; } = "SCP-1650";
        public override string Description { get; set; } = "ויאמר ה' ל— סח השמן על בשרך ולך בינות ה— כששם אלוהים על שפתיך, וזעם ה' וחרון אפו ו— דם המכבים יטהר בית מקדשו אחריך, לנצח נצחים";
        public override float Weight { get; set; } = 0.65f;

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
            Player player = ev.Player;
            Quaternion rotation = player.Rotation;

            switch (Rotation(rotation))
            {
                case 1:
                    //give regen
                    Log.Debug("South");
                    break;
                case 2:
                    Log.Debug("West");
                    player.EnableEffect(Exiled.API.Enums.EffectType.BodyshotReduction, 50, 60);
                    break;
                case 3:
                    Log.Debug("North");
                    player.EnableEffect(Exiled.API.Enums.EffectType.Invigorated, 1, 30);
                    break;
                case 4:
                    Log.Debug("East");
                    player.EnableEffect(Exiled.API.Enums.EffectType.CardiacArrest,1,10);
                    break;

            }


        }

        private int Rotation(Quaternion rotation)
        {

            Vector3 forward1 = rotation * Vector3.forward;
            float x = forward1.x;
            float z = forward1.z;

            if (z > .75f)
                return 1;
            if (x > .75f)
                return 2;
            if (z <= -.75f)
                return 3;
            if (x <= -.75f)
                return 4;
            return -1;
        }
    }
}
