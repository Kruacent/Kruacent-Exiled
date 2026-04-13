using HintServiceMeow.UI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Map.Surface.Rooms
{
    public abstract class SurfaceRoom
    {
        public const string ZoneName = "Surface";
        protected abstract string RoomName { get; }
        public string Name => ZoneName + RoomName;

    }
}
