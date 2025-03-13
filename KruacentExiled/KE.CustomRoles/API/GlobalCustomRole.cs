using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Pools;
using Exiled.CustomRoles.API.Features;
using InventorySystem.Configs;
using LiteNetLib4Mirror.Open.Nat;
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

    public abstract class GlobalCustomRole : KECustomRole
    {
        public override RoleTypeId Role { get; set; } = RoleTypeId.None;
        public abstract SideEnum Side { get; set; }
        public virtual IEnumerable<RoleTypeId> BlacklistedRole { get; } = new List<RoleTypeId>();
        public override void AddRole(Player player)
        {
            if (SideClass.Get(player.Role.Side) != Side) return;
            Log.Debug($"{Name}: Adding role to {player.Nickname}.");
            TrackedPlayers.Add(player);

            if (!BlacklistedRole.Contains(player.Role))
            {
                switch (KeepPositionOnSpawn)
                {
                    case true when KeepInventoryOnSpawn:
                        player.Role.Set(player.Role, SpawnReason.ForceClass, RoleSpawnFlags.None);
                        break;
                    case true:
                        player.Role.Set(player.Role, SpawnReason.ForceClass, RoleSpawnFlags.AssignInventory);
                        break;
                    default:
                        {
                            if (KeepInventoryOnSpawn && player.IsAlive)
                                player.Role.Set(player.Role, SpawnReason.ForceClass, RoleSpawnFlags.UseSpawnpoint);
                            else
                                player.Role.Set(player.Role, SpawnReason.ForceClass, RoleSpawnFlags.All);
                            break;
                        }
                }
            }

            Timing.CallDelayed(
                0.25f,
                () =>
                {
                    if (!KeepInventoryOnSpawn)
                    {
                        Log.Debug($"{Name}: Clearing {player.Nickname}'s inventory.");
                        player.ClearInventory();
                    }

                    foreach (string itemName in Inventory)
                    {
                        Log.Debug($"{Name}: Adding {itemName} to inventory.");
                        TryAddItem(player, itemName);
                    }

                    if (Ammo.Count > 0)
                    {
                        Log.Debug($"{Name}: Adding Ammo to {player.Nickname} inventory.");
                        foreach (AmmoType type in EnumUtils<AmmoType>.Values)
                        {
                            if (type != AmmoType.None)
                                player.SetAmmo(type, Ammo.ContainsKey(type) ? Ammo[type] == ushort.MaxValue ? InventoryLimits.GetAmmoLimit(type.GetItemType(), player.ReferenceHub) : Ammo[type] : (ushort)0);
                        }
                    }
                });

            Log.Debug($"{Name}: Setting health values.");
            player.Health = MaxHealth;
            player.MaxHealth = MaxHealth;
            player.Scale = Scale;

            Vector3 position = GetSpawnPosition();
            if (position != Vector3.zero)
            {
                player.Position = position;
            }

            Log.Debug($"{Name}: Setting player info");

            player.CustomInfo = $"{player.CustomName}\n{CustomInfo}";
            player.InfoArea &= ~(PlayerInfoArea.Role | PlayerInfoArea.Nickname);

            if (CustomAbilities != null)
            {
                foreach (CustomAbility ability in CustomAbilities)
                    ability.AddAbility(player);
            }

            ShowMessage(player);
            ShowBroadcast(player);
            RoleAdded(player);
            player.UniqueRole = Name;
            player.TryAddCustomRoleFriendlyFire(Name, CustomRoleFFMultiplier);

            if (!string.IsNullOrEmpty(ConsoleMessage))
            {
                StringBuilder builder = StringBuilderPool.Pool.Get();

                builder.AppendLine(Name);
                builder.AppendLine(Description);
                builder.AppendLine();
                builder.AppendLine(ConsoleMessage);

                if (CustomAbilities?.Count > 0)
                {
                    builder.AppendLine(AbilityUsage);
                    builder.AppendLine("Your custom abilities are:");
                    for (int i = 1; i < CustomAbilities.Count + 1; i++)
                        builder.AppendLine($"{i}. {CustomAbilities[i - 1].Name} - {CustomAbilities[i - 1].Description}");

                    builder.AppendLine(
                        "You can keybind the command for this ability by using \"cmdbind .special KEY\", where KEY is any un-used letter on your keyboard. You can also keybind each specific ability for a role in this way. For ex: \"cmdbind .special g\" or \"cmdbind .special bulldozer 1 g\"");
                }

                player.SendConsoleMessage(StringBuilderPool.Pool.ToStringReturn(builder), "green");
            }
        }



    }

    public enum SideEnum
    {
        Human,
        SCP,
        None,
    }

    public static class SideClass
    {
        public static SideEnum Get(Exiled.API.Enums.Side side)
        {
            switch (side)
            {
                case Exiled.API.Enums.Side.Scp:
                    return SideEnum.SCP;
                case Exiled.API.Enums.Side.Tutorial:
                case Exiled.API.Enums.Side.Mtf:
                case Exiled.API.Enums.Side.ChaosInsurgency:
                    return SideEnum.Human;
                case Exiled.API.Enums.Side.None: 
                    return SideEnum.None;
            }
            return SideEnum.None;
        }
    }
    
}
