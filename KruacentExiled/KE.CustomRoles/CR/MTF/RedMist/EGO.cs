using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.DamageHandlers;
using KE.Utils.API.Features;
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
    public class EGO : MonoBehaviour
    {


        public bool Active => active;

        private bool active;


        public ReferenceHub Hub { get; private set; }

        private const float Damage = 2;

        private CustomReasonDamageHandler damage;

        internal void Awake()
        {
            Hub = ReferenceHub.GetHub(base.gameObject);
            active = false;

            damage = new CustomReasonDamageHandler("drained", Damage);
            KELog.Debug(Hub);
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
                KELog.Debug(Time.deltaTime);
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


        public void ToggleActive()
        {
            active = !Active;
            KELog.Debug("toggle now active? "+ Active);
            Effect();
        }



        private void Effect()
        {
            Player player = Player.Get(Hub);

            if (Active)
            {
                player.AddLevelEffect<MovementBoost>(25);
                player.AddLevelEffect<Scp1853>(2);
                if (player.IsEffectActive<Poisoned>())
                {
                    player.DisableEffect<Poisoned>();
                }
            }
            else
            {
                player.AddLevelEffect<MovementBoost>(-25);
                player.DisableEffect<Scp1853>();
            }


        }
    }
}
