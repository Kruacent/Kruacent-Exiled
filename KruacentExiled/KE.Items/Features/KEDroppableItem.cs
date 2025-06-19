using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Items.Features
{
    public abstract class KEDroppableItem : KECustomKeycard
    {

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DroppingItem += this.DroppingItem;
            base.SubscribeEvents();
        }


        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DroppingItem -= this.DroppingItem;
            base.UnsubscribeEvents();
        }


        private void DroppingItem(DroppingItemEventArgs ev)
        {
            if (!Check(ev.Item)) return;
            if (ev.IsThrown)
            {
                return;
            }

            ev.IsAllowed = false;
            DroppingEffect(ev);
        }


        /// <summary>
        /// no need to check if thrown
        /// but still need to remove the item afterward
        /// </summary>
        /// <param name="ev"></param>
        protected abstract void DroppingEffect(DroppingItemEventArgs ev);

    }
}
