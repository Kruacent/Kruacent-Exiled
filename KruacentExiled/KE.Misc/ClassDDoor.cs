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

namespace KE.Misc
{
    /// <summary>
    /// Everything about classD door
    /// </summary>
    internal class ClassDDoor
    {
        /// <summary>
        /// Class d door randomly explode at the start of the round
        /// </summary>
        internal void ClassDDoorGoesBoom()
        {
            if (UnityEngine.Random.Range(0, 101) < MainPlugin.Instance.Config.ChanceClassDDoorGoesBoom)
            {
                Log.Debug("ClassD's door exploded");
                foreach (Door door in Door.List)
                {
                    if (door.Type == DoorType.PrisonDoor)
                    {
                        if (door is IDamageableDoor dBoyDoor && !dBoyDoor.IsDestroyed)
                        {
                            dBoyDoor.Break();
                            
                        }
                    }
                }
            }
        }
    }
}
