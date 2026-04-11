using HintServiceMeow.Core.Enum;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features.GamblingCoin
{
    public class CoinHintPosition : HintPosition
    {
        public override float Xposition => 0;

        public override float Yposition => 800;

        public override HintAlignment HintAlignment => HintAlignment.Center;
        public override string Name => "Coins";
    }
}
