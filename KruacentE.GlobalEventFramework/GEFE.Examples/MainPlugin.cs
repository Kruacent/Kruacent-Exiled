using Exiled.API.Features;
using MEC;
using GEFExiled.GEFE.API.Features;

namespace GEFExiled.GEFE.Examples
{
    public class MainPlugin : Plugin<Config>
    {

        public override void OnEnabled()
        {



            Timing.CallDelayed(10f, () => {
                GlobalEvent.RegisterItems();
            });
            base.OnEnabled();
        }
    }
}
