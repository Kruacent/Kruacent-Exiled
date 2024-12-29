using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;

namespace GEFE.Examples
{
    internal class MainPlugin : Plugin<Config>
    {
        public override string Author => "Patrique & OmerGS";

        public override Version Version => new Version(1, 0, 0);
        public override string Name => "KE.GEF.Examples";
        public override void OnEnabled()
        {
            
        }

        public override void OnDisabled()
        {
            
        }
    }
}
