using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.API.Features
{
    public abstract class KECustomRoleMultipleRole : KECustomRole
    {

        public override string Name
        {
            get
            {
                return  "MULTIPLE_" + InternalName.RemoveSpaces();
            }
        }
        public override sealed RoleTypeId Role => RoleTypeId.None;
        public override sealed int MaxHealth { get; set; }= -1;

        public abstract HashSet<RoleTypeId> Roles { get; }





        protected override void AttributeHealth(Player player)
        {
            
        }



        public override bool IsAvailable(Player player)
        {
            if (CurrentNumberOfSpawn >= Limit) return false;
            return Roles.Contains(player.Role);
        }

    }
}
