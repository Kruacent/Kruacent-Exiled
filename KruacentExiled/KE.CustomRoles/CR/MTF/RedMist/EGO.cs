using Exiled.API.Features;
using Exiled.API.Features.DamageHandlers;
using KE.Utils.API.Features;
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


        public bool Active { get; set; }

        


        private ReferenceHub Hub;

        private float Damage = 1;

        private DamageHandler damageHandler;

        private void Awake()
        {
            Active = false;
            Hub = ReferenceHub.GetHub(base.gameObject);
            damageHandler = new CustomDamageHandler(Player.Get(Hub), null, Damage);
        }

        private void Update()
        {
            //if (Hub is null || !Hub.IsAlive())
            //{
            //    Log.Debug(Hub?.nicknameSync.DisplayName +" is dead");
            //    Destroy(this);
            //    return;
            //}

            if (Active)
            {
                Hub.playerStats.DealDamage(damageHandler);
                KELog.Debug("damage");
            }
  
        }


        public void ToggleActive()
        {
            Active = !Active;
            KELog.Debug("toggle now active? "+ Active);
        }
    }
}
