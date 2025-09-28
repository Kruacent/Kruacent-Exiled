using Exiled.API.Features.Attributes;
using InventorySystem.Items.Usables.Scp330;
using KE.CustomRoles.API.Features;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.CR.ClassD
{
    public class Enfant : KECustomRole
    {
        public override string Description { get; set; } = "Tu es un <color=#FFC0CB>Enfant</color> \ndo not the kid \ntu commences avec un bonbon arc-en-ciel \n t'es un peu plus petit que la normal";
        public override uint Id { get; set; } = 1041;
        public override string PublicName { get; set; } = "Enfant";
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.ClassD;
        public override bool KeepRoleOnDeath { get; set; } = true;
        public override bool KeepRoleOnChangingRole { get; set; } = true;

        public override float SpawnChance { get; set; } = 100;
        public override Vector3 Scale { get; set; } = new Vector3(1, 0.75f, 1);

        public override List<string> Inventory { get; set; } = new List<string>()
       {
          $"{CandyKindID.Rainbow}"
        };
    }
    
}
