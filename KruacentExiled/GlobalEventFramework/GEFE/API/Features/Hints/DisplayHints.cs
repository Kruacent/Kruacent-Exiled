using Exiled.API.Features;
using KE.Utils.API.Displays.DisplayMeow;
using KruacentExiled.GlobalEventFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.GlobalEventFramework.GEFE.API.Features.Hints
{
    public static class DisplayHints
    {

        public static void AddHintEffect(Player player,string text,float delay)
        {
            DisplayHandler.Instance.AddHint(MainPlugin.GEEffect, player, text, delay);
        }
    }
}
