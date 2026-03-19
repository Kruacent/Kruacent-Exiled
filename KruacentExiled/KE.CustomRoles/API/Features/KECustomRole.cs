using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pools;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using HintServiceMeow.Core.Models.Hints;
using InventorySystem.Configs;
using KE.CustomRoles.API.HintPositions;
using KE.CustomRoles.API.Interfaces;
using KE.CustomRoles.Events.EventArgs;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using KE.Utils.API.Translations;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using UnityEngine;
using YamlDotNet.Core.Tokens;

namespace KE.CustomRoles.API.Features
{
    public abstract class KECustomRole : CustomRole
    {
        public const float TimeAttributingInventory = .25f;
        public sealed override uint Id { get; set; } = 0;
        public override string Name
        {
            get
            {
                return Role.ToString().ToUpper() + "_" + InternalName.RemoveSpaces();
            }
            set
            {

            }
        }

        public virtual string InternalName => GetType().Name;

        public sealed override string Description { get; set; } = string.Empty;

        public static new HashSet<KECustomRole> Registered { get; } = new();

        public sealed override string CustomInfo { get; set; }

        /// <summary>
        /// the max number of people who can have this role in a round
        /// </summary>
        public virtual int Limit { get; set; } = 1;
        /// <summary>
        /// the number of people who had this role in a round
        /// </summary>
        public int CurrentNumberOfSpawn { get; set; } = 0;

        public static void ResetNumberOfSpawn()
        {

            foreach(KECustomRole cr in Registered)
            {
                cr.CurrentNumberOfSpawn = 0;
            }
            
        }


        protected abstract Dictionary<string, Dictionary<string, string>> SetTranslation();



        public abstract override RoleTypeId Role { get; }

        /// <summary>
        /// <see cref="KEAbilities.Name"/>
        /// </summary>
        public virtual HashSet<string> Abilities { get; }

        public sealed override bool IgnoreSpawnSystem { get; set; } = true;
        public sealed override List<CustomAbility> CustomAbilities { get; set; } = null;
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
            
            sb.Append(GetTranslation(player,TranslationKeyName));

            if (color != null)
            {
                sb.Append("</color>");
            }
            sb.AppendLine("</b>");

            if (MainPlugin.SettingHandler.GetDescriptionsSettings(player))
            {
                sb.AppendLine(GetTranslation(player, TranslationKeyDesc));

                if(this is IHealable heal)
                {
                    sb.AppendLine(GetTranslation(player, TranslationHealable));
                }

            }


            float delay = MainPlugin.SettingHandler.GetTime(player);

            DisplayHandler.Instance.AddHint(MainPlugin.CRHint, player, sb.ToString(), delay);
            StringBuilderPool.Pool.Return(sb);
        }

        public void Show(Player player)
        {
            ShowMessage(player);
        }


        private static Dictionary<Type, KECustomRole> typeLookupTable = new();
        private static Dictionary<string, KECustomRole> stringLookupTable = new();

        protected const string CustomRoleNameKey = "Name";
        protected const string CustomRoleDescriptionKey = "Desc";
        public string TranslationKeyName => Name + "_" + CustomRoleNameKey;
        public string TranslationKeyDesc => Name + "_" + CustomRoleDescriptionKey;

        public const string CustomRoleTranslationId = "CustomRole";
        public override void Init()
        {
            typeLookupTable.Add(GetType(), this);
            stringLookupTable.Add(Name, this);
            OneTimeInit();
            InternalSubscribeEvents();
            SubscribeEvents();

            var translate = SetTranslation();


            TranslationHub.Add(CustomRoleTranslationId, translate);

            Log.Debug("adding keys to "+Name);
        }

        private bool activated = false;

        private const string TranslationGUI = "CustomRoleGUI";
        private const string TranslationHealable = "CustomRoleHealable";
        private static Dictionary<string, Dictionary<string, string>> SetStaticTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new()
                {
                    [TranslationGUI] = "Current Role",
                    [TranslationHealable] = "This Custom Role is healable!",
                },
                ["fr"] = new()
                {
                    [TranslationGUI] = "Role",
                    [TranslationHealable] = "Ce custom role peut être enlevé avec un object!",
                },
            };
        }

        private void OneTimeInit()
        {
            if (activated) return;
            TranslationHub.Add(CustomRoleTranslationId, SetStaticTranslation());


            activated = true;
        }
        public static string GetTranslation(Player player, string key)
        {
            return TranslationHub.Get(player, CustomRoleTranslationId, key);
        }
        public static string GetTranslation(string lang, string key)
        {
            return TranslationHub.Get(lang, CustomRoleTranslationId, key);
        }

        public override void Destroy()
        {
            typeLookupTable.Remove(GetType());
            stringLookupTable.Remove(Name);
            InternalUnsubscribeEvents();
            UnsubscribeEvents();
        }

        protected virtual void InternalSubscribeEvents()
        {
            if(this is IHealable)
            {
                Exiled.Events.Handlers.Player.UsedItem += OnUsedItem;
            }

            if(this is IEffectImmunity)
            {
                Exiled.Events.Handlers.Player.ReceivingEffect += OnReceivingEffect;
            }
            
        }


        protected virtual void InternalUnsubscribeEvents()
        {
            if (this is IHealable)
            {
                Exiled.Events.Handlers.Player.UsedItem -= OnUsedItem;
            }
            if (this is IEffectImmunity)
            {
                Exiled.Events.Handlers.Player.ReceivingEffect -= OnReceivingEffect;
            }
        }


        private void OnUsedItem(UsedItemEventArgs ev)
        {
            if (!Check(ev.Player)) return;
            IHealable healable = this as IHealable;
            if(healable.HealItem is null || healable.HealItem.Count == 0)
            {
                Log.Warn("no healable item found for" + Name);
                return;
            }


            if (healable.HealItem.Contains(ev.Item.Type))
            {
                RemoveRole(ev.Player);
            }
        }


        private void OnReceivingEffect(ReceivingEffectEventArgs ev)
        {
            if (!Check(ev.Player)) return;
            if (!ev.IsAllowed) return;

            IEffectImmunity effectImmunity = this as IEffectImmunity;

            if (effectImmunity.ImmuneEffects is null || effectImmunity.ImmuneEffects.Count == 0)
            {
                Log.Warn("no healable item found for" + Name);
                return;
            }
            if (effectImmunity.ImmuneEffects.Contains(ev.Effect.GetEffectType()))
            {
                ev.IsAllowed = false;
            }

        }


        public static void SpawnStartRound(Misc.Features.Spawn.SpawnedEventArgs ev)
        {
            GiveRandomRole(Player.List.Except(ev.CustomRoles));
        }

        public static void RespawnCustomRole(RespawnedTeamEventArgs ev)
        {
            GiveRandomRole(ev.Players);
        }

        public static void ShowCustomRole(JoinedEventArgs ev)
        {
            Timing.CallDelayed(1, delegate
            {
                Player player = ev.Player;
                DisplayHandler.Instance.CreateAuto(player, arg => CurrentRole(player), CurrentCustomRolePosition.HintPlacement);
            });

            
        }




        public static string CurrentRole(Player player)
        {
            StringBuilder sb = StringBuilderPool.Pool.Get();

            if (HasCustomRole(player))
            {
                KECustomRole kECustomRole = Get(player).First();

                sb.Append(GetTranslation(player, TranslationGUI));
                sb.AppendLine(" : ");
                sb.AppendLine();
                sb.Append("<color=#");
                sb.Append(ColorUtility.ToHtmlStringRGB(player.Role.Color));
                sb.Append(">");
                sb.Append(GetTranslation(player,kECustomRole.TranslationKeyName));
                sb.Append("</color>");
            } 
            else if (player.IsDead)
            {
                Player spectating = GetSpectatingPlayer(player);
                KECustomRole customRole = null;
                if (spectating != null)
                {
                    customRole = Get(spectating).FirstOrDefault();
                }


                if(customRole != null)
                {
                    sb.Append(GetTranslation(player, TranslationGUI));
                    sb.AppendLine(" : ");
                    sb.AppendLine();
                    sb.Append("<color=#");
                    sb.Append(ColorUtility.ToHtmlStringRGB(spectating.Role.Color));
                    sb.Append(">");
                    sb.Append(GetTranslation(player,customRole.TranslationKeyName));
                    sb.Append("</color>");
                }
            }

            string result = StringBuilderPool.Pool.ToStringReturn(sb);

            if (string.IsNullOrEmpty(result))
            {
                result = " ";
            }


            return result;
        }

        private static Player GetSpectatingPlayer(Player spectator)
        {

            foreach(Player player in Player.Enumerable)
            {
                if (player.CurrentSpectatingPlayers.Contains(spectator))
                {
                    return player;
                }
            }

            return null;


        }


        protected void ShowEffectHint(Player player, string text)
        {
            float delay = MainPlugin.SettingHandler.GetTime(player); ;
            DisplayHandler.Instance.AddHint(MainPlugin.CREffect, player, text, delay);
        }
        protected void ShowEffectHint(Player player, string text, float delay)
        {
            DisplayHandler.Instance.AddHint(MainPlugin.CREffect, player, text, delay);
        }





        #region addrole     
        public override void AddRole(Player player)
        {
            Player player2 = player;
            Log.Debug(Name + ": Adding role to " + player2.Nickname + ".");
            Log.Debug(Name + ": old role type " + player2.Role.Type + ".");
            Log.Debug(Name + ": new role type " + Role + ".");


            ReceivingCustomRoleEventArgs ev1 = new(player2,this);

            Events.Handlers.KECustomRole.OnReceivingCustomRole(ev1);

            if (!ev1.IsAllowed)
            {
                Log.Debug(Name + ": role cancelled by plugin");
                return;
            }

            foreach(KECustomRole cr in Get(player))
            {
                cr.RemoveRole(player);
            }


            
            CurrentNumberOfSpawn++;
            



            if (Role != RoleTypeId.None)
            {
                Log.Debug("new role exist");
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
                    player2.Role.Set(Role, SpawnReason.ForceClass, RoleSpawnFlags.UseSpawnpoint);
                }
                else
                {
                    player2.Role.Set(Role, SpawnReason.ForceClass, RoleSpawnFlags.AssignInventory);
                }
            }
            


            TrackedPlayers.Add(player2);
            
            Timing.CallDelayed(TimeAttributingInventory, delegate
            {
                ClearInventory(player2);
                GiveInventory(player2);
                GiveAmmo(player2);
            });


            AttributeHealth(player2);
            AttributeScale(player2);

            SetCustomInfo(player2);

            SetAbilities(player2);

            SetSpawn(player2);


            ShowMessage(player2);
            
            RoleAdded(player2);
            player2.UniqueRole = Name;
            player2.TryAddCustomRoleFriendlyFire(Name, CustomRoleFFMultiplier);
            ReceivedCustomRoleEventArgs ev2 = new(player2, this);

            Events.Handlers.KECustomRole.OnReceivedCustomRole(ev2);

        }


        public static readonly HintPosition CurrentCustomRolePosition = new CurrentCustomRolePosition();



        protected virtual void ClearInventory(Player player)
        {
            if (!KeepInventoryOnSpawn)
            {
                player.ClearInventory();
            }
        }

        protected virtual void GiveInventory(Player player)
        {

            for (int i = 0; i < Inventory.Count; i++)
            {
                string item = Inventory[i];
                Log.Debug(Name + ": Adding " + item + " to inventory.");
                if (TryAddItem(player, item) && CustomItem.TryGet(item, out CustomItem customItem) && customItem is CustomWeapon customWeapon && player.CurrentItem is Firearm firearm && !customWeapon.Attachments.IsEmpty())
                {
                    firearm.AddAttachment(customWeapon.Attachments);
                    Log.Debug(Name + ": Applied attachments to " + item + ".");
                }
            }
        }

        protected virtual void GiveAmmo(Player player)
        {
            if (Ammo.Count > 0)
            {
                AmmoType[] values = EnumUtils<AmmoType>.Values;
                foreach (AmmoType ammoType in values)
                {
                    if (ammoType != 0)
                    {
                        player.SetAmmo(ammoType, (ushort)(Ammo.ContainsKey(ammoType) ? Ammo[ammoType] == ushort.MaxValue ? InventoryLimits.GetAmmoLimit(ammoType.GetItemType(), player.ReferenceHub) : Ammo[ammoType] : 0));
                    }
                }
            }
        }

        protected virtual void AttributeHealth(Player player)
        {
            player.Health = MaxHealth;
            player.MaxHealth = MaxHealth;
        }

        protected virtual void AttributeScale(Player player)
        {
            player.Scale = Scale;
        }

        protected virtual void SetSpawn(Player player)
        {
            Vector3 spawnPosition = GetSpawnPosition();
            if (spawnPosition != Vector3.zero)
            {
                player.Position = spawnPosition;
            }
        }

        protected virtual void SetCustomInfo(Player player)
        {
            //MirrorExtensions.SetPlayerInfoForTargetOnly()

            string name = GetTranslation("legacy", TranslationKeyName);

            player.CustomInfo = player.CustomName;
            if (!string.IsNullOrEmpty(name))
            {
                player.CustomInfo += "\n" + name;
            }
            player.InfoArea &= ~(PlayerInfoArea.Nickname | PlayerInfoArea.Role);
        }

        protected virtual void SetAbilities(Player player)
        {
            KEAbilities.TryRemoveFromPlayer(player);

            if (Abilities != null)
            {
                foreach (string name in Abilities)
                {
                    if(!KEAbilities.TryAddToPlayer(name, player))
                    {
                        Log.Error("couldn't find ability : " + name);
                    }
                   
                }
                KEAbilities.SelectFirstAbility(player,true);
            }
        }


#endregion


        public override void RemoveRole(Player player)
        {
            base.RemoveRole(player);
            if (Abilities != null)
            {
                KEAbilities.TryRemoveFromPlayer(player);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <returns>true if the player can have this CR ; false otherwise</returns>
        public virtual bool IsAvailable(Player player)
        {
            if (CurrentNumberOfSpawn >= Limit) return false;
            return RoleCheck(player.Role);
        }


        public virtual bool RoleCheck(RoleTypeId role)
        {
            return Role == role;
        }


        /// <summary>
        /// The chance of having this role, NOT the chance to have a role
        /// </summary>
        public new virtual float SpawnChance { get; set; } = 100;

        public static new KECustomRole Get(string fullinternalname)
        {
            return stringLookupTable[fullinternalname];
        }


        public static bool TryGet(string fullinternalname,out KECustomRole customrole)
        {
            customrole = Get(fullinternalname);
            return customrole != null;
        }

        public static KECustomRole Get(RoleTypeId roleid,string name)
        {
            return Get(roleid.ToString().ToUpper() + "_" + name);
        }
        public static bool Get(RoleTypeId roleid, string name, out KECustomRole customrole)
        {
            customrole = Get(roleid, name);
            return customrole != null;
        }


        public static bool HasCustomRole(Player player)
        {
            if (player is null) return false;


            foreach(KECustomRole customRole in Registered)
            {
                if (customRole.Check(player))
                {
                    return true;
                }
            }
            return false;

        }

        public static IEnumerable<KECustomRole> Get(Player player)
        {
            List<KECustomRole> cr = new();
            foreach(KECustomRole ke in Registered)
            {
                if (ke.Check(player))
                {
                    cr.Add(ke);
                }
            }
            return cr;
        }

        #region Spawn

        /// <summary>
        /// The chance to get a <see cref="KECustomRole"/> at the start or a respawn
        /// </summary>
        public static float Chance
        {
            get
            {
                return chance;
            }
            set
            {
                chance = Mathf.Clamp(value, 0f, 100f);
            }
        }

        private static float chance = 100;

        private static KECustomRole AssignRole(Dictionary<KECustomRole, float> roleChances)
        {
            float totalWeight = roleChances.Values.Sum();
            float randomValue = UnityEngine.Random.Range(0f, totalWeight);

            foreach (var role in roleChances)
            {
                randomValue -= role.Value;
                if (randomValue <= 0)
                    return role.Key;
            }

            return null;
        }

        public static Dictionary<KECustomRole, float> GetAvailableCustomRole(Player player)
        {
            return Registered.Where(cr => cr.IsAvailable(player)).ToDictionary(c => c, c => c.SpawnChance);
        }

        public static void GiveRandomRole(Player player)
        {
            if (player == null)
                return;
            if (UnityEngine.Random.Range(0f, 100f) > Chance)
            {
                Log.Debug("no luck");
                return;
            }

            if(HasCustomRole(player))
            {
                Log.Debug("already got a cr");
                return;
            }


            KECustomRole cr = AssignRole(GetAvailableCustomRole(player));
            Log.Debug($"{player.Id} : {cr?.Name}");

            cr?.AddRole(player);
        }

        
        public static void GiveRandomRole(IEnumerable<Player> players)
        {
            foreach (Player p in players)
            {
                GiveRandomRole(p);
            }
        }
        #endregion





        #region Register

        public bool TryRegister()
        {

            if (!Registered.Contains(this))
            {
                if (Registered.Any((KECustomRole r) => r.Name == Name))
                {
                    Log.Warn($"{Name} has tried to register with the same Name as another role: {Name}. It will not be registered!");
                    return false;
                }

                Registered.Add(this);
                Init();
                return true;
            }


            return false;
        }
        public bool TryUnregister()
        {
            Destroy();
            if (!Registered.Remove(this))
            {
                Log.Warn($"Cannot unregister {Name} [{Role}], it hasn't been registered yet.");
                return false;
            }

            return true;
        }

        public static IEnumerable<KECustomRole> Register()
        {
            List<KECustomRole> list = new ();
            Type[] types = Assembly.GetCallingAssembly().GetTypes();
            foreach (Type type in types)
            {
                if (!type.IsAbstract && type.IsSubclassOf(typeof(KECustomRole)))
                {
                    KECustomRole customRole = (KECustomRole)Activator.CreateInstance(type);
                    if (customRole.TryRegister())
                    {
                        list.Add(customRole);
                    }
                }
            }
            return list;
        }


        public static IEnumerable<KECustomRole> Unregister()
        {
            List<KECustomRole> list = new ();
            foreach (KECustomRole item in Registered)
            {
                item.TryUnregister();
                list.Add(item);
            }

            return list;
        }

        #endregion
    }
}
