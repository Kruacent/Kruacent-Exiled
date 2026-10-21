using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Features;
using KE.Utils.Extensions;
using UnityEngine;
using KruacentExiled.CustomRoles.API.Features;
using PlayerRoles;
using KruacentExiled.CustomSpawnPoint.Spawned;
using Exiled.API.Features;
using Exiled.API.Extensions;

namespace KruacentExiled.Misc.Features
{
    public class TeslaDeathRedMist
    {
        private readonly string[] profile = new[]
        {
            "76561199573165041@steam",
            "76561198356056056@steam",
            "76561198209599925@steam"
        };

        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Player.Dying += OnDying;
        }

        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Player.Dying -= OnDying;
        }

        private void OnDying(DyingEventArgs ev)
        {
            if (ev.Player == null) return;
            if (ev.Player.Role.Team != Team.FoundationForces) return;
            if (ev.DamageHandler.Type != DamageType.Tesla) return;

            KELog.Debug("Foundation force + tesla damage");

            bool grantRole = false;

            if (profile.Contains(ev.Player.UserId))
            {
                KELog.Debug("user id recognised");

                grantRole = true;
            }

            if (grantRole)
            {
                KELog.Debug("grant role true");

                if (KECustomRole.TryGet("CLASSD_RedMist", out KECustomRole kecr))
                {
                    if(kecr.CurrentNumberOfSpawn == 0)
                    {
                        ev.IsAllowed = false;
                        kecr.AddRole(ev.Player);
                        Room room = Room.List.GetRandomValue();
                        RoomSpawnedCustomPoint point = CustomSpawnPoint.RoomCustomPointHandler.GetRandomPoint(CustomSpawnPoint.RoomCustomPointType.Teleport, room);
                        Vector3 teleport;
                        if (point != null)
                        {
                            teleport = point.Position;
                        }
                        else
                        {
                            teleport = room.GetValidPosition();
                        }

                        ev.Player.Teleport(teleport);
                    }
                }
            }
        }
    }
}