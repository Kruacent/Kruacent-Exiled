using Discord;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Toys;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Patches.Events.Player;
using KE.CustomRoles.API.Features;
using MEC;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.CustomRoles.Abilities.FireAbilities
{
    public class Fireball : FireAbilityBase
    {
        public override string Name { get; } = "Fireball";
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Fireball",
                    [TranslationKeyDesc] = "I cast Fireball",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Boule de feur",
                    [TranslationKeyDesc] = "Attends j'ai fait une faute là non?",
                }
            };
        }
        public override int Cost => 10;

        public override float Cooldown { get; } = 0f;


        public static readonly CustomReasonDamageHandler BallDamage = new CustomReasonDamageHandler("Burned to death", 25, string.Empty);
        public const int MAX_BALLS = 5;
        private static readonly Color ballColor = new Color(2, 1.08f, 0, .75f);
        private Dictionary<Player, int> _activeBalls = new Dictionary<Player, int>();

        protected override bool LaunchedAbility(Player player)
        {
            if (!_activeBalls.ContainsKey(player))
            {
                _activeBalls.Add(player, 0);
            }


            if (_activeBalls[player] >= MAX_BALLS)
            {
                ShowEffectHint(player, "too much balls");
                return true;
            }


            _activeBalls[player]++;
            Timing.RunCoroutine(LaunchingAttack(player));
            return true;

        }


        private float smooth = .01f;

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

            int fallback = Mathf.CeilToInt(100/ smooth);
            Log.Debug("fallback=" + fallback);
            while (!attackTouchedSomething && fallback > 0)
            {
                nextPos = primitive.Position + primitive.Rotation * new Vector3(0, 0, 10 * smooth);


                if (Physics.Linecast(primitive.Position, nextPos, out RaycastHit hit))
                {
                    //spawn mtf looking at central gate
                    if (hit.collider.gameObject.name != "VolumeOverrideTunnel")
                    {
                        attackTouchedSomething = true;
                    }

                }

                Collider[] colliders = Physics.OverlapSphere(primitive.Position, .5f);



                foreach (Collider collider in colliders)
                {

                    attackTouchedSomething = attackTouchedSomething || ProcessHit(player, collider);

                }







                Log.Debug($"current pos = {primitive.Position} next pos = {nextPos}");
                yield return Timing.WaitForSeconds(smooth);
                primitive.Position = nextPos;
                light.Position = nextPos;
                fallback--;
            }
            _activeBalls[player]--;
            primitive.Destroy();
            light.Destroy();
            
            
        }


        private bool ProcessHit(Player attacker,Collider collider)
        {
            bool result = false;
            Player playerhit = Player.Get(collider);
            if (playerhit != null && playerhit.Role.Side != attacker.Role.Side)
            {
                playerhit.Hurt(BallDamage);
                attacker.ShowHitMarker();
                result = true;

            }

            Door doorhit = Door.Get(collider.gameObject);
            if (doorhit != null && doorhit is IDamageableDoor damageable && !damageable.IsDestroyed)
            {
                damageable.Break();
                attacker.ShowHitMarker();
                result = true;
            }

            return result;
        }





    }

}
