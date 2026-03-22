using HintServiceMeow.Core.Enum;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.CR.CustomSCPs
{
    public class RemainingPlayerPosition : HintPosition
    {
        public override float Xposition => 1820;

        public override float Yposition => 60;

        public override HintAlignment HintAlignment => HintAlignment.Center;
    }

    public class LogoPosition : HintPosition
    {
        public override float Xposition => 1700;

        public override float Yposition => 60;

        public override HintAlignment HintAlignment => HintAlignment.Center;
    }
}
