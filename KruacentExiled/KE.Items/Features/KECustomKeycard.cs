using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using MHints = HintServiceMeow.Core.Models.Hints.Hint;
using KE.Items.Interface;
using System.Text;
using KE.Utils.API.Displays.DisplayMeow;
using MEC;
using HintServiceMeow.Core.Extension;
using HintServiceMeow.Core.Utilities;
using System.Reflection;
using Exiled.Events.EventArgs.Player;
using Exiled.API.Features.Items;
using Exiled.API.Extensions;

namespace KE.Items.Features
{
    public abstract class KECustomKeycard : CustomKeycard
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
