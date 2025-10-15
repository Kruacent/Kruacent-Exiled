using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.Abilities
{
    public class ForceOpen : KEAbilities
    {
        public override string Name { get;  } = "ForceOpen";
        public override string PublicName { get; } = "Force open";

        public override string Description { get; } = "Force open a door";

        public override int Id => 2009;

        public override float Cooldown { get;  } = 30;
        private Dictionary<Player, DateTime> abilityActivated = new();
        public static readonly TimeSpan MaxTime = new (0, 0, 30);

        protected override void AbilityUsed(Player player)
        {
            if (abilityActivated.ContainsKey(player))
            {
                abilityActivated[player] = DateTime.Now;
            }
            else
            {
                abilityActivated.Add(player, DateTime.Now);
            }

            
            
        }

        private void InteractingDoor(InteractingDoorEventArgs ev)
        {
            Player player = ev.Player; 
            if (!abilityActivated.ContainsKey(player)) return;
            if (DateTime.Now > abilityActivated[player] + MaxTime) return;
            if (ev.IsAllowed) return;
            if (ev.Door.DoorLockType > DoorLockType.Lockdown079) return;


            int successRate;
            int damage;

            if (ev.Door.Type.IsGate())
            {
                successRate = 20;
                damage = 20;
            }
            else if (ev.Door.Type.IsCheckpoint())
            {
                successRate = 30;
                damage = 10;
            }
            else
            {
                successRate = 40;
                damage = 5;
            }

            int proba = UnityEngine.Random.Range(0, 101);

            if (proba <= successRate)
            {
                ev.IsAllowed = true;
            }
            else
            {
                MainPlugin.ShowEffectHint(player, $"T'as échoué d'ouvrir la porte et t'as perdu {damage} HP !");
                player.Hurt(damage, "Door too stronk");
            }





        }


        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.InteractingDoor += InteractingDoor;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.InteractingDoor -= InteractingDoor;
            base.UnsubscribeEvents();
        }

    }
}
