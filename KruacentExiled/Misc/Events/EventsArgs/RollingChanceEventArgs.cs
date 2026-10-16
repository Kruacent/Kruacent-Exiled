using System;
using Exiled.API.Features;
using UnityEngine;

namespace KruacentExiled.Misc.Events
{
    public class RollingChanceEventArgs : EventArgs
    {
        public float Chance { get; set; }
        public bool IsBeneficial { get; }

        public Player TargetPlayer { get; }
        public bool? ForcedResult { get; set; } = null;

        public RollingChanceEventArgs(float chance, Player targetPlayer, bool isBeneficial)
        {
            Chance = chance;
            TargetPlayer = targetPlayer;
            IsBeneficial = isBeneficial;
        }
    }
}