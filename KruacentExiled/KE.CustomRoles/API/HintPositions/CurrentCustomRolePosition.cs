using HintServiceMeow.Core.Enum;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.API.HintPositions
{
    public class CurrentCustomRolePosition : HintPosition
    {
        public override float Xposition => 600;

        public override float Yposition => 1030;

        public override HintAlignment HintAlignment => HintAlignment.Center;
    }
}
