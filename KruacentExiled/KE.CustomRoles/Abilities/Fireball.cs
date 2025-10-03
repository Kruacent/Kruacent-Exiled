using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Toys;
using Exiled.API.Interfaces;
using KE.CustomRoles.API.Features;
using MEC;
using PlayerStatsSystem;
using System.Collections.Generic;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.CustomRoles.Abilities
{
    public class Fireball : KEAbilities
    {
        public override string Name { get; } = "Fireball";

        public override string Description { get; } = "I cast Fireball";

        public override int Id => 2010;

        public override float Cooldown { get; } = 2f;


        public const float VIGOR_COST = .1f;
        public static readonly CustomReasonDamageHandler BallDamage = new("Burned to death", 25, string.Empty);
        public const int MAX_BALLS = 3;
        private static readonly Color ballColor = new(2, 1.08f, 0, .75f);
        private Dictionary<Player, int> _activeBalls = new();

        protected override void AbilityUsed(Player player)
        {
            if (!_activeBalls.ContainsKey(player))
            {
                _activeBalls.Add(player, 0);
            }


            if (_activeBalls[player] >= MAX_BALLS) return;



            if (player.Role is Scp106Role role106)
            {
                if (role106.Vigor <= VIGOR_COST) return;
                role106.Vigor -= VIGOR_COST;
            }

            _activeBalls[player]++;
            Timing.RunCoroutine(LaunchingAttack(player));

        }
        private IEnumerator<float> LaunchingAttack(Player player)
        {
            Vector3 initpos = player.Position;
            Quaternion direction = player.ReferenceHub.PlayerCameraReference.rotation;


            Log.Debug(direction.eulerAngles);
            bool attackTouchedSomething = false;

            Light light = Light.Create(initpos, direction.eulerAngles, null, false);
            light.Color = ballColor;
            light.Intensity = 1f;
            Primitive primitive = Primitive.Create(initpos, direction.eulerAngles, null, false);
            primitive.Collidable = false;
            primitive.Color = ballColor;
            primitive.Spawn();
            light.Spawn();
            Vector3 nextPos;

            int fallback = 100;
            while (!attackTouchedSomething && fallback > 0)
            {
                nextPos = primitive.Position + primitive.Rotation * new Vector3(0, 0, 1f);
                RaycastHit hit;

                if (Physics.Linecast(primitive.Position, nextPos, out hit))
                {
                    //spawn mtf looking at central gate
                    if (hit.collider.gameObject.name != "VolumeOverrideTunnel")
                    {
                        attackTouchedSomething = true;
                    }

                    Player playerhit = Player.Get(hit.collider);
                    if (playerhit != null && playerhit.Role.Side != player.Role.Side)
                    {
                        playerhit.Hurt(BallDamage);
                        player.ShowHitMarker();

                    }

                    Door doorhit = Door.Get(hit.collider.gameObject);
                    if (doorhit != null && doorhit is IDamageableDoor damageable && !damageable.IsDestroyed)
                    {
                        damageable.Break();
                        player.ShowHitMarker();
                    }
                }




                Log.Debug($"current pos = {primitive.Position} next pos = {nextPos}");
                yield return Timing.WaitForSeconds(.1f);
                primitive.Position = nextPos;
                light.Position = nextPos;
                fallback--;
            }
            _activeBalls[player]--;
            primitive.Destroy();
            light.Destroy();
        }





    }

}
