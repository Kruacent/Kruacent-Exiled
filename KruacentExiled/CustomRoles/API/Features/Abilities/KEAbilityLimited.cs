using Exiled.API.Features;
using System.Collections.Generic;
using System.Text;

namespace KruacentExiled.CustomRoles.API.Features.Abilities
{
    public abstract class KEAbilityLimited : KEAbilities
    {

        public abstract int Uses { get; }

        private Dictionary<Player, int> uses =new Dictionary<Player, int>();

        public override void AddAbility(Player player)
        {
            uses[player] = Uses;
            base.AddAbility(player);
        }
        protected sealed override bool AbilityUsed(Player player)
        {
            bool result = false;
            int use = GetUse(player);
            if (use > 0)
            {
                uses[player]--;
                result = LaunchedAbility(player);
            }
            if(use <= 0)
            {
                RemoveAbility(player);
            }

            return result;
        }

        protected virtual bool LaunchedAbility(Player player)
        {
            return true;
        }


        public int GetUse(Player player)
        {
            if(!uses.TryGetValue(player,out int result))
            {
                return -1;
            }

            return result;
        }



        protected override void AbilityGui(StringBuilder sb,Player player)
        {
            base.AbilityGui(sb, player);
            GuiUses(sb, player);
        }


        protected void GuiUses(StringBuilder sb, Player player)
        {
            sb.Append("(");
            sb.Append(uses[player]);
            sb.Append(" left)");
            sb.Append(" ");
        }
    }
}
