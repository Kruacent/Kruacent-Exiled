
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using System;

namespace ArmeKruacent
{
    public class MainPlugin : Plugin<Config>
    {
        public override string Author => "Patrique & OmerGS";
        public override string Name => "KEItems";
        public override Version Version => new Version(1, 0, 0);
        public override void OnEnabled()
        {

            CustomItem.RegisterItems();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            CustomItem.UnregisterItems();

            base.OnDisabled();
        }
    }
}