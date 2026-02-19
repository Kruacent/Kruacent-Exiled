using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using HintServiceMeow.Core.Enum;
using HintServiceMeow.Core.Utilities;
using KE.Utils.API.KETextToy;
using System.Collections.Generic;
using UnityEngine;
using UserSettings.ServerSpecific;
using YamlDotNet.Core.Tokens;

namespace KE.CustomRoles.Settings.DebugSettings
{
    internal class FollowingTextToyDebugSetting : DebugSetting
    {

        private int _idTestHintspawn = 190;
        private int _idTestHinttext = 191;
        private int _idTestHintslidersize = 192;
        private int _idTestHintsliderx = 193;
        private int _idTestHintslidery = 194;
        private int _idTestHintsliderz = 195;
        private int _idHeaderTestHint = 196;
        private int _idTestHintdestroy = 197;
        protected override List<SettingBase> CreateSettings()
        {
            created = true;
            return 
                [
                    new HeaderSetting(_idHeaderTestHint,"Follow Text Creator",padding:true),
                    new SliderSetting(_idTestHintsliderx,"x",0,360,0),
                    new SliderSetting(_idTestHintslidery,"y",0,360,0),
                    new SliderSetting(_idTestHintsliderz,"z",0,360,0),
                    new SliderSetting(_idTestHintslidersize,"size",0,100,5),
                    SettingBase.Create(new SSPlaintextSetting(_idTestHinttext,"text")),
                    new ButtonSetting(_idTestHintspawn,"spawn","spawn"),
                    new ButtonSetting(_idTestHintdestroy,"destroyall","destroyall"),
                ];
        }
        private bool created = false;
        string text = "TEST";
        float size = 10;

        private float x = 0;
        private float y = 0;
        private float z = 0;

        public override void OnSettingValueReceived(Player player, ServerSpecificSettingBase settingBase)
        {

            if (created)
            {
                CreateTextToy(player, settingBase);
            }
        }

        private List<FollowingTextToy> texttoy = new();
        private void CreateTextToy(Player player, ServerSpecificSettingBase setting)
        {
            if (SettingBase.TryGetSetting<UserTextInputSetting>(player, _idTestHinttext, out var textsetting))
            {
                text = textsetting.Text;
            }

            if (SettingBase.TryGetSetting<SliderSetting>(player, _idTestHintslidersize, out var slidersize))
            {
                size = slidersize.SliderValue;
            }


            if (SettingBase.TryGetSetting<SliderSetting>(player, _idTestHintsliderx, out var sliderx))
            {
                x = sliderx.SliderValue;
            }
            if (SettingBase.TryGetSetting<SliderSetting>(player, _idTestHintslidery, out var slidery))
            {
                y = slidery.SliderValue;
            }
            if (SettingBase.TryGetSetting<SliderSetting>(player, _idTestHintsliderz, out var sliderz))
            {
                z = sliderz.SliderValue;
            }

            if (SettingBase.TryGetSetting<ButtonSetting>(player, _idTestHintspawn, out var button))
            {
                if (setting == button.Base)
                {
                    string fulltext = "<size=" + size + ">" + text + "</size>";

                    Quaternion rotation = Quaternion.Euler(x, y, z);

                    FollowingTextToy followtext = new FollowingTextToy(Player.List, player.Position, rotation, Vector3.one * size);
                    followtext.Toy.TextFormat = fulltext;
                    texttoy.Add(followtext);
                }
            }

            if (SettingBase.TryGetSetting<ButtonSetting>(player, _idTestHintdestroy, out var buttondestroy))
            {
                if (setting == buttondestroy.Base)
                {
                    foreach(var following in texttoy)
                    {
                        following.Destroy();
                    }
                }
            }

        }
    }
}
