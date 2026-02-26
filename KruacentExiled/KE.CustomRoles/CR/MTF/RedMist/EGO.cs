using Exiled.API.Features;
using Exiled.API.Features.DamageHandlers;
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


        public void Awake()
        {
            Active = false;
            Hub = ReferenceHub.GetHub(base.gameObject);
        }

        public void Update()
        {
            if (Hub is null || !Hub.IsAlive())
            {
                Log.Debug(Hub?.nicknameSync.DisplayName +" is dead");
                Destroy(this);
                return;
            }

            if (Active)
            {
                Hub.playerStats.DealDamage(new CustomDamageHandler(Player.Get(Hub), null, Damage));
            }
  
        }


        public void ToggleActive()
        {
            Active = !Active;
        }
    }
}
