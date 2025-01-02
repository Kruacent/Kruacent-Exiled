using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using InventorySystem.Items.Usables.Scp330;
using PlayerRoles;
using PluginAPI.Core.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.CR
{
    [CustomRole(RoleTypeId.ClassD)]
    internal class Enfant : Exiled.CustomRoles.API.Features.CustomRole
    {
        public override string Name { get; set; } = "enfant";
        public override string Description { get; set; } = "Tu es un <color=#FFC0CB>Enfant</color> \ndo not the kid \ntu commences avec un bonbon arc-en-ciel \n t'es un peu plus petit que la normal";
        public override uint Id { get; set; } = 1040;
        public override string CustomInfo { get; set; } = "Enfant";
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.ClassD;
        public override bool KeepRoleOnDeath { get ; set ; } = true;
        public override bool KeepRoleOnChangingRole { get; set; } = true;
        public override bool IgnoreSpawnSystem { get; set; } = true;

        public override float SpawnChance { get; set; } = 100;
        public override Vector3 Scale { get ; set; } = new Vector3(1, 0.75f, 1);

    }
}
