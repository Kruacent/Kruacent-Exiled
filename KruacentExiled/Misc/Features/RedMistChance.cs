using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;
using KE.Utils.API.Features;
using KE.Utils.Extensions;
using KruacentExiled.CustomRoles.API.Features;
using LabApi.Features.Wrappers;
using PlayerRoles;

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
                    ev.IsAllowed = false;
                    kecr.AddRole(ev.Player);
                    ev.Player.Teleport(ev.Player.Zone.RandomSafeRoom());
                }
            }
        }
    }
}