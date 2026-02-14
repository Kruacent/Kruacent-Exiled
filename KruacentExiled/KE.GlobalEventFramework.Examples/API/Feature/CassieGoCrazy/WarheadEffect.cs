using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.GlobalEventFramework.Examples.API.Feature.CassieGoCrazy
{
    // Starting the Warhead
    public class WarheadEffect : ICGCEffect
    {
        public void Effect()
        {
            if (!Warhead.IsDetonated)
            {
                Warhead.Start();
            }
        }
    }
}
