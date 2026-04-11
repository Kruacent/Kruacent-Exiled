using DrawableLine;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using KE.Utils.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Utils;

namespace KE.CustomRoles.Abilities.RedMist.Spear
{
    public class SpearProjectile : MonoBehaviour
    {
        public static float Damage => Spear.Damage;
        private Primitive _primitive;
        private Vector3 _direction;
        private Player _player;

        public const float Speed = 30;

        public const float TimeBeforeMoving = 2f;
        private float timeBeforeMove = 0f;

        public void Init(Primitive primitive,Player player, Vector3 direction)
        {
            _player = player;
            _direction = direction;
            _primitive = primitive;
            DrawableLines.IsDebugModeEnabled = true;

        }

        public const byte Fallback = 200;
        private byte fallback = 0;
        private const float SpeedRotation = 90f;

        private static readonly Vector3 SizeProjectile = new Vector3(1,.2f,.2f);
        public static SpearProjectile Create(Player player, Vector3 direction)
        {
            var prim = Primitive.Create(player.Position, null, SizeProjectile, false);
            prim.MovementSmoothing = 0;
            prim.Collidable = false;
            prim.Spawn();

            SpearProjectile spear = prim.GameObject.AddComponent<SpearProjectile>();

            spear.Init(prim,player, direction);

            return spear;

        }


        private Collider[] NonAlloc = new Collider[64];
        private void Update()
        {
            if(fallback >= Fallback || _player == null || _player.GameObject == null)
            {
                Destroy();
                return;
            }

            if (timeBeforeMove <= TimeBeforeMoving)
            {
                timeBeforeMove += Time.deltaTime;
                _primitive.Rotation  = _primitive.Rotation * Quaternion.Euler(0, SpeedRotation* Time.deltaTime, 0);
                return;
            }

            _primitive.Rotation.SetLookRotation(_direction);


            UpdateDetect();
            UpdateMovement();
            
        }

        private void UpdateMovement()
        {

            Vector3 position = base.transform.position;
            float checkDistance = base.transform.localScale.z;

            DrawableLines.GenerateLine(.1f,Color.blue,position, position + _direction * checkDistance);

            if (Physics.Raycast(position, _direction, checkDistance, (int)LayerMasks.AttackMask))
            {
                KELog.Debug("hit wall");
                
                Destroy();
                return;
            
            }

            _primitive.Position = _primitive.Position + _direction * Speed * Time.deltaTime;
            fallback++;
        }


        private void UpdateDetect()
        {
            Vector3 position = base.transform.position;
            float radius = _primitive.Scale.x /2f;
            int detect = Physics.OverlapSphereNonAlloc(position, radius, NonAlloc, (int)LayerMasks.Hitbox);

            

            DrawableLines.GenerateSphere(position, radius, duration:.1f);

            KELog.Debug("detect=" + detect);

            for (int i = 0; i < detect; i++)
            {
                Collider collider = NonAlloc[i];
                
                if (Player.TryGet(collider, out Player target))
                {
                    KELog.Debug("collider layer=" + collider.gameObject.layer);

                    if (target == _player)
                    {
                        continue;
                    }

                    if (HitboxIdentity.IsDamageable(_player.ReferenceHub, target.ReferenceHub))
                    {
                        KELog.Debug("hurt "+ target.Nickname);
                        target.Hurt(_player, Damage, Exiled.API.Enums.DamageType.Scp1509);
                        Destroy();
                    }


                }
            }
        }


        public void Destroy()
        {
            KELog.Debug($"destroyed ({fallback}/{Fallback})");

            _primitive?.Destroy();
            Destroy(this);
        }

    }
}
