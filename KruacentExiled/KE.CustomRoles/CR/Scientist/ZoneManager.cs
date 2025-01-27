using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using InventorySystem.Items.Keycards;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.CR.Scientist
{
    [CustomRole(RoleTypeId.Scientist)]
    internal class ZoneManager : Exiled.CustomRoles.API.Features.CustomRole
    {
        public override string Name { get; set; } = "ZoneManager";
        public override string Description { get; set; } = "Tu es un <color=#0d7979>Zone Manager</color> \nT'as une carte de zone manager (d'où le nom) \nTu commences à heavy \nBon courage...";
        public override uint Id { get; set; } = 1404;
        public override string CustomInfo { get; set; } = "ZoneManager";
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scientist;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override bool IgnoreSpawnSystem { get; set; } = true;

        public override float SpawnChance { get; set; } = 100;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            DynamicSpawnPoints = new List<DynamicSpawnPoint>()
            {
                new DynamicSpawnPoint()
                {
                    Location = Exiled.API.Enums.SpawnLocationType.InsideHidLower,
                    Chance = 100,
                }
            }
        };

        public override List<string> Inventory { get; set; } = new List<string>()
       {
          $"{ItemType.Medkit}",
          $"{ItemType.Adrenaline}",
          $"{ItemType.KeycardZoneManager}"
        };
    }
}
