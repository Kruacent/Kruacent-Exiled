using Exiled.API.Features;
using Exiled.API.Features.Toys;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KruacentExiled.CustomRoles.CR.MTF.Medic
{
    internal class HealTeammatesComp : MonoBehaviour
    {
        private Player _player;
        private Primitive _zonePrimitive;
        private CoroutineHandle _healCoroutine;

        private static readonly float ZoneRadius = 5f;
        private static readonly float HealInterval = 0.5f;
        private static readonly int HealPerTick = 5;

        public bool IsActive { get; set; } = true;

        private void Awake()
        {
            _player = Player.Get(gameObject);
        }

        private void Update()
        {
            if (_zonePrimitive != null && _player != null && _player.IsAlive)
            {
                _zonePrimitive.Position = _player.Position - new Vector3(0, 0.5f, 0);
            }
        }

        public void StartHealingZone(float duration = 20f)
        {
            IsActive = true;
            _zonePrimitive = Primitive.Create(PrimitiveType.Cylinder, _player.Position, null, new Vector3(ZoneRadius * 2, 0.05f, ZoneRadius * 2), false);
            _zonePrimitive.Visible = false;
            _zonePrimitive.Collidable = false;
            _zonePrimitive.Spawn();

            _healCoroutine = Timing.RunCoroutine(HealZoneRoutine(duration));
        }

        private IEnumerator<float> HealZoneRoutine(float duration)
        {
            float elapsed = 0f;
            
            while (elapsed < duration && IsActive)
            {
                if (_player == null || !_player.IsAlive)
                    break;

                Team medicTeam = _player.Role.Team;

                foreach (Player target in Player.List)
                {
                    if (target == null || !target.IsAlive) continue;

                    if (IsPlayerInZone(target, _player.Position, ZoneRadius))
                    {
                        if (target.Role.Team == medicTeam)
                        {
                           target.Heal(HealPerTick);
                        }
                    }
                }

                elapsed += HealInterval;
                yield return Timing.WaitForSeconds(HealInterval);
            }

            DestroyZoneAndComponent();
        }

        private bool IsPlayerInZone(Player player, Vector3 zonePosition, float radius)
        {
            float distance = Vector3.Distance(new Vector3(player.Position.x, 0, player.Position.z),
                                               new Vector3(zonePosition.x, 0, zonePosition.z));
            return distance <= radius / 2;
        }

        private void OnDestroy()
        {
            Timing.KillCoroutines(_healCoroutine);

            if (_zonePrimitive != null)
            {
                _zonePrimitive.UnSpawn();
                _zonePrimitive.Destroy();
                _zonePrimitive = null;
            }
        }

        public void DestroyZoneAndComponent()
        {
            IsActive = false;

            if (_zonePrimitive != null)
            {
                _zonePrimitive.UnSpawn();
                _zonePrimitive.Destroy();
                _zonePrimitive = null;
            }

            Timing.KillCoroutines(_healCoroutine);
            Destroy(this);
        }
    }
}