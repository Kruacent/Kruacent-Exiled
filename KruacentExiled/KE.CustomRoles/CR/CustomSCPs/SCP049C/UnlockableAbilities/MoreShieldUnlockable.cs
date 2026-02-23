using Exiled.API.Features;
using PlayerRoles.PlayableScps.HumeShield;
using System;
using UnityEngine;
using LabPlayer = LabApi.Features.Wrappers.Player;
namespace KE.CustomRoles.CR.CustomSCPs.SCP049C.UnlockableAbilities
{
    internal class MoreShieldUnlockable : UnlockableAbility
    {
        public override byte Tier => 1;

        public override void Grant(ReferenceHub hub)
        {
            LabPlayer lab = LabPlayer.Get(hub);

            lab.MaxHumeShield = 700;
        }

        public override void Remove(ReferenceHub hub)
        {
            LabPlayer lab = LabPlayer.Get(hub);
            IHumeShieldProvider.GetForHub(hub, out _, out var hsMax, out _, out _);

            lab.MaxHumeShield = hsMax;

            lab.HumeShield = Mathf.Min(lab.HumeShield, lab.MaxHumeShield);


        }
    }
}
