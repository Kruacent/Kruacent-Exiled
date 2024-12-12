using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Interfaces;
using KE.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.KruacentE.Misc
{
    internal class ClassDDoor
    {
        internal void ClassDDoorGoesBoom()
        {
            if (UnityEngine.Random.Range(0, 101) < MainPlugin.Instance.Config.ChanceClassDDoorGoesBoom)
            {
                foreach (Door door in Door.List)
                {
                    if (door.Type == DoorType.PrisonDoor)
                    {
                        if (door is IDamageableDoor dBoyDoor && !dBoyDoor.IsDestroyed)
                        {
                            dBoyDoor.Break();
                            Log.Debug("ClassD's door exploded");
                        }
                    }
                }
            }
        }
    }
}
