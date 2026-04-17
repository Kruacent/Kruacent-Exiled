using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using ProjectMER.Commands.Modifying.Scale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace KruacentExiled.Map.Surface.ElevatorGateA
{
    public class CustomKillerCollision : MonoBehaviour
    {
        // use OnTriggerEnter instead
        private Collider[] NonAlloc = new Collider[8];


        private void Update()
        {
            if(Physics.OverlapBoxNonAlloc(transform.localPosition, transform.localScale / 2f, NonAlloc, transform.localRotation, (int)LayerMasks.Hitbox) > 0)
            {
                for (int i = 0; i < NonAlloc.Length; i++)
                {
                    if(Player.TryGet(NonAlloc[i],out Player player))
                    {
                        if (PitDeath.ValidatePlayer(player.ReferenceHub))
                        {
                            PitDeath pit = player.GetEffect<PitDeath>();
                            //DO NOT USE PitDeath.KillPlayer()
                            if (!pit.IsEnabled)
                            {
                                pit.IsEnabled = true;
                            }
                        }
                    }
                }
            }


        }

    }
}
