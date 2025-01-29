using Exiled.Events.EventArgs.Scp330;
using Exiled.Events.Patches.Events.Scp330;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventorySystem.Items.Usables.Scp330;

namespace KE.Misc
{
    internal class Candy
    {
        public void InteractingScp330(InteractingScp330EventArgs ev)
        {
            if(UnityEngine.Random.Range(0, 100) < MainPlugin.Instance.Config.ChancePinkCandy)
            {
                ev.Candy = CandyKindID.Pink;
            }
        }
    }
}
