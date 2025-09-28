using Exiled.API.Extensions;
using Exiled.API.Features;
using KE.CustomRoles.API.Features;
using MapGeneration;
using System.Linq;
using UnityEngine;

namespace KE.CustomRoles.Abilities
{
    public class Teleport : KEAbilities
    {
        public override string Name { get; } = "Teleportation";

        public override string Description { get; } = "";

        public override int Id => 2008;

        public override float Cooldown { get; } = 120f;

        protected override void AbilityUsed(Player player)
        {
            if(!SelectPosition.TryGetTarget(player, out Vector3 target))
            {
                //show hint
                Log.Info("no target selected");
                return;
            }

            if (target.GetZone() == FacilityZone.LightContainment && Map.IsLczDecontaminated)
            {
                Log.Info("target in LCZ while LCZ is decontaminated.");
                return;
            }

            if (player.Health > 20)
            {
                player.Health -= 20;
            }
            else
            {
                player.Kill("You are dead.");
            }

            if(UnityEngine.Random.Range(1, 101) < 5)
            {
                if (Player.List.Count() > 1)
                {
                    player.Teleport(Player.List.Where(p => p != player).ToList().GetRandomValue());
                    return;
                } else
                {
                    Log.Info("no other player to teleport to");
                    return;
                }
            }
            
            player.Position = target;
        }
    }
}