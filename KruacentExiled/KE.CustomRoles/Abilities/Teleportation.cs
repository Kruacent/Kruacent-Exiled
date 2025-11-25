using Exiled.API.Extensions;
using Exiled.API.Features;
using KE.CustomRoles.API.Features;
using MapGeneration;
using System.Linq;
using UnityEngine;

namespace KE.CustomRoles.Abilities
{
    public class Teleportation : KEAbilities
    {
        public override string Name { get; } = "Teleportation";
        public override string PublicName { get; } = "Teleportation";

        public override string Description { get; } = $"Tu perds {Damage} HP/téléportation, ne peux pas être utilisé dans les ascenseurs";


        public override float Cooldown { get; } = 130f;
        public static float Damage { get; set; } = 60;

        protected override void AbilityUsed(Player player)
        {
            if(!SetPosition.TryGetTarget(player, out Vector3 target))
            {
                MainPlugin.ShowEffectHint(player, "no target selected");
                return;
            }


            
            if(Lift.Get(target) is not null)
            {
                MainPlugin.ShowEffectHint(player, "can't teleport in elevator");
                return;
            }


            if (target.GetZone() == FacilityZone.LightContainment && Map.IsLczDecontaminated)
            {
                MainPlugin.ShowEffectHint(player, "target in LCZ while LCZ is decontaminated.");
                return;
            }

            if (target.GetZone() != player.Zone.GetZone())
            {
                MainPlugin.ShowEffectHint(player, "target is not in the same zone as you.");
                return;
            }

            player.Hurt(Damage, "You have drained your health.");

            if(UnityEngine.Random.Range(1, 101) < 5)
            {
                if (Player.List.Count() > 1)
                {
                    player.Teleport(Player.List.Where(p => p != player).GetRandomValue());
                }
            }
            else
            {
                player.Position = target;
            }
        }
    }
}