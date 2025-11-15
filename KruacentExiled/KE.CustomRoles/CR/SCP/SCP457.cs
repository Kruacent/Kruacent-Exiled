using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Toys;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Scp106;
using KE.CustomRoles.Abilities;
using KE.CustomRoles.API.Features;
using KE.Utils.API;
using MEC;
using PlayerRoles;
using PlayerStatsSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.CustomRoles.CR.SCP
{
    public class SCP457 : CustomSCP
    {

        public override string Description { get; set; } = "You do passive damage around you, and can lauch fireballs";
        public override string PublicName { get; set; } = "SCP-457";
        public override int MaxHealth { get; set; } = 5000;
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scp106;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override bool IsSupport { get; } = false;

        public override float SpawnChance { get; set; } = 100;

        private Dictionary<Player, Light> _inside = new();
        private Dictionary<Player, HashSet<CoroutineHandle>> _handles = new();
        

        public static float DamageRefreshRate = 5f;
        public static readonly Color FlameColor = new(2, 1.08f, 0);



        public override HashSet<string> Abilities => 
        [
            "Fireball"
        ];


        protected override void RoleAdded(Player player)
        {
            Log.Debug("adding role 457");
            _inside.Add(player, null);
            _handles.Add(player, new());

            _handles[player].Add(Timing.RunCoroutine(InsideLight(player)));
            _handles[player].Add(Timing.RunCoroutine(PassiveDamage(player)));

        }

        private IEnumerator<float> InsideLight(Player player)
        {
            Light light = Light.Create();
            light.Intensity = .5f;
            _inside[player] = light;
            light.Position = player.Position;
            light.Color = FlameColor;
            light.Spawn();
            while (true)
            {
                light.Position = player.Position;
                PassiveDamage(player);
                yield return Timing.WaitForOneFrame;
            }
        }



        private IEnumerator<float> PassiveDamage(Player scp)
        {
            while (scp.IsAlive)
            {
                foreach (Player allP in Player.List.Where(p => p != scp && p.Role.Side != scp.Role.Side))
                {


                    if (OtherUtils.IsInCircle(allP.Position, scp.Position, 5))
                    {

                        Physics.Linecast(allP.Position, scp.Position, out var hitinfo);
                        float damage = -(hitinfo.distance / 3) + 10;
                        Log.Debug($"damâge={damage} dist = {hitinfo.distance}");
                        allP.EnableEffect(Exiled.API.Enums.EffectType.Burned, DamageRefreshRate, true);
                        allP.Hurt(damage, Fireball.BallDamage._deathReason);
                        scp.CustomHumeShieldStat.AddAmount(damage);


                    }
                }
                yield return Timing.WaitForSeconds(DamageRefreshRate);
            }

        }

        protected override void RoleRemoved(Player player)
        {
            Log.Debug("remove role 457");


            if (_inside.TryGetValue(player, out var l))
            {
                l.Destroy();
                _inside.Remove(player);
            }





            if (_handles.TryGetValue(player, out var handle))
            {
                foreach (var c in handle)
                {
                    Timing.KillCoroutines(c);
                }
                _handles.Remove(player);
            }

        }


        private void OnStalking(StalkingEventArgs ev)
        {
            if (!Check(ev.Player)) return;
            ev.IsAllowed = false;
        }

        private void OnTP(TeleportingEventArgs ev)
        {
            if (!Check(ev.Player)) return;
            ev.IsAllowed = false;

        }


        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Scp106.Stalking += OnStalking;
            Exiled.Events.Handlers.Scp106.Teleporting += OnTP;
            base.SubscribeEvents();

        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Scp106.Stalking -= OnStalking;
            Exiled.Events.Handlers.Scp106.Teleporting -= OnTP;
            base.UnsubscribeEvents();
        }

    }
}
