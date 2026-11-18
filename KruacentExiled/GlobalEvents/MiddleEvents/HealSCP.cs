using Exiled.API.Features;
using GlobalEventFramework.API.Features;
using GlobalEventFramework.API.Interfaces;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;

namespace KruacentExiled.GlobalEvents.MiddleEvents
{
    public class HealSCP : MiddleEvent, IStart, IConditional
    {
        ///<inheritdoc/>
        public override string Name { get; set; } = "HealSCPM";
        ///<inheritdoc/>
        public override string Description { get; set; } = "Bon aller on rallonge un peu la game";
        ///<inheritdoc/>
        public override int DefaultWeightedChance { get; } = 1;

        public bool Condition()
        {
            List<Player> list = Player.List.Where(p => p.IsScp && p.Role != RoleTypeId.Scp0492).ToList();
            if (list.Count ==0) return false;

            foreach(Player p in list)
            {
                if(p.Health/p.MaxHealth > .3f)
                {
                    return false;
                }
            }
            return true;
        }

        public void Start()
        {
            foreach (Player p in Player.List.Where(p => p.IsScp && p.Role != RoleTypeId.Scp0492))
            {
                p.Health = p.MaxHealth / 2;
            }

        }


    }
}
