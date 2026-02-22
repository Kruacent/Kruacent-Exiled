using CustomPlayerEffects;
using Exiled.API.Features;
using ProjectMER.Commands.Modifying.Scale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace KE.Map.Surface.ElevatorGateA
{
    public class CustomKillerCollision : MonoBehaviour
    {

        private void Update()
        {
            foreach(Player player in Player.List)
            {
                
                if (PitDeath.ValidatePlayer(player.ReferenceHub))
                {
                    if (InBound(player))
                    {
                        PitDeath pit = player.GetEffect<PitDeath>();
                        if (!pit.IsEnabled)
                        {
                            pit.IsEnabled = true;
                        }
                        
                    }
                }
            }

        }

        private bool InBound(Player player)
        {
            Vector3 position = transform.position;
            Vector3 playerPosition = player.Position;
            Vector3 halfSize = transform.lossyScale / 2;
            return playerPosition.x >= position.x - halfSize.x &&
                   playerPosition.x <= position.x + halfSize.x &&
                   playerPosition.y >= position.y - halfSize.y &&
                   playerPosition.y <= position.y + halfSize.y &&
                   playerPosition.z >= position.z - halfSize.z &&
                   playerPosition.z <= position.z + halfSize.z;
        }

    }
}
