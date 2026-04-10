using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Toys;
using Exiled.API.Interfaces;
using Exiled.Events.Commands.Reload;
using Exiled.Events.EventArgs.Scp106;
using KE.CustomRoles.Abilities.FireAbilities;
using KE.CustomRoles.API.Features;
using KE.Utils.API;
using MEC;
using PlayerRoles;
using PlayerStatsSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.CustomRoles.CR.CustomSCPs
{
    public class SCP457 : CustomSCP
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "SCP-457",
                    [TranslationKeyDesc] = "You do passive damage around you",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "SCP-457",
                    [TranslationKeyDesc] = "j'ai dit pas trop cuite",
                },
                ["legacy"] = new()
                {
                    [TranslationKeyName] = "SCP-457",
                    [TranslationKeyDesc] = "You do passive damage around you",
                }
            };
        }
        public override int MaxHealth { get; set; } = 5000;
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scp106;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override bool IsSupport { get; } = false;

        public override float SpawnChance { get; set; } = 0;

        private Dictionary<Player, Light> _inside = new();
        private Dictionary<Player, HashSet<CoroutineHandle>> _handles = new();
        

        public static float DamageRefreshRate = 5f;
        public static readonly Color FlameColor = new(2, 1.08f, 0);

        public Collider[] SphereNonAlloc = new Collider[32];


        public override HashSet<string> Abilities => 
        [
            "Fireball",
            "BlindingFlash"
        ];

        protected override int SettingId => 10001;

        protected override void RoleAdded(Player player)
        {
            Log.Debug("adding role 457");
            _inside.Add(player, null);
            _handles.Add(player, new());

            _handles[player].Add(Timing.RunCoroutine(InsideLight(player)));
            _handles[player].Add(Timing.RunCoroutine(PassiveDamage(player)));

            if(player.Role is Scp106Role role)
            {
                role.Vigor = role.VigorComponent.MaxValue;
            }
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


                int num = Physics.OverlapSphereNonAlloc(scp.Position, 5, SphereNonAlloc);

                if (num > 0)
                {
                    for (int i = 0; i < num; i++)
                    {
                        Player player = Player.Get(SphereNonAlloc[i]);
                        if (player is null) continue;
                        if (!HitboxIdentity.IsDamageable(scp.ReferenceHub, player.ReferenceHub)) continue;

                        if(Physics.Linecast(player.Position, scp.Position, out var hitinfo))
                        {
                            float damage = -(hitinfo.distance / 3) + 10;
                            player.EnableEffect(Exiled.API.Enums.EffectType.Burned, DamageRefreshRate, true);
                            player.Hurt(damage, Fireball.BallDamage.RagdollInspectText);
                            scp.CustomHumeShieldStat.AddAmount(damage);
                        }
                        
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

        public override bool IsAvailable(Player player)
        {
            return false;
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
            Exiled.Events.Handlers.Scp106.Attacking += OnAttacking;
            base.SubscribeEvents();

        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Scp106.Stalking -= OnStalking;
            Exiled.Events.Handlers.Scp106.Teleporting -= OnTP;
            Exiled.Events.Handlers.Scp106.Attacking -= OnAttacking;
            base.UnsubscribeEvents();
        }


        private void OnAttacking(AttackingEventArgs ev)
        {
            if (!Check(ev.Player)) return;

            ev.IsAllowed = false;
        }
            
            
            
        }
}
