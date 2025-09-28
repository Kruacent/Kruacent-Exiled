using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;

namespace KE.Items.API.Features
{
    public abstract class KECustomGrenade : CustomGrenade
    {
        public virtual float DamageModifier { get; set; } = 1f;


        protected override void SubscribeEvents()
        {

            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
        }



        protected void InternalOnHurting(HurtingEventArgs ev)
        {

            //can't get the custom grenade
            /*if(ev.DamageHandler.Type == Exiled.API.Enums.DamageType.Explosion)
            {
                ev.Amount *= DamageModifier;
            }*/

        }

        protected override void ShowPickedUpMessage(Player player)
        {
            KECustomItem.Message(this, player, true);
        }

        protected override void ShowSelectedMessage(Player player)
        {
            KECustomItem.Message(this, player);
        }
    }
}
