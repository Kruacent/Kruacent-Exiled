using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using HarmonyLib;
using KE.Utils.API.Displays.DisplayMeow;
using KruacentExiled;
using KruacentExiled.CustomItems.API.Core.Lights;
using KruacentExiled.CustomItems.API.Core.Settings;
using KruacentExiled.CustomItems.API.Core.Upgrade;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomSpawnPoint;
using MEC;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KruacentExiled.CustomItems
{
    public class MainPlugin : KEPlugin
    {
        public override string Name => "KE.Items";
        public override string Prefix => "KE.I";
        internal UpgradeHandler UpgradeHandler { get; private set; }
        internal LightsHandler LightsHandler { get; private set; }
        internal static MainPlugin Instance { get; private set; }
        internal SettingsHandler SettingsHandler { get; private set; }

        public override IConfig Config => null;

        internal static readonly HintPlacement ItemEffectPlacement = new HintPlacement(0, 200, HintServiceMeow.Core.Enum.HintAlignment.Center);
        internal static readonly HintPlacement HintPlacement = new HintPlacement(0, 400, HintServiceMeow.Core.Enum.HintAlignment.Center);

        
        public override void OnEnabled()
        {
            Instance = this;
            UpgradeHandler = new UpgradeHandler();
            LightsHandler = new LightsHandler();
            SettingsHandler = new SettingsHandler();


            KE.Utils.API.Sounds.SoundPlayer.Load();

            KECustomItem.RegisterItems();
            UpgradeHandler.SubscribeEvents();
            LightsHandler.SubscribeEvents();
        }


        public override void OnDisabled()
        {
            KECustomItem.UnregisterItems();
            UpgradeHandler?.UnsubscribeEvents();
            LightsHandler?.UnsubscribeEvents();

            SettingsHandler = null;
            LightsHandler = null;
            UpgradeHandler = null;
            Instance = null;
        }


    }
}