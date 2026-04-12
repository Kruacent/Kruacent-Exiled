using HintServiceMeow.Core.Enum;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.API.Core.Positions
{
    public class UltraPosition : HintPosition
    {
        public override float Xposition => 740;

        public override float Yposition => 280;
        public override string Name => "Ultra";

        public override HintAlignment HintAlignment => HintAlignment.Right;
    }
}
