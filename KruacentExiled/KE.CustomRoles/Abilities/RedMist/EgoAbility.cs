using Exiled.API.Features;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Features.Abilities;
using KE.CustomRoles.CR.MTF.RedMist;
using KE.Utils.API.Features;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.Abilities.RedMist
{
    public abstract class EgoAbility : NeedCompAbility<EGO>
    {

        protected sealed override bool AddCompIfMissing => false;
        protected abstract NeedActive NeedEGOActive { get; }

        public override bool CanLaunchAbility(Player player, out EGO comp)
        {



            bool active = base.CanLaunchAbility(player,out comp);

            if (CheckItem(player))
            {
                return false;
            }


            if (NeedEGOActive == NeedActive.NeedActive)
            {
                return active;
            }

            if (NeedEGOActive == NeedActive.NeedNotActive)
            {
                return !active;
            }

            return true;
        }


        protected override void Gui(StringBuilder sb, Player player)
        {


            if(CanLaunchAbility(player, out _))
            {
                sb.Append("<color=#FFFFFF>");
            }
            else
            {
                sb.Append("<color=#454545>");
            }

            AbilityGui(sb, player);
            GuiReady(sb, player);
            GuiArrow(sb, player);
            sb.Append("</color>");
        }

        protected static bool CheckItem(Player player)
        {
            return player.CurrentItem is null || player.CurrentItem.Type != ItemType.SCP1509;
        }

        public enum NeedActive
        {
            NeedActive,
            NeedNotActive,
            Either,
        }
    }
}
