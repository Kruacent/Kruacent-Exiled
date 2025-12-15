using HintServiceMeow.Core.Enum;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features.LastHuman
{
    public class LastHumanPosition : HintPosition
    {
        public override float Xposition => 0;

        public override float Yposition => 400;
        public override string Name => "LastHuman";

        public override HintAlignment HintAlignment => HintAlignment.Center;
    }
}
