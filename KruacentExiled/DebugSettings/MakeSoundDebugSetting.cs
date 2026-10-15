using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using HintServiceMeow.Core.Enum;
using KE.Utils.API.KETextToy;
using KruacentExiled.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UserSettings.ServerSpecific;

namespace KruacentExiled.DebugSettings
{
    internal class MakeSoundDebugSetting : DebugSetting
    {
        private int _idHeaderTestHint = 300;
        private int _idSoundType = 301;
        private int _idClipName = 302;
        private int _idStart = 303;
        private int _idSetPosition = 304;
        protected override List<SettingBase> CreateSettings()
        {
            string[] options = new string[] { "Noise", "Music" };
            string[] options2 = KE.Utils.API.Sounds.SoundPlayer.clips.ToArray();

            return new List<SettingBase>()
            {
                new HeaderSetting(_idHeaderTestHint, "Audio spawn", padding: true),
                new DropdownSetting(_idSoundType,"Sound Type",options),
                new DropdownSetting(_idClipName,"clip name",options2),
                new TwoButtonsSetting(_idSetPosition,"Follow Player","yes","no"),
                new ButtonSetting(_idStart, "spawn", "spawn"),
            };
        }

        private SoundType type = SoundType.Music;
        private string clipName = string.Empty;
        private bool followPlayer = false;
        public override void OnSettingValueReceived(Player player, ServerSpecificSettingBase settingBase)
        {


            if (SettingBase.TryGetSetting<DropdownSetting>(player, _idSoundType, out var dropdown))
            {
                if (Enum.TryParse<SoundType>(dropdown.SelectedOption, out var sound))
                {
                    type = sound;
                }


            }

            if (SettingBase.TryGetSetting<DropdownSetting>(player, _idClipName, out var textsetting))
            {
                clipName = textsetting.SelectedOption;
            }

            if (SettingBase.TryGetSetting<TwoButtonsSetting>(player, _idSetPosition, out var followsetting))
            {
                followPlayer = followsetting.IsFirst;
            }


            if (SettingBase.TryGetSetting<ButtonSetting>(player, _idStart, out var button))
            {
                if(button.Base == settingBase)
                {
                    if (followPlayer)
                    {
                        MainPlugin.AudioHandler.Play(player, type, clipName);
                    }
                    else
                    {
                        MainPlugin.AudioHandler.Play(player, type, clipName,player.Position);
                    }                    
                    Log.Info("playing " + clipName);
                }

            }

        }


        
    }
}
