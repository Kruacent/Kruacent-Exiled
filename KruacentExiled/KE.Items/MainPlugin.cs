
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using KE.Items.Lights;
using KE.Items.Settings;
using KE.Items.Upgrade;
using KE.Utils.API.Displays.DisplayMeow;
using PlayerRoles;
using System;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.Items
{
    public class MainPlugin : Plugin<Config>
    {
        public override string Author => "Patrique & OmerGS";
        public override string Name => "KEItems";
        internal Sound Sound { get; private set; }
        internal UpgradeHandler UpgradeHandler { get; private set; }
        internal LightsHandler LightsHandler { get; private set; }
        internal static MainPlugin Instance { get; private set; }
        internal SettingsHandler SettingsHandler { get; private set; }

        internal static readonly HintPlacement ItemEffectPlacement = new(0, 200, HintServiceMeow.Core.Enum.HintAlignment.Center);
        internal static readonly HintPlacement HintPlacement = new(0, 400, HintServiceMeow.Core.Enum.HintAlignment.Center);

        //scrapped
        //internal PickupQuality PickupQuality { get; private set; }
        //internal QualityHandler QualityHandler { get; private set; }

        public override PluginPriority Priority => PluginPriority.Low;
        public override Version Version => new (1, 0, 0);
        
        public override void OnEnabled()
        {
            Instance = this;
            Sound = new Sound();
            //QualityHandler = QualityHandler.Instance;
            //QualityHandler.Register();
            UpgradeHandler = new UpgradeHandler();
            LightsHandler = new LightsHandler(); // lights color and intensity broken
            //PickupQuality = new PickupQuality();
            SettingsHandler = new();

            Sound.LoadClips();



            //Exiled.Events.Handlers.Server.RoundStarted += Test;


            CustomItem.RegisterItems();
            //PickupQuality?.SubscribeEvents();
            SettingsHandler.SubscribeEvents();
            UpgradeHandler.SubscribeEvents();
            LightsHandler.SubscribeEvents();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            CustomItem.UnregisterItems();
            UpgradeHandler?.UnsubscribeEvents();
            LightsHandler?.UnsubscribeEvents();
            //PickupQuality?.UnsubscribeEvents();
            //QualityHandler?.Unregister();
            SettingsHandler.UnsubscribeEvents();

            //QualityHandler = null;
            //PickupQuality = null;
            SettingsHandler = null;
            LightsHandler = null;
            Sound = null;
            UpgradeHandler = null;
            Instance = null;
        }





    }
}