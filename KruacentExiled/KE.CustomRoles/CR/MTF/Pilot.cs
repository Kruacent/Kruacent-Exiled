using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.Abilities;
using KE.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.CR.MTF
{
    [CustomRole(RoleTypeId.NtfPrivate)]
    public class Pilot : KECustomRole
    {
        public override string Description { get; set; } = "So I haveth a Laser Pointere";
        public override uint Id { get; set; } = 1088;
        public override string PublicName { get; set; } = "Pilot";
        public override int MaxHealth { get; set; } = 75;
        public override RoleTypeId Role { get; set; } = RoleTypeId.NtfPrivate;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;

        public override float SpawnChance { get; set; } = 100;

        public override List<string> Inventory { get; set; } = new List<string>()
       {
          $"{ItemType.GunCrossvec}",
          $"{ItemType.KeycardMTFOperative}",
          $"{ItemType.Radio}",
          $"{ItemType.ArmorCombat}",
          $"{ItemType.Medkit}"
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
          { AmmoType.Nato9, 100}
        };

        public override HashSet<Type> Abilities { get; } = new()
        {
            typeof(SelectPosition),
            typeof(Airstrike)
        };


    }
}
