using Exiled.API.Features;
using GEFExiled.GEFE.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEFExiled.GEFE.Examples.GE
{
    public class KIWIS : GlobalEvent
    {
        public override int Id { get; set; } = 32;
        public override string Name { get; set; } = "KIWIS";
        public override string Description { get; set; } = "Kill It While It's Small";
        public override double Weight { get; set; } = 1;
        public override IEnumerator<float> Start()
        {
            yield return 0;
        }

    }
}
