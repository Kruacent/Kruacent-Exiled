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

namespace KE.CustomRoles.Abilities.RedMist
{
    public class EGO : MonoBehaviour
    {


        private bool active = false;




        private ReferenceHub Hub;

        private float Damage = 1;


        public void Init()
        {
            active = false;
            Hub = ReferenceHub.GetHub(base.transform.root.gameObject);
        }


        public void Update()
        {
            if (Hub is null || !Hub.IsAlive())
            {
                Log.Debug(Hub?.nicknameSync.DisplayName +" is dead");
                Destroy(gameObject);
            }

            if (active)
            {
                Hub.playerStats.DealDamage(new CustomDamageHandler(Player.Get(Hub), null, Damage));
            }
  
        }


        public void SetActive(bool active)
        {
            this.active = active;
        }
        public bool IsActive()
        {
            return active;
        }

        public void ToggleActive()
        {
            active = !active;
        }
    }
}
