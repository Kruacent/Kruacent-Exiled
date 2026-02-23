using Exiled.API.Features;
using Exiled.API.Features.Roles;
using PlayerRoles.FirstPersonControl;
using System;
using LabPlayer = LabApi.Features.Wrappers.Player;
namespace KE.CustomRoles.CR.CustomSCPs.SCP049C.UnlockableAbilities
{
    internal class JumpUnlockable : UnlockableAbility
    {
        public override byte Tier => 5; //1

        public override void Grant(ReferenceHub hub)
        {
            Player lab = Player.Get(hub);


            if(lab.Role is FpcRole role)
            {
                role.Gravity = UnityEngine.Vector3.up * (-3);
            }
        }

        public override void Remove(ReferenceHub hub)
        {
            Player lab = Player.Get(hub);


            if (lab.Role is FpcRole role)
            {
                role.Gravity = FpcGravityController.DefaultGravity;
            }
        }
    }
}
