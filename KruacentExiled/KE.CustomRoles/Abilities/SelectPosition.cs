using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.Abilities
{
    [CustomAbility]
    public class SelectPosition : ActiveAbility
    {
        public override string Name { get; set; } = "SetPosition";

        public override string Description { get; set; } = "Select the current position for another ability";

        public override float Duration { get; set; } = 0f;

        public override float Cooldown { get; set; } = 0;


        private static Dictionary<Player, Vector3> SelectedTarget = new();

        protected override void AbilityUsed(Player player)
        {
            
            if (SelectedPlayers.Contains(player))
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
