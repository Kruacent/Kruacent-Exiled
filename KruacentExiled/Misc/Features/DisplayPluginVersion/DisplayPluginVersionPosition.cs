using HintServiceMeow.Core.Enum;
using KE.Utils.API.Displays.DisplayMeow.Placements;

namespace KruacentExiled.Misc.Features.DisplayPluginVersion
{
    public class DisplayPluginVersionPosition : HintPosition
    {
        public override float Xposition => 0;

        public override float Yposition => 1050;
        public override string Name => "PluginVersion";

        public override HintAlignment HintAlignment => HintAlignment.Center;
    }
}
