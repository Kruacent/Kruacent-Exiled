using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.DamageHandlers;
using Exiled.Events.EventArgs.Player;
using KE.CustomRoles.API.Features.Abilities;
using KE.Utils.API.Features;
using KE.Utils.API.Interfaces;
using KE.Utils.Extensions;
using PlayerRoles;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.CR.MTF.RedMist
{
    public class EGO : BaseCompAbility, IUsingEvents
    {


        public override bool Active
        {
            get
            {
                return active;
            }
        }

        private bool active;


        public ReferenceHub Hub { get; private set; }

        private const float Damage = 2;

        private CustomReasonDamageHandler damage;

        internal void Awake()
        {
            Hub = ReferenceHub.GetHub(base.gameObject);
            active = false;

            damage = new CustomReasonDamageHandler("drained", Damage);

            SubscribeEvents();
        }


        internal void OnDestroy()
        {
            UnsubscribeEvents();
        }

        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ReceivingEffect += OnReceivingEffect;
            Exiled.Events.Handlers.Player.Hurt += OnHurt;
        }
        private void OnHurt(HurtEventArgs ev)
        {
            Player player = Player.Get(Hub);
            if (player != ev.Attacker) return;

            if (Active)
            {
                IncreaseObjective();
            }
        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ReceivingEffect -= OnReceivingEffect;

        }

        private void OnReceivingEffect(ReceivingEffectEventArgs ev)
        {
            Player player = Player.Get(Hub);
            if (ev.Player != player) return;
            StatusEffectBase effect = ev.Effect;

            if (Active && effect is Poisoned)
            {
                effect.DisableEffect();
            }
        }


        private float cooldown = 0;
        private float objective = .2f;
        private float baseObjective = .2f;

        private void UpdateDamage()
        {
            if (Hub is null)
            {
                KELog.Debug("null");
                return;
            }

            if (!Hub.IsAlive())
            {
                KELog.Debug(Hub?.nicknameSync.DisplayName + " is dead");
                Destroy(this);
                return;
            }

            if (Active)
            {
                cooldown += Time.deltaTime;
                KELog.Debug(cooldown);
                if (cooldown >= objective)
                {
                    Hub.playerStats.DealDamage(damage);
                    cooldown = 0;
                    objective = baseObjective;
                }
            }


        }

        public void IncreaseObjective()
        {
            objective += baseObjective*10;
        }


        public void Update()
        {
            UpdateDamage();
        }


        public override void ToggleActive()
        {
            active = !Active;
            KELog.Debug("toggle now active? "+ Active);
            Effect();
        }


        private static readonly Dictionary<EffectType,byte> effects = new()
        {
            { EffectType.MovementBoost,25 },
            { EffectType.Scp1853,2 },
        };

        public const byte MovementBoostIntensity = 25;
        public const byte SCP1853Intensity = 2;
        private void Effect()
        {
            Player player = Player.Get(Hub);

            if (Active)
            {
                foreach(var kvp in effects)
                {
                    player.AddLevelEffect(kvp.Key, kvp.Value);
                }
                if (player.IsEffectActive<Poisoned>())
                {
                    player.DisableEffect<Poisoned>();
                }
            }
            else
            {
                foreach (var kvp in effects)
                {
                    player.AddLevelEffect(kvp.Key, -kvp.Value);
                }
            }


        }
    }
}
