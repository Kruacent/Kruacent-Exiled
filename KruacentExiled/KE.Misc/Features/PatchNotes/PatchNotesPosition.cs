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
        public override float Xposition => -340;

        public override float Yposition => 500;
        public override string Name => "PatchNotes";

        public override HintAlignment HintAlignment => HintAlignment.Left;
    }
}
