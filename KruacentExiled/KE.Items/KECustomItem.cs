using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using MHints = HintServiceMeow.Core.Models.Hints.Hint;
using KE.Items.Interface;
using System.Text;
using KE.Utils.API.Displays.DisplayMeow;
using MEC;
using HintServiceMeow.Core.Extension;
using HintServiceMeow.Core.Utilities;

namespace KE.Items
{
    public abstract class KECustomItem : CustomItem
    {
        
        protected override void ShowPickedUpMessage(Player player)
        {
            Log.Debug("pickup");
            Message(this, player,true);
        }

        protected override void ShowSelectedMessage(Player player)
        {
            Log.Debug("select");
            Message(this, player);
        }


        internal static void Message(CustomItem c, Player player, bool pickedUp = false)
        {



            StringBuilder builder = new();

            if (MainPlugin.Instance.SettingsHandler.GetPrefixes(player))
            {
                if (pickedUp)
                {
                    builder.Append("(P)");
                }
                else
                {
                    builder.Append("(I)");
                }
            }
            else
            {
                if (pickedUp)
                {
                    builder.Append("You've picked up ");
                }
                else
                {
                    builder.Append("You've selected up ");
                }
            }

            builder.AppendLine($"<b>{c.Name}</b>");
            if (MainPlugin.Instance.SettingsHandler.GetDescriptionsSettings(player))
            {
                builder.AppendLine(c.Description);
                if (c is IUpgradableCustomItem ci)
                {
                    foreach (var a in ci.Upgrade)
                    {
                        builder.AppendLine($"<b>{c.Name}</b>");
                    }
                }
            
            }


            Log.Debug("adding hint");
            
            DisplayHandler.Instance.AddHint(MainPlugin.HintPlacement, player, builder.ToString(),10);

            
        }

    }
}
