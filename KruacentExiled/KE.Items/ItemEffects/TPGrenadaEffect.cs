using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Items.Interface;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Items.ItemEffects
{
    public class TPGrenadaEffect : CustomItemEffect
    {
        private List<Player> effectedPlayers = new List<Player>();
        [Description("What roles will not be able to be affected by Implosion Grenades. Keeping SCP-173 on this list is highly recommended.")]
        public HashSet<RoleTypeId> BlacklistedRoles { get; set; } = new HashSet<RoleTypeId>() { RoleTypeId.Scp173, RoleTypeId.Scp106, RoleTypeId.Scp049, RoleTypeId.Scp096, RoleTypeId.Scp3114, RoleTypeId.Scp0492, RoleTypeId.Scp939 };

        public override void Effect(UsedItemEventArgs ev)
        {
            OnExploding(new HashSet<Player>() { ev.Player });
        }
        public override void Effect(DroppingItemEventArgs ev)
        {
            OnExploding(new HashSet<Player>() { ev.Player });
        }

        public override void Effect(ExplodingGrenadeEventArgs ev)
        {
            OnExploding(ev.TargetsToAffect,ev.Projectile);
        }



        private void OnExploding(HashSet<Player> targets, EffectGrenadeProjectile projectile = null)
        {

            effectedPlayers = ListPool<Player>.Pool.Get();
            foreach (Player player in targets)
            {
                if (BlacklistedRoles.Contains(player.Role))
                    continue;
                try
                {
                    bool line;
                    if (projectile == null)
                        line = Physics.Linecast(projectile.Transform.position, player.Position);
                    else
                        line = true;

                    if (line)
                    {
                        effectedPlayers.Add(player);
                        player.Teleport(RandomRoom());
                    }
                }
                catch (Exception exception)
                {
                    Log.Error($"{nameof(OnExploding)} error: {exception}");
                }
            }
        }



        private Room RandomRoom()
        {
            Room room = Room.Random();
            if (Warhead.IsDetonated)
            {
                return Room.Random(ZoneType.Surface);
            }

            if (Map.IsLczDecontaminated)
            {
                float random = UnityEngine.Random.value;
                Log.Debug($"random={random}");
                if (random <= 0.33f)
                {
                    return Room.Random(ZoneType.HeavyContainment);
                }
                if (random > 0.33f && random <= 0.66f)
                {
                    return Room.Random(ZoneType.Entrance);
                }
                return Room.Random(ZoneType.Surface);
            }
            Log.Debug($"roomZone={room.Zone}");
            return room;
        }
    }
}
