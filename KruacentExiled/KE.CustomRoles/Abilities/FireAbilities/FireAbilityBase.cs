using Exiled.API.Extensions;
using Exiled.API.Features;
using KE.CustomRoles.API.Features.Abilities;
using KE.Utils.API.CustomStats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.Abilities.FireAbilities
{
    public abstract class FireAbilityBase : KEAbilityCost
    {        
        public override bool CanLaunchAbility(Player player)
        {

            if(player.GameObject.TryGetComponent<CustomPlayerStat>(out var stat))
            {
                if (stat.TryGetModule<FireStat>(out var firestat))
                {
                    Log.Debug($"cur = {firestat.CurValue} / {Cost}");
                    if (firestat.CurValue > Cost)
                    {
                        Log.Debug("enough");
                        firestat.AddAmount(-Cost);
                        return true;
                    }
                    else
                    {
                        Log.Debug("not enough");
                        return false;
                    }
                }

            }


            Log.Debug("no fire stat");
            return true;

        }



    }
}
