using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Pools;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Configs;
using KE.CustomRoles.API.Interfaces;
using KE.Utils.API.Displays.DisplayMeow;
using LiteNetLib4Mirror.Open.Nat;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.API.Features
{

    public abstract class GlobalCustomRole : KECustomRole
    {
        public sealed override RoleTypeId Role { get; set; } = RoleTypeId.None;
        public abstract SideEnum Side { get; set; }
        public override bool KeepInventoryOnSpawn { get; set; } = true;
        public override bool RemovalKillsPlayer => false;


        
        public override void AddRole(Player player)
        {
            SideEnum side = SideClass.Get(player.Role.Side);
            
            if (side != Side)
            {
                Log.Error($"tried to give a global custom role to a player in the wrong side ({side} instead of {Side})");
                return;
            }
            Log.Debug($"{Name}: Adding role to {player.Nickname}.");
            TrackedPlayers.Add(player);



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
            player.MaxHealth *= MaxHealthMultiplicator;
            player.Health = player.MaxHealth;
            player.Scale = Scale;

            Vector3 position = GetSpawnPosition();
            if (position != Vector3.zero)
            {
                player.Position = position;
            }

            Log.Debug($"{Name}: Setting player info");

            player.CustomInfo = $"{player.CustomName}\n{CustomInfo}";
            player.InfoArea &= ~(PlayerInfoArea.Role | PlayerInfoArea.Nickname);

            KEAbilities.TryRemoveFromPlayer(player);
            if (Abilities != null)
            {
                foreach (int abilityId in Abilities)
                {
                    KEAbilities.TryAddToPlayer(abilityId, player);
                }
            }


            ShowMessage(player);
            RoleAdded(player);
            player.UniqueRole = Name;
        }
        public override void RemoveRole(Player player)
        {
            

            if (!TrackedPlayers.Contains(player))
            {
                return;
            }

            Log.Debug(Name + ": Removing role from " + player.Nickname + $"({player.Id})");
            TrackedPlayers.Remove(player);
            player.CustomInfo = string.Empty;
            player.InfoArea |= PlayerInfoArea.Nickname | PlayerInfoArea.Role;
            player.Scale = Vector3.one;
            KEAbilities.TryRemoveFromPlayer(player);

            RoleRemoved(player);
            player.UniqueRole = string.Empty;
            player.TryRemoveCustomeRoleFriendlyFire(Name);
            if (RemovalKillsPlayer)
            {
                //player.Role.Set(RoleTypeId.Spectator);
            }
            Log.Debug(Name + ": finish Removing role from " + player.Nickname + $"({player.Id})");

        }




        public sealed override int MaxHealth { get; set; }
        public virtual float MaxHealthMultiplicator { get; set; } = 1;

        protected override void ShowMessage(Player player)
        {
            StringBuilder sb = StringBuilderPool.Pool.Get();
            sb.Append("<b>");
            IColor color = this as IColor;
            if (color != null)
            {
                sb.Append("<color=#");
                sb.Append(ColorUtility.ToHtmlStringRGB(color.Color));
                sb.Append(">");
            }


            sb.Append(PublicName);

            if (color != null)
            {
                sb.Append("</color>");
            }

            if (player.IsScp)
            {
                sb.Append(player.Role.Name);
            }


            sb.AppendLine("</b>");

            if (MainPlugin.SettingHandler.GetDescriptionsSettings(player))
            {
                sb.AppendLine(Description);
            }


            float delay = MainPlugin.SettingHandler.GetTime(player);

            DisplayHandler.Instance.AddHint(MainPlugin.CRHint, player, sb.ToString(), delay);
            StringBuilderPool.Pool.Return(sb);
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
        public static SideEnum Get(Side side)
        {
            return side switch
            {
                Side.Scp => SideEnum.SCP,
                Side.Tutorial or Side.Mtf or Side.ChaosInsurgency => SideEnum.Human,
                Side.None => SideEnum.None,
                _ => SideEnum.None,
            };
        }
    }

}
