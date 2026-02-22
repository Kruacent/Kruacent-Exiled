using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.Settings.DebugSettings;
using KE.Utils.API;
using KE.Utils.API.Interfaces;
using KE.Utils.API.Settings;
using LabApi.Events.Arguments.PlayerEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UserSettings.ServerSpecific;

namespace KE.CustomRoles.Settings
{
    internal class SettingHandler : IUsingEvents
    {
        private readonly int _idHeader = 148;
        
        private readonly int _idTimeCustomRole = 144;
        private readonly int _idDesc = 143;
        private readonly int _idMode = 145;
        private readonly int _idDown = 149;
        private readonly int _idUp = 150;
        private readonly int _idSelect = 151;
        private readonly int _idArrow = 152;
        private readonly int _idTimeAbilityDesc = 153;
        private readonly int _idReshowCustomRole = 154;




        public static event Action<Player> DownPressed = delegate { };
        public static event Action<Player> UpPressed = delegate { };

        public static SettingHandler Instance { get; private set; }
        private List<SettingBase> settings;
        //public readonly SettingsPage page;
        //public readonly SettingsPage hintpage = null;
        public const string baseArrow = "<--";
        public SettingHandler()
        {
            Instance = this;
            HeaderSetting header = new HeaderSetting(_idHeader, "Custom Roles", padding: true);
            SettingBase arrow = SettingBase.Create(new SSPlaintextSetting(_idArrow, "Personalize the arrow next to the selected ability", baseArrow, 16, TMP_InputField.ContentType.Standard, string.Empty, 0));
            arrow.Header = header;
            settings = new List<SettingBase>()
            {
                header,

                new TwoButtonsSetting(_idDesc,"Descriptions","Disabled","Enabled",true,"hide/show the description the Custom Role ",header:header),
                new SliderSetting(_idTimeCustomRole,"Time shown",0,30,20,header:header),
                new SliderSetting(_idTimeAbilityDesc,"Ability Description time shown",0,30,20,header:header),
                new KeybindSetting(_idUp, "Select up", UnityEngine.KeyCode.None,header:header),
                new KeybindSetting(_idDown, "Select down", UnityEngine.KeyCode.None,header:header),
                new KeybindSetting(_idSelect, "Use selected ability", UnityEngine.KeyCode.None,header:header),
                arrow,
                new ButtonSetting(_idReshowCustomRole, "Reshow Custom role description","click",header:header)
            };
            


            

            if (MainPlugin.Instance.Config.Debug)
            {

                ReflectionHelper.GetObjects<DebugSetting>().ToList();

                List<SettingBase> debugsettings = new();
                foreach (DebugSetting debug in DebugSetting.settings)
                {
                    debug.Create();
                    debugsettings.AddRange(debug.Settings);
                }
                
                settings.AddRange(debugsettings);
                

            }
            SettingBase.Register(settings);

            
        }


        public void SubscribeEvents()
        {
            
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += SafeOnSettingValueReceived;
            LabApi.Events.Handlers.PlayerEvents.Joined += AddPlayer;
            DownPressed += Down;
            UpPressed += Up;

        }

        public void UnsubscribeEvents()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= SafeOnSettingValueReceived;
            LabApi.Events.Handlers.PlayerEvents.Joined -= AddPlayer;
            DownPressed -= Down;
            UpPressed -= Up;
        }
        

        private void AddPlayer(PlayerJoinedEventArgs ev)
        {
            ServerSpecificSettingsSync.SendToPlayer(ev.Player.ReferenceHub);
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
            if (CheckPressed(settingBase, _idDown))
            {
                DownPressed?.Invoke(player);
            }
            
            if (CheckPressed(settingBase, _idUp))
            {
                UpPressed?.Invoke(player);
            }

            if (CheckPressed(settingBase, _idSelect))
            {
                Log.Debug("select");
                KEAbilities.UseSelected(player);
            }

            if (settingBase is SSButton buttonSetting)
            {
                KECustomRole cr = KECustomRole.Get(player).FirstOrDefault();

                if(cr != null)
                {
                    cr.Show(player);
                }


            }


            foreach (DebugSetting debug in DebugSetting.settings)
            {
                debug.OnSettingValueReceived(player, settingBase);
                
            }


        }

        private Dictionary<Player, int> playerPos = new();

        private void Up(Player player)
        {
            if(KEAbilities.PlayersAbility.TryGetValue(player,out var list))
            {
                if (list.Count == 0)
                {
                    return;
                }
                if (!playerPos.ContainsKey(player))
                {
                    playerPos.Add(player, 0);
                }

                int index = mod((playerPos[player] - 1),list.Count);
                Log.Debug("up "+ index);
                KEAbilities ability = list[index];
                playerPos[player] += -1; 
                

                ability?.SelectAbility(player);
                KEAbilities.UpdateGUI(player);
            }
        }

        private int mod(int x, int m)
        {
            return (x % m + m) % m;
        }

        private void Down(Player player)
        {
            if (KEAbilities.PlayersAbility.TryGetValue(player, out var list))
            {
                if(list.Count == 0)
                {
                    return;
                }

                if (!playerPos.ContainsKey(player))
                {
                    playerPos.Add(player, 0);
                }
                

                int index = mod((playerPos[player] + 1), list.Count);
                Log.Debug("down " + index);
                KEAbilities ability = list[index];
                playerPos[player] += 1;
                ability?.SelectAbility(player);
                KEAbilities.UpdateGUI(player);
            }
        }

        public static bool CheckPressed(ServerSpecificSettingBase settingBase, int id)
        {
            return settingBase.SettingId == id &&
                (settingBase is SSKeybindSetting keybindSetting && keybindSetting.SyncIsPressed);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns>true if keybinds mode is activated ; false otherwise</returns>
        internal bool GetMode(Player p)
        {
            if (!SettingBase.TryGetSetting<TwoButtonsSetting>(p, _idMode, out var setting)) return setting.IsSecondDefault;
            
            return setting.IsFirst;
        }


        internal string GetArrow(Player p)
        {
            if (!SettingBase.TryGetSetting<UserTextInputSetting>(p, _idArrow, out var setting)) return baseArrow;

            return setting.Text;
            
        }
        internal float GetTime(Player p)
        {
            if (!SettingBase.TryGetSetting<SliderSetting>(p, _idTimeCustomRole, out var setting)) return 20;
            return setting.SliderValue;
        }
        internal float GetAbilityTime(Player p)
        {
            if (!SettingBase.TryGetSetting<SliderSetting>(p, _idTimeAbilityDesc, out var setting)) return 20;
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
