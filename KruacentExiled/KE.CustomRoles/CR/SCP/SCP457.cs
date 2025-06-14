using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Toys;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Scp106;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using KE.Utils.API;
using MEC;
using PlayerRoles;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.CustomRoles.CR.SCP
{
    [CustomRole(RoleTypeId.Scp106)]
    public class SCP457 : KECustomRole, ISCPPreferences
    {

        public override string Name { get; set; } = "SCP-457";
        public override string Description { get; set; } = "";
        public override uint Id { get; set; } = 1084;
        public override string CustomInfo { get; set; } = "SCP-457";
        public override int MaxHealth { get; set; } = 3900;
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scp106;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public bool IsSupport { get; } = false;

        public override float SpawnChance { get; set; } = 100;

        private Dictionary<Player,Light> _inside = new();
        private Dictionary<Player, HashSet<CoroutineHandle>> _handles = new();

        public static float LightRefreshRate = .1f;
        public static float DamageRefreshRate = 5f;
        public static readonly Color FlameColor = new(2, 1.08f, 0);
        private static readonly Color ballColor = new(2, 1.08f, 0, .25f);
        public static readonly float VigorCost = .1f;

        private static readonly string _deathMessage = "Burned to death";
        public static CustomReasonDamageHandler BallDamage = new (_deathMessage, 25, string.Empty);

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
            _inside[player] = light;
            light.Position = player.Position;
            light.Color = FlameColor;
            light.Spawn();
            while (true)
            {
                light.Position = player.Position;
                PassiveDamage(player);
                yield return Timing.WaitForSeconds(LightRefreshRate);
            }
        }



        private IEnumerator<float> PassiveDamage(Player scp)
        {
            while (true)
            {
                foreach (Player allP in Player.List.Where(p => p != scp && !p.IsScp))
                {


                    if (OtherUtils.IsInCircle(allP.Position, scp.Position, 5))
                    {

                        Physics.Linecast(allP.Position, scp.Position, out var hitinfo);
                        float damage = -(hitinfo.distance/3)+10;
                        Log.Debug($"damâge={damage} dist = {hitinfo.distance}");
                        allP.EnableEffect(Exiled.API.Enums.EffectType.Burned, DamageRefreshRate, true);
                        allP.Hurt(damage, _deathMessage);


                    }
                }
                yield return Timing.WaitForSeconds(DamageRefreshRate);
            }
            
        }

        protected override void RoleRemoved(Player player)
        {
            Log.Debug("remove role 457");


            if(_inside.TryGetValue(player,out var l))
            {
                l.Destroy();
                _inside.Remove(player);
            }


            

            if(_handles.TryGetValue(player,out var handle))
            {
                foreach(var c in handle)
                {
                    Timing.KillCoroutines(c);
                }
                _handles.Remove(player);
            }
            
        }

        private void Attack(Player player, Scp106Role role)
        {

            if (role.Vigor > VigorCost)
            {
                Timing.RunCoroutine(LaunchingAttack(player));
                role.Vigor -= VigorCost;
            }
                
        }

        private IEnumerator<float> LaunchingAttack(Player player)
        {
            Vector3 initpos = player.Position;
            Quaternion direction = player.ReferenceHub.PlayerCameraReference.rotation;


            Log.Debug(direction.eulerAngles);
            bool attackTouchedSomething =false;


            Primitive primitive = Primitive.Create(initpos, direction.eulerAngles, null, false);
            primitive.Collidable = false;
            primitive.Color = ballColor;
            primitive.Spawn();
            Vector3 nextPos;

            int fallback = 100;
            while (!attackTouchedSomething && fallback > 0)
            {
                nextPos = primitive.Position + primitive.Rotation * new Vector3(0, 0, 1f);
                RaycastHit hit;

                if (UnityEngine.Physics.Linecast(primitive.Position, nextPos, out hit))
                {
                    attackTouchedSomething = true;
                    Player playerhit = Player.Get(hit.collider);
                    if(playerhit != null && playerhit.Role.Side != Exiled.API.Enums.Side.Scp)
                    {
                        playerhit.Hurt(BallDamage);
                        player.ShowHitMarker();
                        
                    }

                    Door doorhit = Door.Get(hit.collider.gameObject);
                    if(doorhit != null && doorhit is IDamageableDoor damageable && !damageable.IsDestroyed)
                    {
                        damageable.Break();
                        player.ShowHitMarker();
                    }
                }
                    
                


                Log.Debug($"current pos = {primitive.Position} next pos = {nextPos}");
                yield return Timing.WaitForSeconds(.1f);
                primitive.Position = nextPos;
                fallback--;
            }
            primitive.Destroy();
        }


        private void OnStalking(StalkingEventArgs ev)
        {
            if (!Check(ev.Player)) return;
            ev.IsAllowed = false;
            
            Attack(ev.Player,ev.Scp106);
        }

        private void OnTP(TeleportingEventArgs ev)
        {
            if (!Check(ev.Player)) return;
            ev.IsAllowed = false;

        }

        private void OnAttacking(AttackingEventArgs ev)
        {
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

    }
}
