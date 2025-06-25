using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp173;
using InventorySystem.Items.Usables;
using KE.GlobalEventFramework.GEFE.API.Features;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.GlobalEventFramework.Examples.MiddleEvents
{
    public class HealSCP : MiddleEvent, IStart, IConditional
    {
        ///<inheritdoc/>
        public override uint Id { get; set; } = 10043;
        ///<inheritdoc/>
        public override string Name { get; set; } = "HealSCPM";
        ///<inheritdoc/>
        public override string Description { get; set; } = "Bon aller on rallonge un peu la game";
        ///<inheritdoc/>
        public override int WeightedChance { get; set; } = 1;

        public bool Condition()
        {
            List<Player> list = Player.List.Where(p => p.IsScp && p.Role != RoleTypeId.Scp0492).ToList();
            if (list.Count ==0) return false;

            foreach(Player p in list)
            {
                if(p.Health/p.MaxHealth > .1f)
                {
                    return false;
                }
            }
            return true;
        }

        public IEnumerator<float> Start()
        {
            foreach (Player p in Player.List.Where(p => p.IsScp && p.Role != RoleTypeId.Scp0492))
            {
                p.Health = p.MaxHealth / 2;
            }

            yield return Timing.WaitForOneFrame;
        }


    }
}
