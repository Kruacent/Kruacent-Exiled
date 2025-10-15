using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pools;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;
using InventorySystem.Configs;
using KE.CustomRoles.API.Interfaces;
using KE.Utils.API.Displays.DisplayMeow;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KE.CustomRoles.API.Features
{
    public abstract class KECustomRole : CustomRole
    {

        public override string Name
        {
            get
            {
                return GetType().Name;
            }
            set
            {
                
            }
        }


        public static IEnumerable<KECustomRole> KnownKECR => Registered.Where(c => c is KECustomRole).Cast<KECustomRole>();

        public sealed override string CustomInfo { get; set; }
        public abstract string PublicName { get; set; }

        public virtual HashSet<int> Abilities { get; }

        public sealed override bool IgnoreSpawnSystem { get; set; } = true;
        protected override void ShowMessage(Player player)
        {
            //string msg = MainPlugin.Translations.GettingNewRole;
            //msg = msg.Replace("%Name%", PublicName).Replace("%Desc%",Description);
            StringBuilder sb =StringBuilderPool.Pool.Get();
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




            sb.AppendLine("</b>");

            if (MainPlugin.SettingHandler.GetDescriptionsSettings(player))
            {
                sb.AppendLine(Description);
            }


            float delay = MainPlugin.SettingHandler.GetTime(player);

            DisplayHandler.Instance.AddHint(MainPlugin.CRHint, player, sb.ToString(), delay);
            StringBuilderPool.Pool.Return(sb);
        }

        

        protected void ShowEffectHint(Player player, string text)
        {
            float delay = MainPlugin.SettingHandler.GetTime(player); ;
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

                for (int i = 0; i < Inventory.Count;i++)
                {
                    string item = Inventory[i];
                    Log.Debug(Name + ": Adding " + item + " to inventory.");
                    if (TryAddItem(player2, item) && CustomItem.TryGet(item, out CustomItem customItem) && customItem is CustomWeapon customWeapon && player2.CurrentItem is Firearm firearm && !customWeapon.Attachments.IsEmpty())
                    {
                        firearm.AddAttachment(customWeapon.Attachments);
                        Log.Debug(Name + ": Applied attachments to " + item + ".");
                    }
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

            player2.CustomInfo = player2.CustomName + "\n" + PublicName;
            player2.InfoArea &= ~(PlayerInfoArea.Nickname | PlayerInfoArea.Role);
            if (CustomAbilities != null)
            {
                foreach (CustomAbility customAbility in CustomAbilities)
                {
                    customAbility.AddAbility(player2);
                }
            }

            KEAbilities.TryRemoveFromPlayer(player);

            if(Abilities != null)
            {
                foreach(int abilityId in Abilities)
                {
                    KEAbilities.TryAddToPlayer(abilityId, player2);
                }
                KEAbilities.SelectFirstAbility(player2);
            }


            ShowMessage(player2);
            RoleAdded(player2);
            player2.UniqueRole = Name;
            player2.TryAddCustomRoleFriendlyFire(Name, CustomRoleFFMultiplier);
        }

        public override void RemoveRole(Player player)
        {
            if (Abilities != null)
            {
                KEAbilities.TryRemoveFromPlayer(player);
            }
            base.RemoveRole(player);
        }


        /// <summary>
        /// The chance of having this role NOT the chance to have a role
        /// </summary>
        public override abstract float SpawnChance { get; set; }




        #region Spawn

        /// <summary>
        /// The chance to get a <see cref="KECustomRole"/> at the start or a respawn
        /// </summary>
        public static int Chance
        {
            get
            {
                return chance;
            }
            set
            {
                chance = Mathf.Clamp(value, 0, 100);
            }
        }

        private static int chance = 40;

        private static CustomRole AssignRole(Dictionary<CustomRole, float> roleChances)
        {
            float totalWeight = roleChances.Values.Sum();
            float randomValue = UnityEngine.Random.Range(0f, totalWeight);

            foreach (var role in roleChances)
            {
                randomValue -= role.Value;
                if (randomValue <= 0)
                    return role.Key;
            }

            return roleChances.Keys.First();
        }

        public static Dictionary<CustomRole, float> GetAvailableCustomRole(Player player)
        {
            return Registered.Where(c => c.Role == player.Role || c is GlobalCustomRole cgr && cgr.Side == SideClass.Get(player.Role.Side)).ToDictionary(c => c, c => c.SpawnChance);
        }

        public static void GiveRandomRole(Player player)
        {
            if (player == null)
                return;
            if (Random.Range(0, 101) > Chance)
            {
                Log.Debug("no luck");
                return;
            }

            if(player.GetCustomRoles().Count != 0)
            {
                Log.Debug("already got a cr");
                return;
            }


            CustomRole cr = AssignRole(GetAvailableCustomRole(player));
            Log.Debug($"{player.Id} : {cr.Name}");

            //error assigning cr to a player with a gcr 
            cr?.AddRole(player);
        }
        
        public static void GiveRole(IEnumerable<Player> players)
        {
            foreach (Player p in players)
            {
                GiveRandomRole(p);
            }
        }
        #endregion


    }
}
