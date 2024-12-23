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
    public class R : GlobalEvent
    {
        public override int Id { get; set; } = 32;
        public override string Name { get; set; } = "nothing";
        public override string Description { get; set; } = "y'a r";
        public override int Weight { get; set; } = 1;
        public override IEnumerator<float> Start()
        {
            yield return 0;
        }

    }
}
