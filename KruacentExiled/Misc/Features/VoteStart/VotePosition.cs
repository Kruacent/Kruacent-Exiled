using HintServiceMeow.Core.Enum;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Misc.Features.VoteStart
{
    public class VotePosition : HintPosition
    {
        public override float Xposition => 0;

        public override float Yposition => 860;
        public override string Name => "VoteStart";

        public override HintAlignment HintAlignment => HintAlignment.Center;
    }
}
