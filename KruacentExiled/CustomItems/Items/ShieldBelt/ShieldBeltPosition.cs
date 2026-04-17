using HintServiceMeow.Core.Enum;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.CustomItems.Items.ShieldBelt
{
    public class ShieldBeltPosition : HintPosition
    {
        public override float Xposition => 0;

        public override float Yposition => 1050;

        public override HintAlignment HintAlignment => HintAlignment.Center;

        public override string Name => "ShieldBelt";
    }
}
