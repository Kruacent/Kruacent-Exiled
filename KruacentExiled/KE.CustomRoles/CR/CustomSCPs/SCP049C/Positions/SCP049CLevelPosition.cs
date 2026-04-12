using HintServiceMeow.Core.Enum;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.CR.CustomSCPs.SCP049C.Positions
{
    public class SCP049CLevelPosition : HintPosition
    {
        public override float Xposition => 1060;

        public override float Yposition => 1030;

        public override HintAlignment HintAlignment => HintAlignment.Center;
        public override string Name => "SCP049CLevel";
    }
}
