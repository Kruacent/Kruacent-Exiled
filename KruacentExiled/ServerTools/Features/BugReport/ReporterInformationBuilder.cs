using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pools;
using GlobalEventFramework.API.Features;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using KruacentExiled.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace KruacentExiled.ServerTools.Features.BugReport
{
    /// <summary>
    /// Get all of information about the player and round at the report moment
    /// </summary>
    internal static class ReporterInformationBuilder
    {
        // Radius at which player is included in th ereport
        private const float NearbyRadius = 40f;

        public static string Build(Player player)
        {
            StringBuilder info = StringBuilderPool.Pool.Get();

            GetRoundInformation(info);
            GetReporterInformation(info, player);
            GetNearbyPlayerInformation(info, player);

            return StringBuilderPool.Pool.ToStringReturn(info);
        }

        private static void GetRoundInformation(StringBuilder info)
        {
            info.AppendLine("## Round");
            info.AppendLine($"- **Time :** {DateTime.Now}");
            info.AppendLine($"- **State :** {GetRoundState()}");
            info.AppendLine($"- **Time of the round :** {(int)Round.ElapsedTime.TotalMinutes} min {Round.ElapsedTime.Seconds} s");
            info.AppendLine($"- **Map seed :** {Exiled.API.Features.Map.Seed}");
            info.AppendLine($"- **SCP(s) :** {GetCurrentSCP()}");
            info.AppendLine($"- **Number of player connected :** {Player.List.Count}");
            info.AppendLine($"- **Active Global Event :** {GetActiveGlobalEvents()}");
            info.AppendLine($"- **Nuke State :** {GetWarheadState()}");
            info.AppendLine($"- **Friendly Fire :** {YesNo(Server.FriendlyFire)}");
            info.AppendLine($"- **LCZ decontaminated :** {YesNo(Exiled.API.Features.Map.IsLczDecontaminated)}");
            info.AppendLine($"- **Server TPS :** {Server.Tps:0}");
            info.AppendLine($"- **Game version :** {GameCore.Version.VersionString}");
            info.AppendLine($"- **KruacentExiled version :** {KruacentExiled.MainPlugin.Instance.Version}");
            info.AppendLine();
        }

        private static void GetReporterInformation(StringBuilder info, Player player)
        {
            string heldItem;
            if (player.CurrentItem != null)
            {
                heldItem = GetItemName(player.CurrentItem);
            } else {
                heldItem = "nothing";
            }

            info.AppendLine("## Player who report");
            info.AppendLine($"- **Nickname :** {player.Nickname}");
            info.AppendLine($"- **Role :** {player.Role.Type}");
            info.AppendLine($"- **Custom Role :** {GetCustomRoles(player)}");
            info.AppendLine($"- **Alive :** {YesNo(player.IsAlive)}");
            info.AppendLine($"- **HP :** {player.Health:0}/{player.MaxHealth:0}, AHP {player.ArtificialHealth:0}");
            info.AppendLine($"- **Position :** {player.Position}");
            info.AppendLine($"- **Room :** {GetRoomName(player)} ({player.Zone})");
            info.AppendLine($"- **Held Item :** {heldItem}");
            info.AppendLine($"- **Inventory :** {GetInventory(player)}");
            info.AppendLine($"- **Ammos :** {GetAmmo(player)}");
            info.AppendLine($"- **ActiveEffetc :** {GetEffects(player)}");
            info.AppendLine();
        }

        private static void GetNearbyPlayerInformation(StringBuilder info, Player player)
        {
            info.AppendLine($"## Player arround the reporter (less thant {NearbyRadius:0} m)");

            Dictionary<Player, float> playerNearby = new Dictionary<Player, float>();

            foreach (Player p in Player.List)
            {
                if (p == player || !p.IsAlive)
                {
                    continue;
                }

                float distance = Vector3.Distance(p.Position, player.Position);

                if (distance <= NearbyRadius)
                {
                    playerNearby.Add(p, distance);
                }
            }

            if (playerNearby.Count == 0)
            {
                info.AppendLine("nobody");
                info.AppendLine();
                return;
            }

            info.AppendLine("| Nickname | Role | Custom Role | HP | Room | Held Item | Distance |");
            info.AppendLine("|---|---|---|---|---|---|---|");

            foreach (KeyValuePair<Player, float> pair in playerNearby)
            {
                Player other = pair.Key;
                float distance = pair.Value;

                string heldItem = "nothing";
                if (other.CurrentItem != null)
                {
                    heldItem = GetItemName(other.CurrentItem);
                }

                string health = $"{other.Health:0}/{other.MaxHealth:0}";

                info.AppendLine($"| {other.Nickname} | {other.Role.Type} | {GetCustomRoles(other)} | {health} | {GetRoomName(other)} | {heldItem} | {distance:0.0} m |");
            }

            info.AppendLine();
        }

        private static string GetCurrentSCP()
        {
            string scps;
            List<string> scpPlayer = ListPool<string>.Pool.Get();

            foreach (Player p in Player.List)
            {
                if(!SCPTeam.IsSCP(p.ReferenceHub))
                {
                    continue;
                }

                scpPlayer.Add(GetCustomRoleOrRole(p));
            }

            if(scpPlayer.Count == 0)
            {
                scps = "No SCP";
            } else
            {
                scps = string.Join(", ", scpPlayer);
            }

            ListPool<string>.Pool.Return(scpPlayer);

            return scps;
        }

        private static string GetRoundState()
        {
            if (Round.IsLobby)
            {
                return "in lobby (not started yet)";
            }

            if (Round.IsEnded)
            {
                return "round is ended";
            }

            return "round in progress";
        }

        private static string GetWarheadState()
        {
            if (Warhead.IsDetonated)
            {
                return "detonated";
            }

            if (Warhead.IsInProgress)
            {
                return $"in progress ({Warhead.DetonationTimer:0} s remaingin)";
            }

            return "not started yet";
        }

        private static string GetActiveGlobalEvents()
        {
            string geName;
            List<string> names = ListPool<string>.Pool.Get();
            foreach (GlobalEvent globalEvent in GlobalEvent.GlobalEventsList)
            {
                if (globalEvent.IsActive)
                {
                    names.Add(globalEvent.Name);
                }
            }

            if(names.Count == 0)
            {
                geName = "nothing";
            } else
            {
                geName = string.Join(", ", names);
            }

            ListPool<string>.Pool.Return(names);
            return geName;
        }

        private static string GetRoomName(Player player)
        {
            string roomName;

            if(player.CurrentRoom != null)
            {
                roomName = player.CurrentRoom.Type.ToString();
            } else
            {
                roomName = "nothing";
            }

            return roomName;
        }

        private static string GetCustomRoleOrRole(Player player)
        {
            string roleName = GetCustomRoles(player);
            if(roleName == "nothing")
            {
                roleName = player.Role.Type.ToString();
            }

            return roleName;
        }
        
        private static string GetCustomRoles(Player player)
        {
            string roleName;
            List<string> names = ListPool<string>.Pool.Get();

            foreach (KECustomRole role in KECustomRole.Get(player))
            {
                names.Add(role.Name);
            }

            if (names.Count == 0)
            {
                roleName = "nothing";
            } else
            {
                roleName = string.Join(", ", names);
            }

            ListPool<string>.Pool.Return(names);
            return roleName;
        }

        private static string GetItemName(Item item)
        {
            return $"{item.GetName()} ({item.Type})";
        }

        private static string GetInventory(Player player)
        {
            string inventory;
            List<string> items = ListPool<string>.Pool.Get();
            
            foreach (Item item in player.Items)
            {
                items.Add(GetItemName(item));
            }

            if(items.Count == 0)
            {
                inventory = "empty";
            } else
            {
                inventory = string.Join(", ", items);
            }

            ListPool<string>.Pool.Return(items);
            return inventory;
        }

        private static string GetAmmo(Player player)
        {
            string ammo; 
            List<string> ammoList = ListPool<string>.Pool.Get();

            foreach (var pair in player.Ammo)
            {
                if (pair.Value > 0)
                {
                    ammoList.Add($"{pair.Key} x{pair.Value}");
                }
            }

            if (ammoList.Count == 0)
            {
                ammo = "empty";
            } else
            {
                ammo = string.Join(", ", ammoList);
            }

            ListPool<string>.Pool.Return(ammoList);
            return ammo;
        }

        private static string GetEffects(Player player)
        {
            string effects;
            List<string> effectsList = ListPool<string>.Pool.Get();
            foreach (var effect in player.ActiveEffects)
            {
                effectsList.Add($"{effect.name} ({effect.Intensity})");
            }

            if(effectsList.Count == 0)
            {
                effects = "no effect";
            } else
            {
                effects = string.Join(", ", effectsList);
            }

            ListPool<string>.Pool.Return(effectsList);
            return effects;
        }

        private static string YesNo(bool value)
        {
            string str;

            if (value)
            {
                str = "Yes";
            } else
            {
                str = "No";
            }

            return str;
        }
    }
}
