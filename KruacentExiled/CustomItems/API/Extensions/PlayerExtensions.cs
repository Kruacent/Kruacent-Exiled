using Exiled.API.Features;
using KruacentExiled.CustomItems.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.CustomItems.API.Extensions
{
    public static class PlayerExtensions
    {

        public static void ItemEffectHint(this Player player, string text)
        {
            KECustomItem.ItemEffectHint(player, text);
        }
    }
}
