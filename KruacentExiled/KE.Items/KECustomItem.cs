using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.CustomItems;
using Exiled.CustomItems.API.Features;
using KE.Items.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Items
{
    public abstract class KECustomItem : CustomItem
    {
        
        protected override void ShowPickedUpMessage(Player player)
        {
            if (CustomItems.Instance.Config.PickedUpHint.Show)
            {
                string show = string.Format(CustomItems.Instance.Config.PickedUpHint.Content, Name, Description) + "\n";
                if (this is IUpgradableCustomItem ci)
                {
                    foreach(var a in ci.Upgrade)
                    {
                        show += $"{a.Value.Chance}% chance of upgrading on {a.Key}\n";
                    }
                }
                
                player.ShowHint(show, (int)CustomItems.Instance.Config.PickedUpHint.Duration);

            }
        }

        protected override void ShowSelectedMessage(Player player)
        {
            if (CustomItems.Instance.Config.PickedUpHint.Show)
            {
                string show = string.Format(CustomItems.Instance.Config.PickedUpHint.Content, Name, Description) + "\n";
                if (this is IUpgradableCustomItem ci)
                {
                    foreach (var a in ci.Upgrade)
                    {
                        show += $"{a.Value.Chance}% chance of upgrading on {a.Key}\n";
                    }
                }

                player.ShowHint(show, (int)CustomItems.Instance.Config.PickedUpHint.Duration);

            }
        }
    }
}
