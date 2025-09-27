using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Keycards;
using KE.CustomRoles.API.Features;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KE.CustomRoles.CR.Scientist
{
    [CustomRole(RoleTypeId.Scientist)]
    internal class ZoneManager : KECustomRole
    {
        public override string Description { get; set; } = "Incroyable tu peux avoir une promotion alors fais ton boulot et ouvre tous ces checkpoint et tu pourras sortir d'ici";
        public override uint Id { get; set; } = 1044;
        public override string PublicName { get; set; } = "Zone Manager";
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scientist;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;


        public override float SpawnChance { get; set; } = 100;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            DynamicSpawnPoints = new List<DynamicSpawnPoint>()
            {
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.InsideHidLab,
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
        public static readonly List<DoorType> DoorToOpen = new()
        {
                DoorType.CheckpointLczA,
                DoorType.CheckpointLczB,
                DoorType.CheckpointEzHczA,
                DoorType.CheckpointEzHczB
        };


        private static Dictionary<Player, HashSet<DoorType>> objectives = new();
        private static Dictionary<Player, bool> flag = new();

        protected override void SubscribeEvents()
        {

            Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
            base.UnsubscribeEvents();
        }

        protected override void RoleAdded(Player player)
        {
            objectives.Add(player, new(DoorToOpen));
            flag.Add(player, false);
        }
        protected override void RoleRemoved(Player player)
        {
            objectives.Remove(player);
            flag.Remove(player);
        }


        private void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            Player player = ev.Player;
            if (!Check(player)) return;
            objectives[player].Remove(ev.Door.Type);

            if (CheckDoors(player))
            {
                bool equipped = player.CurrentItem.Type == ItemType.KeycardFacilityManager;
                Item zoneKeycard = player.Items.Where(p => p.Type == ItemType.KeycardFacilityManager).ElementAtOrDefault(0);
                if (zoneKeycard != null)
                {
                    zoneKeycard.Destroy();
                }

                player.AddItem(ItemType.Radio);
                flag[player] = true;
                Item engineerKeycard = player.AddItem(ItemType.KeycardFacilityManager);
                if (equipped)
                {
                    player.CurrentItem = engineerKeycard;
                }
            }

        }

        private bool CheckDoors(Player p)
        {
            if (flag[p]) return false;
            return objectives[p].Count == 0;
        }

    }
}
