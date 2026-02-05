using Exiled.API.Features;
using KE.CustomRoles.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.Abilities.RedMist
{
    public class ToggleEGO : KEAbilities
    {
        public override string Name { get; } = "ToggleEGO";
        public override string PublicName { get; } = "Toggle E.G.O.";

        public override string Description { get; } = "Manifest your E.G.O, gain powerful buff but rapid health drain.\nCan be deactivated anytime";

        public override float Cooldown { get; } = 0f;



        protected override bool AbilityUsed(Player player)
        {




            if(!player.GameObject.TryGetComponent<EGO>(out var ego))
            {
                player.GameObject.AddComponent<EGO>();
            }

            ego.ToggleActive();



            return base.AbilityUsed(player);
        }


    }
}
