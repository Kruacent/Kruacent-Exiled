using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Spawn;
using KE.CustomRoles.API.Features;
using LabApi.Events.Arguments.Scp914Events;
using LabApi.Features.Wrappers;
using PlayerRoles;
using System.Collections.Generic;
using Player = Exiled.API.Features.Player;
using Item = Exiled.API.Features.Items.Item;
using LabPlayer = LabApi.Features.Wrappers.Player;
using Interactables.Interobjects.DoorUtils;
using UnityEngine;
using InventorySystem;
using MEC;
using InventorySystem.Items;
using Exiled.API.Features.Items.Keycards;

namespace KE.CustomRoles.CR.Guard
{
    public class Guard914 : KECustomRole
    {

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Guard of SCP-914",
                    [TranslationKeyDesc] = "You are <b>The</b> <color=#6B6B38>guard of SCP-914</color> \nYou start at SCP-914 \nbut someone tampered with your card\nand also fuck you",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Garde de SCP-914",
                    [TranslationKeyDesc] = "Tu es <b>Le</b> <color=#6B6B38>garde de SCP-914</color> \nTu commences à 914 \nmais on a traffiqué ta carte \net ntm aussi",
                }
            };
        }
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.FacilityGuard;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            DynamicSpawnPoints = new List<DynamicSpawnPoint>()
            {
                new DynamicSpawnPoint()
                {
                    Location = Exiled.API.Enums.SpawnLocationType.Inside914,
                    Chance = 100,
                }
            }
        };

        public override float SpawnChance { get; set; } = 100;

        public override List<string> Inventory { get; set; } = new List<string>()
       {
          $"{ItemType.ArmorLight}",
          $"{ItemType.Radio}",
          $"{ItemType.GrenadeFlash}",
          $"{ItemType.Medkit}",
          $"{ItemType.GunFSP9}",
        };
        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato9, 60}
        };

        public override void Init()
        {
            base.Init();
            storedSerials = new();
        }

        protected override void RoleAdded(Player player)
        {
            Timing.CallDelayed(.25f, delegate
            {
                CreateFakeGuardCard(player);
            });
            base.RoleAdded(player);
        }

        protected override void SubscribeEvents()
        {
            LabApi.Events.Handlers.Scp914Events.ProcessedPlayer += OnProcessedPlayer;
            base.SubscribeEvents();
        }


        protected override void UnsubscribeEvents()
        {
            LabApi.Events.Handlers.Scp914Events.ProcessedPlayer -= OnProcessedPlayer;
            base.UnsubscribeEvents();
        }

        public static KeycardItem CreateFakeGuardCard(LabPlayer player)
        {
            Log.Debug("guard");
            KeycardItem item = KeycardItem.CreateCustomKeycardMetal(
                player, "Guard Keycard?", "Ofc. " + player.Nickname, "SECURITY GAURD", new KeycardLevels(0, 0, 1),
                new Color32(0, 0, 0, 255), new Color32(0, 0, 0, 255), new Color32(255, 255, 255, 255),
                1, "");

            storedSerials.Add(item.Serial);

            return item;
        }

        public static KeycardItem CreateFakeOperativeCard(LabPlayer player)
        {
            Log.Debug("operative");
            KeycardItem item = KeycardItem.CreateCustomKeycardTaskForce(
                player, "MTF operative Keycard?", "Pvt. " + player.Nickname, new KeycardLevels(0, 0, 2),
                new Color32(60, 100, 150, 255), new Color32(157, 136, 43, 255),
                "", 1);
            storedSerials.Add(item.Serial);

            return item;

        }
        public static KeycardItem CreateFakeCaptainCard(LabPlayer player)
        {
            Log.Debug("captain");
            KeycardItem item = KeycardItem.CreateCustomKeycardTaskForce(
                player, "MTF captain Keycard?", "Cpt. " + player.Nickname, new KeycardLevels(0, 0, 3),
                new Color32(35, 50, 150, 255), new Color32(157, 136, 43, 255),
                "", 2);
            storedSerials.Add(item.Serial);

            return item;
        }



        private static HashSet<ushort> storedSerials;
        private void OnProcessedPlayer(Scp914ProcessedPlayerEventArgs ev)
        {
            if (ev.KnobSetting != Scp914.Scp914KnobSetting.Fine) return;
            Player player = ev.Player;
            Item item = Item.Get(player.CurrentItem.Serial);
            if (item is null) return;

            if (storedSerials.Contains(item.Serial))
            {
                ItemBase itemBase = null;
                bool equipped = player.CurrentItem is not null && player.CurrentItem.Serial == item.Serial;
                player.RemoveItem(item);
                storedSerials.Remove(item.Serial);

                if (item is not CustomKeycardItem keycard)
                {
                    Log.Error("not a custom keycard");
                    return;
                }



                switch (keycard.KeycardLevels.Admin)
                {
                    case 1:
                        itemBase = CreateFakeOperativeCard(player).Base;
                        break;
                    case 2:
                        itemBase = CreateFakeCaptainCard(player).Base;
                        break;
                    case 3:
                    default:
                        itemBase = player.Inventory.ServerAddItem(ItemType.KeycardJanitor, ItemAddReason.Scp914Upgrade);
                        break;
                }
                if(equipped)
                {
                    player.Inventory.ServerSelectItem(itemBase.ItemSerial);
                }

                
            }

        }


    }
}
