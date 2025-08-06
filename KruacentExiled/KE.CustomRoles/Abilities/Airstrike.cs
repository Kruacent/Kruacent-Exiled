using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.API.Features.Toys;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.API.Features;
using MapGeneration;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.CustomRoles.Abilities
{
    public class Airstrike : KEAbilities
    {
        public override string Name { get;  } = "Airstrike";

        public override string Description { get;  } = "Don't overuse it or your co-op will not be happy";

        public override int Id => 2001;
        public override float Cooldown { get; } = 0f;

        public float height = 5;

        

        protected override void AbilityUsed(Player player)
        {
            if(!SelectPosition.TryGetTarget(player, out Vector3 target))
            {
                //show hint
                Log.Info("no target selected");
                return;
            }
            if(target.GetZone() != FacilityZone.Surface)
            {
                Log.Info("set target surface");
                return;
            }

            Physics.Linecast(target, target + height * Vector3.up, out RaycastHit hit);
            if (hit.collider != null)
            {
                Log.Info($"hit something [{hit.collider}]");
                return;
            }
            
            var l = Light.Create(target,null,null,true,Color.red);

            ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE,player);
            grenade.ScpDamageMultiplier = 1;
            grenade.FuseTime = 10;
            Timing.CallDelayed(5, () =>
            {
                Projectile gre = grenade.SpawnActive(target + (height - .5f) * Vector3.up);

                //explode on collision
                gre.GameObject.AddComponent<Exiled.API.Features.Components.CollisionHandler>().Init(player.GameObject, gre.Base);
                l.Destroy();
            });




        }


    }
}
