using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API;
using KE.Utils.API.Displays.DisplayMeow;
using MEC;
using PlayerRoles.Subroutines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UserSettings.ServerSpecific;

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

        private Dictionary<Player, DateTime> LastUsed = new();
        private static Dictionary<System.Type, KEAbilities> TypeToAbility { get; } = new();
        private static HeaderSetting header;
        private static bool flagHeader = false;
        private SettingBase setting;
        public HashSet<Player> Players { get; } = new HashSet<Player>();


        private static Dictionary<Player, List<KEAbilities>> Show = new();

        protected KEAbilities()
        {

        }
        public void Init()
        {
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

        
        public void RemoveAbility(Player player)
        {

            Log.Debug($"player {player.Nickname} lost {this}");
            if (Players.Contains(player))
            {
                Show[player].Remove(this);
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
                if(!Show.TryGetValue(player,out var _))
                {
                    Show.Add(player, new());
                }
                Show[player].Add(this);

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

        public static bool TryAddToPlayer(Type typeAbility,Player player)
        {
            if (!TypeToAbility.TryGetValue(typeAbility, out var ability)) return false;


            ability.AddAbility(player);
            return true;


        }

        public static void TryRemoveFromPlayer(Player player)
        {
            foreach(KEAbilities abilities in Registered)
            {
                abilities.RemoveAbility(player);
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


        #region gui


        private static float UpdateTime = 1f;
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
        private static void UpdateAllGUI()
        {
            foreach(Player player in Show.Keys)
            {
                UpdateGUI(player);
            }
        }
        private static void UpdateGUI(Player player)
        {
            string msg = "";

            foreach (KEAbilities ability in Show[player])
            {
                msg += $"{ability.Name} ";
                if (ability.CanUse(player,out var output))
                {
                    msg += "[READY]";
                }
                else
                {
                    DateTime dateTime = ability.LastUsed[player] + TimeSpan.FromSeconds(ability.Cooldown);
                    msg += $"[{Math.Round((dateTime - DateTime.Now).TotalSeconds, 0)}s]";
                }



                msg += "\n";
            }
            DisplayHandler.Instance.AddHint(MainPlugin.Abilities, player, msg, 1f);
        }


        #endregion

        public override string ToString()
        {
            return Name;
        }

    }
}
