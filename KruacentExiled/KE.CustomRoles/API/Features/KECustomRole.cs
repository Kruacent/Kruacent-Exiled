using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.Commands.PluginManager;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Configs;
using KE.Utils.API.Displays.DisplayMeow;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace KE.CustomRoles.API.Features
{
    public abstract class KECustomRole : CustomRole
    {
        public sealed override bool IgnoreSpawnSystem { get; set; } = true;
        protected override void ShowMessage(Player player)
        {

            string show = $"<b>{Name}</b>\n {Description}";

            //todo settings
            float delay = 20;

            DisplayHandler.Instance.AddHint(MainPlugin.CRHint, player, show, delay);
        }



        protected void ShowEffectHint(Player player, string text)
        {
            //todo settings
            float delay = 20;
            DisplayHandler.Instance.AddHint(MainPlugin.CREffect, player, text, delay);
        }

        public override void AddRole(Player player)
        {
            Player player2 = player;
            Log.Debug(Name + ": Adding role to " + player2.Nickname + ".");
            
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
            TrackedPlayers.Add(player2);

            Timing.CallDelayed(0.25f, delegate
            {
                if (!KeepInventoryOnSpawn)
                {
                    player2.ClearInventory();
                }

                foreach (string item in Inventory)
                {
                    TryAddItem(player2, item);
                }

                if (Ammo.Count > 0)
                {
                    AmmoType[] values = EnumUtils<AmmoType>.Values;
                    foreach (AmmoType ammoType in values)
                    {
                        if (ammoType != 0)
                        {
                            player2.SetAmmo(ammoType, (ushort)(Ammo.ContainsKey(ammoType) ? Ammo[ammoType] == ushort.MaxValue ? InventoryLimits.GetAmmoLimit(ammoType.GetItemType(), player2.ReferenceHub) : Ammo[ammoType] : 0));
                        }
                    }
                }
            });
            player2.Health = MaxHealth;
            player2.MaxHealth = MaxHealth;
            player2.Scale = Scale;
            Vector3 spawnPosition = GetSpawnPosition();
            if (spawnPosition != Vector3.zero)
            {
                player2.Position = spawnPosition;
            }

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

        /// <summary>
        /// The chance of having this role NOT the chance to have a role
        /// </summary>
        public override abstract float SpawnChance { get; set; }
        


    }
}
