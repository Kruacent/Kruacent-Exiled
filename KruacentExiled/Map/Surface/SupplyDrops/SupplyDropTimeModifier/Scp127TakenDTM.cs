using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Map.Surface.SupplyDrops.SupplyDropTimeModifier
{
    internal class Scp127TakenDTM : SupplyDropTimeModifierBase
    {
        protected override TimeSpan TimeAdded => TimeSpan.FromMinutes(1);

        protected override void SubscribeEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.PickedUpItem += OnPickedUpItem;
        }

        protected override void UnsubscribeEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.PickedUpItem -= OnPickedUpItem;
        }


        private void OnPickedUpItem(LabApi.Events.Arguments.PlayerEvents.PlayerPickedUpItemEventArgs ev)
        {
            if (IsActive) return;


            if(ev.Item.Type == ItemType.GunSCP127)
            {
                IsActive = true;
                return;
            }


        }
    }
}
