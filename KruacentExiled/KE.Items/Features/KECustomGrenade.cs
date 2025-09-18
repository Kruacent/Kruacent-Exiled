using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;

namespace KE.Items.Features
{
    public abstract class KECustomGrenade : CustomGrenade
    {
        public virtual float DamageModifier { get; set; } = 1f;


        protected override void SubscribeEvents()
        {

            Exiled.Events.Handlers.Player.Hurting += InternalOnHurting;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting -= InternalOnHurting;
            base.UnsubscribeEvents();
        }


        protected void InternalOnHurting(HurtingEventArgs ev)
        {
            
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
