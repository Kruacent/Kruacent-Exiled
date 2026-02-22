using HintServiceMeow.Core.Enum;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features.PatchNotes
{
    public class PatchNotesPosition : HintPosition
    {
        public override float Xposition => -240;

        public override float Yposition => 300;
        public override string Name => "PatchNotes";

        public override HintAlignment HintAlignment => HintAlignment.Left;
    }
}
