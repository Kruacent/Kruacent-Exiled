using Exiled.API.Features;
using Exiled.API.Features.Doors;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features.Auto079.Jobs
{
    public class LiberateScp : Job
    {

        protected override IEnumerator<float> Started()
        {

            var lockedDoors = npc.Scp.CurrentRoom.Doors.Where(d => !d.IsOpen && d.RequiredPermissions != Interactables.Interobjects.DoorUtils.DoorPermissionFlags.ScpOverride).ToList();

            foreach(Door d in lockedDoors)
            {

                int cost = (int)Math.Ceiling((float)(npc.Role.DoorStateChanger.GetCostForDoor(Interactables.Interobjects.DoorUtils.DoorAction.Opened,d.Base)) / 2);
                Log.Debug($"opening door {d.Name} ({cost})");

                yield return Timing.WaitUntilTrue(() => cost < npc.Role.Energy);
                d.IsOpen = true;
            }


        }
    }
}
