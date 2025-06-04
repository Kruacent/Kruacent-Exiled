
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.Items.Interface;
using KE.Items.Lights;
using KE.Items.PickupModels;
using KE.Items.Upgrade;
using KE.Utils.Quality;
using KE.Utils.Quality.Settings;
using KE.Utils.Quality.Tests;
using MEC;
using PlayerRoles;
using PluginAPI.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            LightsHandler = new LightsHandler();
            //PickupQuality = new PickupQuality();

            Sound.LoadClips();

            

            //Exiled.Events.Handlers.Server.RoundStarted += Test;


            CustomItem.RegisterItems();
            //PickupQuality?.SubscribeEvents();
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

            base.OnDisabled();
            //QualityHandler = null;
            //PickupQuality = null;
            LightsHandler = null;
            Sound = null;
            UpgradeHandler = null;
            Instance = null;
        }



        private void TestQuality()
        {
            /*Vector3 pos = 
            Primitive c = Primitive.Create()

            QualityHandler.QualityToysHandler.SetQuality()*/
        }
    }
}