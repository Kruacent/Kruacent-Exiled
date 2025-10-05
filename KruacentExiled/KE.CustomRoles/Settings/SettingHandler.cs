using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Exiled.Events.EventArgs.Player;
using KE.CustomRoles.API.Features;
using KE.Utils.API.Interfaces;
using System;
using System.Collections.Generic;
using TMPro;
using UserSettings.ServerSpecific;

namespace KE.CustomRoles.Settings
{
    internal class SettingHandler : IUsingEvents
    {
        private int _idHeader = 148;
        private int _idTimeCustomRole = 144;
        private int _idDesc = 143;
        private int _idMode = 145;
        private int _idDown = 149;
        private int _idUp = 150;
        private int _idSelect = 151;
        private int _idArrow = 152;
        private int _idTimeAbilityDesc = 153;



        public static event Action<Player> DownPressed = delegate { };
        public static event Action<Player> UpPressed = delegate { };

        public static SettingHandler Instance { get; private set; }
        private List<SettingBase> settings;
        public const string baseArrow = "<--";
        public SettingHandler()
        {
            Instance = this;
            settings = new List<SettingBase>()
            {
                new HeaderSetting (_idHeader,"Custom Roles Settings"),
                new TwoButtonsSetting(_idDesc,"Descriptions","Disabled","Enabled",true,"hide/show the description the Custom Role "),
                new SliderSetting(_idTimeCustomRole,"Time shown",0,30,20),
                new SliderSetting(_idTimeAbilityDesc,"Ability Description time shown",0,30,20),
                new TwoButtonsSetting(_idMode,"Mode","Keybinds","Select wheel",true,onChanged:OnChanged),
                new KeybindSetting(_idUp, "Select up", UnityEngine.KeyCode.None,hintDescription:"only work in Select Wheel mode"),
                new KeybindSetting(_idDown, "Select down", UnityEngine.KeyCode.None,hintDescription:"only work in Select Wheel mode"),
                new KeybindSetting(_idSelect, "Use selected ability", UnityEngine.KeyCode.None,hintDescription:"only work in Select Wheel mode"),
                //this crashes the player idk why
                SettingBase.Create(new SSPlaintextSetting(_idArrow, "Personalize the arrow next to the selected ability", baseArrow, 16, TMP_InputField.ContentType.Standard, "only work in Select Wheel mode", 0))
            };
        }

        public void OnChanged(Player player, SettingBase setting)
        {
            
            try
            {
                if (GetMode(player))
                {
                    KEAbilities.RemoveAllSelect(player);
                }
                else
                {
                    KEAbilities.SelectFirstAbility(player);
                }
            }
            catch(Exception e)
            {
                Log.Error(e);
            }
            
            
        }


        public void SubscribeEvents()
        {
            SettingBase.Register(settings);
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += SafeOnSettingValueReceived;
            Exiled.Events.Handlers.Player.Verified += OnVerified;
            DownPressed += Down;
            UpPressed += Up;

        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Verified -= OnVerified;
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= SafeOnSettingValueReceived;
            DownPressed -= Down;
            UpPressed -= Up;
            SettingBase.Unregister(predicate:null, settings);
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
            if (KEAbilities.CheckPressed(settingBase, _idDown))
            {
                DownPressed?.Invoke(player);
            }
            
            if (KEAbilities.CheckPressed(settingBase, _idUp))
            {
                UpPressed?.Invoke(player);
            }

            if (KEAbilities.CheckPressed(settingBase, _idSelect))
            {
                KEAbilities.UseSelected(player);
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



        

        private void OnVerified(VerifiedEventArgs ev)
        {
            ServerSpecificSettingsSync.SendToPlayer(ev.Player.ReferenceHub);
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
