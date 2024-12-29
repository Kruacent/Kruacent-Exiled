
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;

namespace ArmeKruacent
{
    public class MainPlugin : Plugin<Config>
    {

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