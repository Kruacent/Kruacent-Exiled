using Exiled.Events.EventArgs.Interfaces;
using KE.GlobalEventFramework.GEFE.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.GlobalEventFramework.GEFE.Events.EventArgs
{
    public class DisabledEventArgs : IExiledEvent
    {
        
        public KEEvents Event { get; }

        public DisabledEventArgs(KEEvents ev)
        {
            Event = ev;
        }
    }
}
