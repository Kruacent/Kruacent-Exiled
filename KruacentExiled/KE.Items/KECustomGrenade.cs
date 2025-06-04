using Exiled.API.Features;
using Exiled.CustomItems.API.Features;

namespace KE.Items
{
    public abstract class KECustomGrenade : CustomGrenade
    {
        
        protected override void ShowPickedUpMessage(Player player)
        {
            KECustomItem.Message(this,player);
        }

        protected override void ShowSelectedMessage(Player player)
        {
            KECustomItem.Message(this, player);
        }
    }
}
