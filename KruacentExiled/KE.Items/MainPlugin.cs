
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.Items.Interface;
using KE.Items.Lights;
using KE.Items.Upgrade;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;

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
        
        public override Version Version => new Version(1, 0, 0);
        
        public override void OnEnabled()
        {
            Instance = this;
            Sound = new Sound();
            UpgradeHandler = new UpgradeHandler();
            LightsHandler = new LightsHandler();

            CustomItem.RegisterItems();
            UpgradeHandler.SubscribeEvents();
            LightsHandler.SubscribeEvents();

            
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            CustomItem.UnregisterItems();
            UpgradeHandler.UnsubscribeEvents();
            LightsHandler.UnsubscribeEvents();


            base.OnDisabled();
            LightsHandler = null;
            Sound = null;
            UpgradeHandler = null;
            Instance = null;
        }

    }
}