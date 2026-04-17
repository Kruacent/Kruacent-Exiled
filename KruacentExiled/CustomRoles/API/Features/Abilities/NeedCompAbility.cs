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
    public abstract class NeedCompAbility<T> : KEAbilities where T : BaseCompAbility
    {


        protected virtual bool AddCompIfMissing { get; } = false;

        protected sealed override bool AbilityUsed(Player player)
        {
            bool result = CanLaunchAbility(player,out T comp);
            if (result)
            {
                result = LaunchedAbility(player,comp);
            }
            return result;
        }

        protected abstract bool LaunchedAbility(Player player, T component);

        public virtual bool CanLaunchAbility(Player player, out T component)
        {
            if(!player.GameObject.TryGetComponent(out component))
            {
                if (AddCompIfMissing)
                {
                    component = player.GameObject.AddComponent<T>();
                }
                else
                {
                    return false;
                }
            }

            return component.Active;
        }

    }
}
