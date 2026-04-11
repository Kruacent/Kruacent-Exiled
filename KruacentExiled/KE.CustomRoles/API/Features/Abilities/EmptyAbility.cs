using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.API.Features.Abilities
{
    /// <summary>
    /// an ability used to show text
    /// </summary>
    public abstract class EmptyAbility : KEAbilities
    {

        public sealed override float Cooldown => 1;
        protected override void GuiReady(StringBuilder sb, Player player)
        {
            
        }



    }
}
