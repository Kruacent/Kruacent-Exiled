using KE.BlackoutNDoor.API.Features;
using Exiled.API.Features;
using MEC;
using System.Collections.Generic;
using System;

namespace KE.BlackoutNDoor.Handlers
{
    public class ServerHandler
    {
        [Obsolete()]
        public int Cooldown { get; set; }
        internal double ChanceBO { get; set; } = MainPlugin.Instance.Config.InitialChanceBO;
        private static CoroutineHandle Handle;




    }
}
