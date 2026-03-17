using Exiled.API.Features;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Features.Abilities;
using KE.CustomRoles.API.Interfaces.Ability;
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
    public abstract class EgoAbility : NeedCompAbility<EGO>, IConditional
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

        protected static bool CheckItem(Player player)
        {
            return player.CurrentItem is null || player.CurrentItem.Type != ItemType.SCP1509;
        }

        public bool CheckCondition(Player player)
        {
            return CanLaunchAbility(player, out _);
        }

        public enum NeedActive
        {
            NeedActive,
            NeedNotActive,
            Either,
        }
    }
}
