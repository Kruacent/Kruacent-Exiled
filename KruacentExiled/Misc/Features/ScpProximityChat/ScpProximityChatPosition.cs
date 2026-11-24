using HintServiceMeow.Core.Enum;
using KE.Utils.API.Displays.DisplayMeow.Placements;

namespace KruacentExiled.Misc.Features.ScpProximityChat
{
    public class ScpProximityChatPosition : HintPosition
    {
        public override float Xposition => -200;

        public override float Yposition => 100;
        public override string Name => "ScpProximityChat";

        public override HintAlignment HintAlignment => HintServiceMeow.Core.Enum.HintAlignment.Left;
    }
}
