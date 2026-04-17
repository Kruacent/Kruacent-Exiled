using InventorySystem.Items.ThrowableProjectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.CustomItems.API.Events
{
    public class OnExplodeDestructibleEventsArgs
    {
        
        public IDestructible Destructible { get; set; }
        public float Damage { get; set; }
        public ExplosionGrenade ExplosionGrenade { get;}
        public OnExplodeDestructibleEventsArgs(IDestructible destructible,float damage, ExplosionGrenade explosionGrenade)
        {
            Destructible = destructible;
            Damage = damage;
            ExplosionGrenade = explosionGrenade;
        }


    }
}
