using Exiled.API.Features;
using KruacentExiled.CustomRoles.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.CustomRoles.API.Features.Abilities
{
    public abstract class ToggleableAbility : KEAbilities
    {

        public abstract Color ColorOn { get; }
        private string colorOn = string.Empty;
        public abstract Color ColorOff { get; }
        private string colorOff = string.Empty;

        public abstract bool GetState(Player player);
        
        protected override void Gui(StringBuilder sb, Player player)
        {
            sb.Append("<color=#");
            if (GetState(player))
            {

                if (colorOn == string.Empty)
                {
                    colorOn = ColorUtility.ToHtmlStringRGB(ColorOn);
                }
                sb.Append(colorOn);
            }
            else
            {
                if (colorOff == string.Empty)
                {
                    colorOff = ColorUtility.ToHtmlStringRGB(ColorOff);
                }
                sb.Append(colorOff);
            }
            sb.Append(">");
            AbilityGui(sb, player);
            GuiReady(sb, player);
            GuiArrow(sb, player);
            sb.Append("</color>");
        }



    }
}
