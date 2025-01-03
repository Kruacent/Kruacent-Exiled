﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
using MEC;

namespace KE.Misc
{
    internal class ServerHandler
    {
        public void OnRoundStarted()
        {
            if(MainPlugin.Instance.Config.ChanceFF >= 0 && MainPlugin.Instance.Config.ChanceFF <= 100)
                MainPlugin.Instance.RandomFF();
            if (MainPlugin.Instance.Config.ChanceClassDDoorGoesBoom >= 0 && MainPlugin.Instance.Config.ChanceClassDDoorGoesBoom <= 100)
                MainPlugin.Instance.ClassDDoor.ClassDDoorGoesBoom();
            if(MainPlugin.Instance.Config.AutoNukeAnnoucement)
                Timing.RunCoroutine(MainPlugin.Instance.NukeAnnouncement());
            if(MainPlugin.Instance.Config.PeanutLockDown && Player.List.Where(p => p.Role.Type == PlayerRoles.RoleTypeId.Scp173).Count() > 0)
                Timing.RunCoroutine(MainPlugin.Instance.PeanutLockdown());
            if(MainPlugin.Instance.Config.AutoElevator)
                Timing.RunCoroutine(MainPlugin.Instance.AutoElevator.StartElevator());
        }
    }
}
