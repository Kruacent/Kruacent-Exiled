using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.CustomRoles.API.Features;
using InventorySystem.Configs;
using KE.Utils.Display;
using KE.Utils.Display.Enums;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.API
{
    public abstract class KECustomRole : CustomRole
    {
        public override bool IgnoreSpawnSystem { get; set; } = true;
        protected override void ShowMessage(Exiled.API.Features.Player player)
        {

            string show = $"<b>{Name}</b>\n {Description}";

            RueIHint r = new(HPosition.Left, VPosition.CustomItem, show, Exiled.CustomRoles.CustomRoles.Instance.Config.GotRoleHint.Duration);
            DisplayPlayer.Get(player).Hint(r);
        }

        public override void AddRole(Exiled.API.Features.Player player)
        {
            Exiled.API.Features.Player player2 = player;
            Log.Debug(Name + ": Adding role to " + player2.Nickname + ".");
            TrackedPlayers.Add(player2);
            if (Role != RoleTypeId.None)
            {
                if (KeepPositionOnSpawn)
                {
                    if (KeepInventoryOnSpawn)
                    {
                        player2.Role.Set(Role, SpawnReason.ForceClass, RoleSpawnFlags.None);
                    }
                    else
                    {
                        player2.Role.Set(Role, SpawnReason.ForceClass, RoleSpawnFlags.AssignInventory);
                    }
                }
                else if (KeepInventoryOnSpawn && player2.IsAlive)
                {
                    player2.Role.Set(Role, SpawnReason.ForceClass, RoleSpawnFlags.None);
                }
                else
                {
                    player2.Role.Set(Role, SpawnReason.ForceClass, RoleSpawnFlags.AssignInventory);
                }
            }

            Timing.CallDelayed(0.25f, delegate
            {
                if (!KeepInventoryOnSpawn)
                {
                    Log.Debug(Name + ": Clearing " + player2.Nickname + "'s inventory.");
                    player2.ClearInventory();
                }

                foreach (string item in Inventory)
                {
                    Log.Debug(Name + ": Adding " + item + " to inventory.");
                    TryAddItem(player2, item);
                }

                if (Ammo.Count > 0)
                {
                    Log.Debug(Name + ": Adding Ammo to " + player2.Nickname + " inventory.");
                    AmmoType[] values = EnumUtils<AmmoType>.Values;
                    foreach (AmmoType ammoType in values)
                    {
                        if (ammoType != 0)
                        {
                            player2.SetAmmo(ammoType, (ushort)(Ammo.ContainsKey(ammoType) ? ((Ammo[ammoType] == ushort.MaxValue) ? InventoryLimits.GetAmmoLimit(ammoType.GetItemType(), player2.ReferenceHub) : Ammo[ammoType]) : 0));
                        }
                    }
                }
            });
            Log.Debug(Name + ": Setting health values.");
            player2.Health = MaxHealth;
            player2.MaxHealth = MaxHealth;
            player2.Scale = Scale;
            Vector3 spawnPosition = GetSpawnPosition();
            if (spawnPosition != Vector3.zero)
            {
                player2.Position = spawnPosition;
            }

            Log.Debug(Name + ": Setting player info");
            player2.CustomInfo = player2.CustomName + "\n" + CustomInfo;
            player2.InfoArea &= ~(PlayerInfoArea.Nickname | PlayerInfoArea.Role);
            if (CustomAbilities != null)
            {
                foreach (CustomAbility customAbility in CustomAbilities)
                {
                    customAbility.AddAbility(player2);
                }
            }

            ShowMessage(player2);
            ShowBroadcast(player2);
            RoleAdded(player2);
            player2.UniqueRole = Name;
            player2.TryAddCustomRoleFriendlyFire(Name, CustomRoleFFMultiplier);
            if (string.IsNullOrEmpty(ConsoleMessage))
            {
                return;
            }

            StringBuilder stringBuilder = StringBuilderPool.Pool.Get();
            stringBuilder.AppendLine(Name);
            stringBuilder.AppendLine(Description);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(ConsoleMessage);
            List<CustomAbility> customAbilities = CustomAbilities;
            if (customAbilities != null && customAbilities.Count > 0)
            {
                stringBuilder.AppendLine(AbilityUsage);
                stringBuilder.AppendLine("Your custom abilities are:");
                for (int i = 1; i < CustomAbilities.Count + 1; i++)
                {
                    stringBuilder.AppendLine($"{i}. {CustomAbilities[i - 1].Name} - {CustomAbilities[i - 1].Description}");
                }

                stringBuilder.AppendLine("You can keybind the command for this ability by using \"cmdbind .special KEY\", where KEY is any un-used letter on your keyboard. You can also keybind each specific ability for a role in this way. For ex: \"cmdbind .special g\" or \"cmdbind .special bulldozer 1 g\"");
            }

            player2.SendConsoleMessage(StringBuilderPool.Pool.ToStringReturn(stringBuilder), "green");
        }
    }
}
