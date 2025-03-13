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
using System.Xml.Linq;

namespace KE.Items
{
    public abstract class KECustomItem : CustomItem
    {
        
        protected override void ShowPickedUpMessage(Player player)
        {
            Message(this, player);
        }

        protected override void ShowSelectedMessage(Player player)
        {
            Message(this, player);
        }


        internal static void Message(CustomItem c,Player player)
        {
            if (CustomItems.Instance.Config.PickedUpHint.Show)
            {
                string show =  $"<b>{c.Name}</b>\n{c.Description} \n";
                if (c is IUpgradableCustomItem ci)
                {
                    foreach (var a in ci.Upgrade)
                    {
                        show += $"{a.Value.Chance}% chance of upgrading on {a.Key}\n";
                    }
                }
                DisplayPlayer.Get(player).Hint((float)HintPlacement.CustomItem, "<align=right>" +show+"</align>", (int)CustomItems.Instance.Config.PickedUpHint.Duration);
                //player.ShowHint(show, (int)CustomItems.Instance.Config.PickedUpHint.Duration);

            }
        }
    }
}
