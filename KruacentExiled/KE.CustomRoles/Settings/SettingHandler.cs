using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Exiled.Events.EventArgs.Player;
using HintServiceMeow.Core.Enum;
using HintServiceMeow.Core.Extension;
using HintServiceMeow.Core.Models.Hints;
using HintServiceMeow.Core.Utilities;
using KE.CustomRoles.API.Features;
using KE.Utils.API.Interfaces;
using KE.Utils.API.Settings;
using LabApi.Events.Arguments.PlayerEvents;
using System;
using System.Collections.Generic;
using System.Configuration;
using TMPro;
using UserSettings.ServerSpecific;
using static UnityEngine.Rendering.RayTracingAccelerationStructure;
using Hint = HintServiceMeow.Core.Models.Hints.Hint;

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
        private int _idTestHintslidery = 154;
        private int _idTestHintsliderx = 155;
        private int _idTestHintslidertime = 156;
        private int _idTestHintalign = 157;
        private int _idTestHintspawn = 158;
        private int _idTestHinttext = 159;
        private readonly int _idHeaderTestHint = 160;



        public static event Action<Player> DownPressed = delegate { };
        public static event Action<Player> UpPressed = delegate { };

        public static SettingHandler Instance { get; private set; }
        private List<SettingBase> settings;
        public readonly SettingsPage page;
        public readonly SettingsPage hintpage = null;
        public const string baseArrow = "<--";
        public SettingHandler()
        {
            Instance = this;
            settings = new List<SettingBase>()
            {
                new HeaderSetting(_idHeader,"Custom Roles",padding:true),
                
                new TwoButtonsSetting(_idDesc,"Descriptions","Disabled","Enabled",true,"hide/show the description the Custom Role "),
                new SliderSetting(_idTimeCustomRole,"Time shown",0,30,20),
                new SliderSetting(_idTimeAbilityDesc,"Ability Description time shown",0,30,20),
                new KeybindSetting(_idUp, "Select up", UnityEngine.KeyCode.None),
                new KeybindSetting(_idDown, "Select down", UnityEngine.KeyCode.None),
                new KeybindSetting(_idSelect, "Use selected ability", UnityEngine.KeyCode.None),
                SettingBase.Create(new SSPlaintextSetting(_idArrow, "Personalize the arrow next to the selected ability", baseArrow, 16, TMP_InputField.ContentType.Standard, string.Empty, 0))
            };



            string[] options = ["left", "center", "right"];

            if (MainPlugin.Instance.Config.Debug)
            {
                List<SettingBase>  hintscreator = new()
                {
                    new HeaderSetting(_idHeaderTestHint,"Hint creator",padding:true),
                    new SliderSetting(_idTestHintsliderx,"x",-2000,2000,0),
                    new SliderSetting(_idTestHintslidery,"y",0,2000,0),
                    new SliderSetting(_idTestHintslidertime,"time",0,10,5),
                    new DropdownSetting(_idTestHintalign,"alignment",options),
                    new ButtonSetting(_idTestHintspawn,"spawn","spawn"),
                    SettingBase.Create(new SSPlaintextSetting(_idTestHinttext,"text")),
                };



                settings.AddRange(hintscreator);
                created = true;

            }



            SettingBase.Register(settings);

            
        }

        private bool created = false;
        float xvalue = 0;
        float yvalue = 0;
        float timevalue = 5;
        HintAlignment HintAlignment = HintAlignment.Center;
        string text = "TEST";

        public void CreateHint(Player player, ServerSpecificSettingBase setting)
        {
            if (SettingBase.TryGetSetting<SliderSetting>(player, _idTestHintslidery, out var slidery))
            {
                yvalue = slidery.SliderValue;
            }

            if (SettingBase.TryGetSetting<SliderSetting>(player, _idTestHintsliderx, out var sliderx))
            {
                xvalue = sliderx.SliderValue;
            }

            if (SettingBase.TryGetSetting<SliderSetting>(player, _idTestHintslidertime, out var slidertime))
            {
                timevalue = slidertime.SliderValue;
            }

            if (SettingBase.TryGetSetting<UserTextInputSetting>(player, _idTestHinttext, out var textsetting))
            {
                text = textsetting.Text;
            }

            if (SettingBase.TryGetSetting<DropdownSetting>(player, _idTestHintalign, out var dropdown))
            {

                int selected = dropdown.SelectedIndex;
                if (selected == 0)
                {
                    HintAlignment = HintAlignment.Left;
                }
                if (selected == 1)
                {
                    HintAlignment = HintAlignment.Center;
                }
                if (selected == 2)
                {
                    HintAlignment = HintAlignment.Right;
                }
            }

            if (SettingBase.TryGetSetting<ButtonSetting>(player, _idTestHintspawn, out var button))
            {
                if(setting == button.Base)
                {
                    PlayerDisplay display = PlayerDisplay.Get(player);
                    Log.Debug($"creating hint at {xvalue},{yvalue} for {timevalue}");
                    var hint = new Hint()
                    {
                        XCoordinate = xvalue,
                        YCoordinate = yvalue,
                        Alignment = HintAlignment,
                        Text = text
                    };
                    display.ClearHint();
                    display.AddHint(hint);
                    hint.HideAfter(timevalue);

                }



            }


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
                KEAbilities.UseSelected(player);
            }
            if (created)
            {
                CreateHint(player, settingBase);
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
            return settingBase is SSKeybindSetting keybindSetting && keybindSetting.SettingId == id && keybindSetting.SyncIsPressed;
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
