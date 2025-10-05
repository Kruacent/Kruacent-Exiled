using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Exiled.API.Features.Pools;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.CustomRoles.Settings;
using KE.Utils.API;
using KE.Utils.API.Displays.DisplayMeow;
using MEC;
using PlayerRoles.Subroutines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UserSettings.ServerSpecific;
using UserSettings.UserInterfaceSettings;
using static PlayerList;

namespace KE.CustomRoles.API.Features
{
    /// <summary>
    /// Our version of the <see cref="Exiled.CustomRoles.API.Features.ActiveAbility"/>
    /// </summary>
    public abstract class KEAbilities
    {
        #region static stuff
        private static HashSet<KEAbilities> registered = new();
        public static HashSet<KEAbilities> Registered => registered;
        #endregion

        #region abstract stuff
        public abstract string Name { get; }
        public abstract string PublicName { get; }
        public abstract string Description { get; }
        /// <summary>
        /// Used for the settings option
        /// </summary>
        public abstract int Id { get; }
        /// <summary>
        /// in seconds
        /// </summary>
        public abstract float Cooldown { get; }
        #endregion

        public HashSet<KECustomRole> GetRoles
        {
            get
            {
                HashSet<KECustomRole> result = new();
                foreach (KECustomRole cr in KECustomRole.KnownKECR)
                {
                    foreach(int abilityId in cr.Abilities)
                    {
                        KEAbilities ability = Get(abilityId);
                        if (ability == this)
                        {
                            result.Add(cr);
                        }
                    }
                }
                return result;
            }
        }
        private Dictionary<Player, DateTime> LastUsed = new();
        private static Dictionary<System.Type, KEAbilities> TypeToAbility { get; } = new();
        private static Dictionary<int, KEAbilities> IdToAbility { get; } = new();
        private static HeaderSetting header;
        private static bool flagHeader = false;
        private SettingBase setting;
        public HashSet<Player> Players { get; } = new HashSet<Player>();
        public HashSet<Player> Selected { get; } = new();



        public static Dictionary<Player, List<KEAbilities>> PlayersAbility { get;} = new();

        protected KEAbilities()
        {
            
        }
        public void Init()
        {
            if (IdToAbility.ContainsKey(Id))
            {
                Log.Warn($"{Name} ({Id}) have the same id as {IdToAbility[Id].Name}. Skipping...");
                return;
            }



            SettingBase old = SettingBase.List.Where(s => s.Id == Id).FirstOrDefault();

            if(old == null)
            {
                if (!flagHeader)
                {
                    header = new(MainPlugin.Configs.HeaderId, "Abilities");
                    SettingBase.Register([header]);
                    flagHeader = true;
                }
                Log.Debug("creating keybind");
                setting = new KeybindSetting(Id, Name, UnityEngine.KeyCode.None,hintDescription:Description);
                SettingBase.Register([setting]);
            }
            else
            {
                Log.Error($"setting of {this} have the same id as {old.Label}");
            }
            StartLoop();
            IdToAbility.Add(Id, this);
            TypeToAbility.Add(GetType(), this);
            InternalSubscribeEvent();
        }

        public void Destroy()
        {
            Func<Player,bool> p = null;
            SettingBase.Unregister(p, [setting]);
            InternalUnsubcribeEvent();
        }

        private void InternalSubscribeEvent()
        {

            ServerSpecificSettingsSync.ServerOnSettingValueReceived += SafeOnSettingValueReceived;
            Exiled.Events.Handlers.Player.Verified += OnVerified;
            SubscribeEvents();
        }

        private void InternalUnsubcribeEvent()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= SafeOnSettingValueReceived;
            Exiled.Events.Handlers.Player.Verified -= OnVerified;
            UnsubscribeEvents();
        }

        private void SafeOnSettingValueReceived(ReferenceHub hub, ServerSpecificSettingBase settingBase)
        {
            //not catching the exception will desync & kick the player
            try
            {
                OnSettingValueReceived(Player.Get(hub), settingBase);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private void OnSettingValueReceived(Player player, ServerSpecificSettingBase settingBase)
        {
            if (!CheckPressed(settingBase)) return;
            if (!Check(player)) return;

            if (!SettingHandler.Instance.GetMode(player)) return;


            if(CanUse(player,out string _))
            {
                UseAbility(player);
            }
        }



        private void OnVerified(VerifiedEventArgs ev)
        {
            ServerSpecificSettingsSync.SendToPlayer(ev.Player.ReferenceHub);
        }

        protected virtual void SubscribeEvents()
        {

        }

        protected virtual void UnsubscribeEvents()
        {

        }

        protected virtual void AbilityUsed(Player player)
        {

        }

        protected virtual void AbilityAdded(Player player)
        {

        }
        protected virtual void AbilityRemoved(Player player)
        {

        }

        public void ShowAbility(Player player)
        {
            float time = MainPlugin.SettingHandler.GetAbilityTime(player);

            string msg = $"<b>{PublicName}</b>\n{Description}";


            DisplayHandler.Instance.AddHint(MainPlugin.AbilitiesDesc, player, msg, time);
        }

        public void SelectAbility(Player player)
        {
            if (Selected.Add(player))
            {
                Log.Debug($"player {player.Nickname} selected ability {this}");
                foreach (KEAbilities abilities in Registered.Where(a => a != this && a.Players.Contains(player)))
                {
                    abilities.UnselectAbility(player);
                }

                ShowAbility(player);
            }
        }

        public void UnselectAbility(Player player)
        {
            Log.Debug($"player {player.Nickname} unselected ability {this}");
            Selected.Remove(player);
        }
        
        public void RemoveAbility(Player player)
        {

            if (Players.Contains(player))
            {
                Log.Debug($"player {player.Nickname} lost {this}");
                PlayersAbility[player].Remove(this);
                Players.Remove(player);
                AbilityRemoved(player);
            }
        }
        public void AddAbility(Player player)
        {
            
            bool result = Players.Add(player);
            Log.Debug($"player {player.Nickname} got {this} ({result})");
            if (result)
            {
                if(!PlayersAbility.TryGetValue(player,out var _))
                {
                    PlayersAbility.Add(player, new());
                }
                PlayersAbility[player].Add(this);

                AbilityAdded(player);
            }
            
        }
        public void UseAbility(Player player)
        {
            LastUsed[player] = DateTime.Now;

            
            AbilityUsed(player);
        }

        public bool Check(Player player)
        {
            if(player is not null)
            {
                return Players.Contains(player);
            }
            return false;
        }

        public bool CanUse(Player player, out string result)
        {
            if (player == null)
            {
                result = "player null";
                return false;
            }
            if (!Players.Contains(player))
            {
                result = "cannot use this";
                return false;
            }

            if (!LastUsed.ContainsKey(player))
            {
                result = "never used";
                return true;
            }

            DateTime dateTime = LastUsed[player] + TimeSpan.FromSeconds(Cooldown);
            if (DateTime.Now > dateTime)
            {
                result = "ok";
                return true;
            }
            result = "in cooldown";
            return false;
        }

        public bool CheckPressed(ServerSpecificSettingBase settingBase)
        {
            return CheckPressed(settingBase, setting.Id);
        }

        public static void UseSelected(Player player)
        {
            if(TryGetSelected(player,out var ability))
            {
                if (ability.CanUse(player, out string _))
                {
                    ability.UseAbility(player);
                }
            }
        }

        public static bool TryAddToPlayer(Type typeAbility,Player player)
        {
            if (!TypeToAbility.TryGetValue(typeAbility, out var ability)) return false;


            ability.AddAbility(player);
            return true;


        }        
        public static bool TryAddToPlayer(int abilityId,Player player)
        {
            if (!IdToAbility.TryGetValue(abilityId, out var ability)) return false;
            

            ability.AddAbility(player);
            return true;


        }

        public static void RemoveAllSelect(Player player)
        {
            if (!PlayersAbility.ContainsKey(player)) return;

            foreach(KEAbilities ability in PlayersAbility[player])
            {
                ability.Selected.Remove(player);
            }

            UpdateGUI(player);
        }

        public static void SelectFirstAbility(Player player)
        {
            if(PlayersAbility.TryGetValue(player,out var list))
            {
                list[0].SelectAbility(player);

                UpdateGUI(player);
            }
            
        }

        public static void TryRemoveFromPlayer(Player player)
        {
            foreach(KEAbilities abilities in Registered)
            {
                if (abilities.Players.Contains(player))
                {
                    abilities.RemoveAbility(player);
                }
            }
        }


        public static bool CheckPressed(ServerSpecificSettingBase settingBase, int id)
        {
            return settingBase is SSKeybindSetting keybindSetting && keybindSetting.SettingId == id && keybindSetting.SyncIsPressed;
        }

        #region register
        private bool TryRegister()
        {


            if (!Registered.Contains(this))
            {
                Registered.Add(this);
                Init();
                Log.Debug("registered " + Name);
                return true;
            }



            return false;
        }


        public static IEnumerable<KEAbilities> Register(Assembly assembly =null)
        {
            IEnumerable<KEAbilities> abilities = ReflectionHelper.GetObjects<KEAbilities>(assembly);
            List<KEAbilities> result = abilities.ToList();

            foreach(KEAbilities ability in abilities)
            {

                if (!ability.TryRegister())
                {
                    Log.Warn("couldn't register KEability " + ability);
                    result.Remove(ability);
                }
                
            }
            registered = result.ToHashSet();
            return result;

        }


        private bool TryUnregister()
        {
            Destroy();
            if (Registered.Remove(this))
            {
                Log.Debug("unregistered " + this);
                return true;
            }
            Log.Warn(this + " was not registered");
            return false;
        }

        public static IEnumerable<KEAbilities> Unregister()
        {
            List<KEAbilities> list = new();
            foreach (KEAbilities item in Registered)
            {
                item.TryUnregister();
                list.Add(item);
            }

            return list;
        }
        #endregion

        #region getters

        public static KEAbilities Get(int id)
        {
            foreach(KEAbilities kEAbilities in Registered)
            {
                if (id == kEAbilities.Id)
                {
                    return kEAbilities;
                }
            }

            return null;
        }

        public static bool TryGet(int id, out KEAbilities ability)
        {
            ability = Get(id);
            return ability != null;
        }

        public static KEAbilities GetSelected(Player player)
        {
            foreach(KEAbilities ability in Registered)
            {
                if (ability.Selected.Contains(player))
                {
                    return ability;
                }
            }
            return null;
        }

        public static bool TryGetSelected(Player player, out KEAbilities ability)
        {
            ability = GetSelected(player);
            return ability != null;
        }


        #endregion

        #region gui


        public const float UpdateTime = 1;
        private static bool flag = false;
        private static void StartLoop()
        {
            if (!flag)
            {
                Timing.RunCoroutine(Loop());
                flag = true;
            }
        }
        private static IEnumerator<float> Loop()
        {
            while (true)
            {
                UpdateAllGUI();
                yield return Timing.WaitForSeconds(UpdateTime);
            }
        }
        public static void UpdateAllGUI()
        {
            foreach(Player player in PlayersAbility.Keys)
            {
                UpdateGUI(player);
            }
        }
        public static void UpdateGUI(Player player)
        {
            StringBuilder builder = StringBuilderPool.Pool.Get();
            

            List<KEAbilities> allAbilities = PlayersAbility[player];

            for (int i = 0; i < allAbilities.Count; i++)
            {


                KEAbilities ability = allAbilities[i];

                builder.Append(ability.PublicName);
                builder.Append(" ");
                
                if (ability.CanUse(player,out var output))
                {
                    builder.Append("[READY]");
                }
                else
                {
                    DateTime dateTime = ability.LastUsed[player] + TimeSpan.FromSeconds(ability.Cooldown);
                    builder.Append("[");
                    builder.Append(Math.Round((dateTime - DateTime.Now).TotalSeconds, 0));
                    builder.Append("s]");
                }

                
                if (ability.Selected.Contains(player))
                {
                    string arrow = SettingHandler.Instance.GetArrow(player);

                    if (string.IsNullOrEmpty(arrow))
                    {
                        arrow = SettingHandler.baseArrow;
                    }

                    builder.Append(arrow);
                }
                builder.AppendLine();
            }

            string msg = builder.ToString();
            StringBuilderPool.Pool.Return(builder);
            DisplayHandler.Instance.AddHint(MainPlugin.Abilities, player, msg, UpdateTime);
        }


        #endregion

        public override string ToString()
        {
            return  "("+Id + ") " +Name;
        }

    }
}
