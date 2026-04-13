using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Interfaces;
using KE.Utils.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features
{
    /// <summary>
    /// Everything about classD door
    /// </summary>
    public class ClassDDoor : IUsingEvents
    {
        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            
        }


        private void OnRoundStarted()
        {
            if(UnityEngine.Random.Range(0, 101) < MainPlugin.Instance.Config.ChanceClassDDoorGoesBoom)
            {
                HumanDoorsGoesBoom();
            }

        }


        private void HumanDoorsGoesBoom()
        {
            foreach(Player player in Player.List.Where(p => p.IsHuman))
            {
                if(player.CurrentRoom != null)
                {
                    foreach(Door door in player.CurrentRoom.Doors.Where(d => d is IDamageableDoor && !d.IsCheckpoint))
                    {
                        IDamageableDoor damageable = door as IDamageableDoor;
                        damageable.Break();
                    }
                }
            }
        }




    }
}
