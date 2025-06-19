using Exiled.API.Features;
using Exiled.CustomItems.API.Features;

namespace KE.Items.Features
{
    public abstract class KECustomGrenade : CustomGrenade
    {

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
