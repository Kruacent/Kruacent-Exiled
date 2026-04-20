using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Keycards;
using KruacentExiled.CustomRoles.API.Features;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KruacentExiled.CustomRoles.CR.Scientist
{
    public class ZoneManager : KECustomRole
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Zone Manager",
                    [TranslationKeyDesc] = "Open all of the checkpoint to get a better card!",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Zone Manager",
                    [TranslationKeyDesc] = "Incroyable tu peux avoir une promotion alors fais ton boulot et ouvre tous ces checkpoints et tu pourras sortir d'ici!",
                },
                ["legacy"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Zone Manager",
                    [TranslationKeyDesc] = "Incroyable tu peux avoir une promotion alors fais ton boulot et ouvre tous ces checkpoints et tu pourras sortir d'ici!",
                }
            };
        }
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scientist;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public Color32 Color => new Color32(4, 52, 50, 0);


        public override float SpawnChance { get; set; } = 100;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            DynamicSpawnPoints = new List<DynamicSpawnPoint>()
            {
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.Inside127Lab,
                    Chance = 100,
                }
            }
        };


        public override HashSet<string> Abilities { get; } = new HashSet<string>()
        {
            "NumberCheckpoints"
        };

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            $"{ItemType.Medkit}",
            $"{ItemType.Adrenaline}",
            $"{ItemType.KeycardZoneManager}"
        };
        public static readonly IReadOnlyCollection<DoorType> DoorToOpen = new List<DoorType>()
        {
            DoorType.CheckpointLczA,
            DoorType.CheckpointLczB,
            DoorType.CheckpointEzHczA,
            DoorType.CheckpointEzHczB
        }.AsReadOnly();


        private static Dictionary<Player, HashSet<DoorType>> objectives = new Dictionary<Player, HashSet<DoorType>>();
        private static Dictionary<Player, bool> flag = new Dictionary<Player, bool>();

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
            objectives.Add(player, new HashSet<DoorType>(DoorToOpen));
            flag.Add(player, false);
        }
        protected override void RoleRemoved(Player player)
        {
            objectives.Remove(player);
            flag.Remove(player);
        }


        public static int GetNumberCheckpoints(Player player)
        {
            if (!objectives.ContainsKey(player))
            {
                return -1;
            }

            return objectives[player].Count;
        }

        private void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            Player player = ev.Player;
            if (!Check(player)) return;
            objectives[player].Remove(ev.Door.Type);

            if (CheckDoors(player))
            {
                KEAbilities.Remove("NumberCheckpoints", player);
                bool equipped;

                if(player.CurrentItem != null)
                {
                    equipped = player.CurrentItem.Type == ItemType.KeycardZoneManager;
                }
                else
                {
                    equipped = false;
                }

                Item zoneKeycard = player.Items.Where(p => p.Type == ItemType.KeycardZoneManager).ElementAtOrDefault(0);
                zoneKeycard?.Destroy();

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
