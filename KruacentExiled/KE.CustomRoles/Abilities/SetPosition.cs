using Exiled.API.Features;
using KE.CustomRoles.API.Features;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.Abilities
{
    public class SetPosition : KEAbilities
    {
        public override string Name { get; } = "SetPosition";
        public override string PublicName { get; } = "Set Position";

        public override string Description { get;  } = "Select the current position for another ability";
        public override float Cooldown { get; } = 5f;

        private static Dictionary<Player, Vector3> SelectedTarget = new();

        protected override bool AbilityUsed(Player player)
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
            return base.AbilityUsed(player);

        }

        public static bool TryGetTarget(Player p, out Vector3 target)
        {
            return SelectedTarget.TryGetValue(p, out target);

        }


    }
}
