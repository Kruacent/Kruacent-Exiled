using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Mirror;
using PlayerRoles.FirstPersonControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utf8Json.Internal.DoubleConversion;

namespace KE.CustomRoles.CR.CustomSCPs.SCP049C.UnlockableAbilities.Tier1
{
    internal class HigherJumpComp : MonoBehaviour
    {

        private Player _player;
        private static readonly Vector3 _jumpgravity = UnityEngine.Vector3.up * -3;

        private void Awake()
        {
            _player = Player.Get(base.gameObject);
            IsActive = false;
        }

        public bool IsActive { get; set; }

        private void Update()
        {

            if(_player.Role is FpcRole fpc)
            {

                if (IsActive)
                {

                    Vector3 vector = (fpc.Velocity.y < 0f) ? FpcGravityController.DefaultGravity : _jumpgravity;
                    if (fpc.Gravity != vector)
                    {
                        fpc.Gravity = vector;
                    }
                }
                else
                {
                    fpc.Gravity = FpcGravityController.DefaultGravity;
                }
            }

        }

        private void OnDestroy()
        {
            if(_player.Role is FpcRole fpc)
            {
                fpc.Gravity = FpcGravityController.DefaultGravity;
            }
        }

        public void Destroy()
        {
            Destroy(this);
        }
    }
}
