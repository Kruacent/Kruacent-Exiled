using HintServiceMeow.Core.Enum;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.CR.CustomSCPs.SCP049C.Positions
{
    internal class SCP049CUnlockableTitlePosition : HintPosition
    {
        public override float Xposition => 0;

        public override float Yposition => 360;

        public override HintAlignment HintAlignment => HintAlignment.Center;
        public override string Name => "SCP049CUnlockableTitle";
    }


    internal class SCP049CUnlockableLeftPosition : HintPosition
    {
        public override float Xposition => -400;

        public override float Yposition => 450;

        public override HintAlignment HintAlignment => HintAlignment.Center;
        public override string Name => "SCP049CUnlockableLeft";
    }

    internal class SCP049CUnlockableRightPosition : HintPosition
    {
        public override float Xposition => 400;

        public override float Yposition => 450;

        public override HintAlignment HintAlignment => HintAlignment.Center;
        public override string Name => "SCP049CUnlockableRight";
    }
}
