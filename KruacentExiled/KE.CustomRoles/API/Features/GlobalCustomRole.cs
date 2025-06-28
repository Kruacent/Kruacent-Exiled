using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Pools;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Configs;
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

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.SpawningRagdoll += InternalSpawningRagdoll;

            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.SpawningRagdoll -= InternalSpawningRagdoll;

            base.UnsubscribeEvents();
        }

        private void InternalSpawningRagdoll(SpawningRagdollEventArgs ev)
        {

            //letting this event go disconnect everyone idk why
            if (Check(ev.Player))
            {
                ev.IsAllowed = false;

                Ragdoll.CreateAndSpawn(ev.Role, ev.Player.Nickname, ev.DamageHandlerBase, ev.Position, ev.Rotation);
            }
        }



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
            foreach(Player p in TrackedPlayers.ToList())
            {
                if(p.Id == player.Id)
                {
                    Log.Debug("he is in the tracked");
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

            if (CustomAbilities != null)
            {
                foreach (CustomAbility ability in CustomAbilities)
                    ability.AddAbility(player);
            }

            ShowMessage(player);
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
        public override void RemoveRole(Player player)
        {
            

            Log.Debug($"player in {Name}:");
            foreach (Player p in TrackedPlayers.ToList())
            {
                if (p.Id == player.Id)
                {
                    Log.Debug($"{p.Id} is in");
                }
            }

            if (!TrackedPlayers.Contains(player))
            {
                return;
            }

            Log.Debug(Name + ": Removing role from " + player.Nickname + $"({player.Id})");
            TrackedPlayers.Remove(player);
            player.CustomInfo = string.Empty;
            player.InfoArea |= PlayerInfoArea.Nickname | PlayerInfoArea.Role;
            player.Scale = Vector3.one;
            if (CustomAbilities != null)
            {
                foreach (CustomAbility customAbility in CustomAbilities)
                {
                    customAbility.RemoveAbility(player);
                }
            }

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
            string show;
            if (player.IsScp)
            {
                show = $"<b>{Name} {player.Role.Name}</b>\n {Description}";
            }
            else
            {
                show = $"<b>{Name}</b>\n {Description}";
            }


            //todo settings
            float delay = 20;

            DisplayHandler.Instance.AddHint(MainPlugin.CRHint, player, show, delay);
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
