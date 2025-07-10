using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Exiled.API.Features.Pools;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;
using Exiled.CustomRoles.API.Features.Enums;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UserSettings.ServerSpecific;
using UserSettings.UserInterfaceSettings;

namespace KE.CustomRoles.Settings
{
    internal class SettingHandler : IUsingEvents
    {
        private int _idTimeCustomRole = 144;
        private int _idDesc = 143;
        private int _idkeybind = 145;
        private int switchForwardId = 146;
        private int switchBackwardId = 147;
        private IReadOnlyCollection<SettingBase> settings;
        public SettingHandler()
        {

            settings = new List<SettingBase>()
            {
                new HeaderSetting ("Custom Roles Hint Settings"),
                new TwoButtonsSetting(_idDesc,"Descriptions","Disabled","Enabled",true,"hide/show the description the Custom Role "),
                new SliderSetting(_idTimeCustomRole,"Time shown",0,30,20),
                new KeybindSetting(_idkeybind,"ability",UnityEngine.KeyCode.K),
                new KeybindSetting(switchForwardId,"forward",UnityEngine.KeyCode.L),
                new KeybindSetting(switchBackwardId,"backward",UnityEngine.KeyCode.J)
            };
        }

        public void SubscribeEvents()
        {
            SettingBase.Register(settings);
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += SafeOnSettingValueReceived;
            Exiled.Events.Handlers.Player.Verified += OnVerified;

        }

        public void UnsubscribeEvents()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= SafeOnSettingValueReceived;
            Exiled.Events.Handlers.Player.Verified -= OnVerified;
            SettingBase.Unregister(predicate:null, settings);
        }



        private void OnVerified(VerifiedEventArgs ev)
        {
            ServerSpecificSettingsSync.SendToPlayer(ev.Player.ReferenceHub);
        }


        private void SafeOnSettingValueReceived(ReferenceHub hub, ServerSpecificSettingBase settingBase)
        {
            try
            {
                OnSettingValueReceived( hub, settingBase);
            }
            catch(Exception e)
            {
                Log.Error(e);
            }
        }

        private void OnSettingValueReceived(ReferenceHub hub, ServerSpecificSettingBase settingBase)
        {
            if (settingBase == null)
            {
                Log.Debug("setting null whar?");
                return;
            }
            Player p = Player.Get(hub);
            if(p == null)
            {
                Log.Debug("no player");
                return;
            }
            //Log.Debug("get abilities");
            IEnumerable<ActiveAbility> a = p.GetActiveAbilities();

            if(a == null)
            {
                //Log.Debug("no abilities 1");
                return;
            }
            
            List<ActiveAbility> abilities = ListPool<ActiveAbility>.Pool.Get(a);
            
            if (abilities.Count == 0)
            {
                //Log.Debug("no abilities 2");
                return;
            }
            //Log.Debug($" (prev : {abilities.Count}) get selected abilities");
            ActiveAbility currentAbility = GetSelectedAbility(p);
            //Log.Debug("finish selected");
            ActiveAbility want = null;

            


            int index = 0;

            //Log.Debug($" (prev : {currentAbility?.Name}) check press forward");
            if (CheckPressed(settingBase, switchForwardId))
            {
                if (currentAbility != null)
                {
                    index = abilities.IndexOf(currentAbility);
                    currentAbility.UnSelectAbility(p);
                }
                want = abilities[(index + 1) % abilities.Count];
                
                
            }
            //Log.Debug($" check press backward");
            if (CheckPressed(settingBase, switchBackwardId))
            {
                if (currentAbility != null)
                {
                    index = abilities.IndexOf(currentAbility);
                    currentAbility.UnSelectAbility(p);
                }

                if(index - 1 < 0)
                {
                    want = abilities[abilities.Count -1];
                }
                else
                {
                    want = abilities[(index - 1) % abilities.Count];
                }
                
                
            }
            //Log.Debug($"select ability");
            want?.SelectAbility(p);

            currentAbility = GetSelectedAbility(p);
            //Log.Debug(currentAbility.Name);
            if (CheckPressed(settingBase,_idkeybind))
            {
                if (CanUse(currentAbility,p, out var result))
                {
                    
                    currentAbility.UseAbility(p);
                }
            }
            Log.Debug(currentAbility?.Name);
        }

        public static ActiveAbility GetSelectedAbility(Player player)
        {
            Player player2 = player;
            if (ActiveAbility.AllActiveAbilities.TryGetValue(player2, out HashSet<ActiveAbility> value))
            {
                //Log.Debug("got all abilities");
               // ActiveAbility sel = value.FirstOrDefault((ActiveAbility a) => a.Check(player2, CheckType.Selected));

                foreach(ActiveAbility a in value)
                {
                    //Log.Debug("checking "+ a?.Name);
                    if (a?.Check(player2, CheckType.Selected) ?? false)
                    {
                        //Log.Debug("found " + a.Name);
                        return a;
                    }
                    //Log.Debug("finish checking");
                }
            }

            return null;
        }

        private bool CanUse(ActiveAbility a, Player player, out string result)
        {
            if(a == null)
            {
                result = "abi null";
                return false;
            }
            if (player == null)
            {
                result = "player null";
                return false;
            }
            if (!a.SelectedPlayers.Contains(player))
            {
                result = "no selected";
                return false;
            }

            if (!a.LastUsed.ContainsKey(player))
            {
                result = "already in";
                return true;
            }

            DateTime dateTime = a.LastUsed[player] + TimeSpan.FromSeconds(a.Cooldown);
            if (DateTime.Now > dateTime)
            {
                result = "ok";
                return true;
            }
            result = "in cooldown";
            return false;
        }


        public bool CheckPressed(ServerSpecificSettingBase settingBase,int id)
        {
            return settingBase is SSKeybindSetting keyindSetting && keyindSetting.SettingId == id && keyindSetting.SyncIsPressed;
        }

        internal float GetTime(Player p)
        {
            if (!SettingBase.TryGetSetting<SliderSetting>(p, _idTimeCustomRole, out var setting)) return 20;
            return setting.SliderValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns> true if the player wants description ; false otherwise</returns>
        internal bool GetDescriptionsSettings(Player p)
        {
            if (!SettingBase.TryGetSetting<TwoButtonsSetting>(p, _idDesc, out var setting)) return true;
            return setting.IsSecond;
        }

    }
}
