
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using KE.Items.API.Core.Lights;
using KE.Items.API.Core.Settings;
using KE.Items.API.Core.Upgrade;
using KE.Utils.API.Displays.DisplayMeow;
using System;
using System.Linq;
using InteractableToy = LabApi.Features.Wrappers.InteractableToy;

namespace KE.Items
{
    public class MainPlugin : Plugin<Config>
    {
        public override string Author => "Patrique & OmerGS";
        public override string Name => "KE.Items";
        public override string Prefix => "KE.I";
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
            //QualityHandler = QualityHandler.Instance;
            //QualityHandler.Register();
            UpgradeHandler = new UpgradeHandler();
            LightsHandler = new LightsHandler();
            //PickupQuality = new PickupQuality();
            SettingsHandler = new();

            Utils.API.Sounds.SoundPlayer.Load();
            

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
            UpgradeHandler = null;
            Instance = null;
        }






        private void Test()
        {
            Player player = Player.List.First();



            var prim = Primitive.Create(player.Position,spawn:false);
            prim.Collidable = false;
            prim.Spawn();
            var itoy = InteractableToy.Create(prim.Transform, true);
            itoy.InteractionDuration = 3f;


            itoy.OnSearchAborted += GiveDestroy;

        }

        private void GiveDestroy(LabApi.Features.Wrappers.Player player)
        {

            CustomItem.Get(1046)?.Give(player);

        }
        
    }
}