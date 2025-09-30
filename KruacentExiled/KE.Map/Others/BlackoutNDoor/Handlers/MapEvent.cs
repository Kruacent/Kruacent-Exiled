using Exiled.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Map.Others.BlackoutNDoor.Handlers
{
    public abstract class MapEvent
    {

        public abstract string Cassie { get; }
        public abstract string CassieTranslated { get; }


        public abstract void Start(ZoneType zone);


        public abstract void Stop(ZoneType zone);

    }
}
