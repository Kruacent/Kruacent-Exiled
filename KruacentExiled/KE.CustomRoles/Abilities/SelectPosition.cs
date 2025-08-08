using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.Abilities
{
    public class SelectPosition : KEAbilities
    {
        public override string Name { get; } = "SetPosition";

        public override string Description { get;  } = "Select the current position for another ability";

        public override int Id => 2000;
        public override float Cooldown { get; } = 5f;

        private static Dictionary<Player, Vector3> SelectedTarget = new();

        protected override void AbilityUsed(Player player)
        {
            


            if (SelectedTarget.ContainsKey(player))
            {
                SelectedTarget[player] = player.Position;
            }
            else
            {
                SelectedTarget.Add(player, player.Position);
            }

            Log.Info("set position at " +player.Position);
            

        }

        public static bool TryGetTarget(Player p, out Vector3 target)
        {
            return SelectedTarget.TryGetValue(p, out target);

        }


    }
}
