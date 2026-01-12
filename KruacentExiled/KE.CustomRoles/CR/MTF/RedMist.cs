/*using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.CR.MTF
{
    public class RedMist : KECustomRole, IColor
    {
        public override string Description { get; set; } = "Tu es débuff mais ta force de tir est démultiplié (fais attention à tes balles)";
        public override string PublicName { get; set; } = "RedMist";
        public override int MaxHealth { get; set; } = 175;
        public override RoleTypeId Role { get; set; } = RoleTypeId.NtfSergeant;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public Color32 Color => new(255, 192, 203, 0);
        public override float SpawnChance { get; set; } = 100;

        protected override void GiveInventory(Player player)
        {
            
        }

        protected override void SubscribeEvents()
        {
            
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            
            base.UnsubscribeEvents();
        }

    }
}
*/