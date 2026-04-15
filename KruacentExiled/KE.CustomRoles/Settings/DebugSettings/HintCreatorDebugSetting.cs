using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using HintServiceMeow.Core.Enum;
using HintServiceMeow.Core.Extension;
using HintServiceMeow.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSettings.ServerSpecific;

using Hint = HintServiceMeow.Core.Models.Hints.Hint;

namespace KE.CustomRoles.Settings.DebugSettings
{
    internal class HintCreatorDebugSetting : DebugSetting
    {
        private int _idTestHintslidery = 154;
        private int _idTestHintsliderx = 155;
        private int _idTestHintslidertime = 156;
        private int _idTestHintalign = 157;
        private int _idTestHintspawn = 158;
        private int _idTestHinttext = 159;
        private int _idTestHintslidersize = 180;
        private int _idHeaderTestHint = 160;
        protected override List<SettingBase> CreateSettings()
        {
            string[] options = new string[] { "left", "center", "right" };
            created = true;
            return new List<SettingBase>()
            {
                new HeaderSetting(_idHeaderTestHint,"Hint creator",padding:true),
                new SliderSetting(_idTestHintsliderx,"x",-2000,2000,0),
                new SliderSetting(_idTestHintslidery,"y",0,2000,0),
                new SliderSetting(_idTestHintslidertime,"time",0,10,5),
                new SliderSetting(_idTestHintslidersize,"size",0,100,5),
                new DropdownSetting(_idTestHintalign,"alignment",options),
                new ButtonSetting(_idTestHintspawn,"spawn","spawn"),
                SettingBase.Create(new SSPlaintextSetting(_idTestHinttext,"text")),
            };
            
        }

        public override void OnSettingValueReceived(Player player, ServerSpecificSettingBase settingBase)
        {
            if (created)
            {
                CreateHint(player, settingBase);
            }
        }


        private bool created = false;
        float xvalue = 0;
        float yvalue = 0;
        float timevalue = 5;
        HintAlignment HintAlignment = HintAlignment.Center;
        string text = "TEST";
        float size = 10;

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
            if (SettingBase.TryGetSetting<SliderSetting>(player, _idTestHintslidersize, out var slidersize))
            {
                size = slidersize.SliderValue;
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
                if (setting == button.Base)
                {
                    PlayerDisplay display = PlayerDisplay.Get(player);
                    Log.Debug($"creating hint at {xvalue},{yvalue} for {timevalue}");
                    string fulltext = "<size=" + size + ">" + text + "</size>";

                    var hint = new Hint()
                    {
                        XCoordinate = xvalue,
                        YCoordinate = yvalue,
                        Alignment = HintAlignment,
                        Text = fulltext
                    };
                    display.ClearHint();
                    display.AddHint(hint);
                    hint.HideAfter(timevalue);

                }



            }


        }



    }
}
