using Exiled.API.Features;
using KE.CustomRoles.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.Abilities.FireAbilities
{
    public abstract class FireAbility : KEAbilities
    {        
        public abstract int Cost { get; }
        public bool Exclusive => false;
        protected sealed override bool AbilityUsed(Player player)
        {
            bool result = CanLaunchAbility(player);
            if (result)
            {
                result= LaunchedAbility(player);
            }

            


            return result;
        }

        protected virtual bool LaunchedAbility(Player player)
        {
            return true;
        }

        public bool CanLaunchAbility(Player player)
        {
            if (!player.GameObject.TryGetComponent<FireStat>(out var stat) && Exclusive)
            {
                Log.Warn($"Exclusive ability ({Name}) used by a player ({player}) without the proper stat");
                return false;
            }

            if (stat is not null)
            {
                Log.Debug(stat.CurValue);
                if (stat.CurValue < Cost)
                {
                    ShowAbilityHint(player, "not enough " + stat.Name);

                    return false;
                }
                stat.AddAmount(-Cost);
            }

            return true;

        }



    }
}
