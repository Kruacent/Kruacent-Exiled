using HintServiceMeow.Core.Enum;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.CustomRoles.Abilities.RedMist.GreaterSplitHorizontal
{
    public class DebugPosition : HintPosition
    {
        public override float Xposition => 800;
        public override float Yposition => 400;
        public override HintAlignment HintAlignment => HintAlignment.Center;
        public override string Name => "DebugGreaterSplit";
    }
}
