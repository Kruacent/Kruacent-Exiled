using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.CustomItems;
using Exiled.CustomItems.API.Features;
using KE.Items.Interface;
using KE.Utils.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
