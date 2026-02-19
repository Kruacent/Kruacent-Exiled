using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Doors;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.Abilities
{
    public class ForceOpen : KEAbilities, ICustomIcon
    {
        public override string Name { get;  } = "ForceOpen";
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Force open",
                    [TranslationKeyDesc] = "Force open a door",
                    ["ForceOpenFail"] = "You failed opening the door and lost %HP% HP!",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Forcer une porte",
                    [TranslationKeyDesc] = "Force une porte (grosse description)",
                    ["ForceOpenFail"] = "Tu as raté d'ouvrir la porte et a perdu %HP% HP!",
                }
            };
        }
        public Utils.API.GifAnimator.TextImage IconName => MainPlugin.Instance.icons["ForceOpen"];
        public override float Cooldown { get;  } = 30;
        private Dictionary<Player, DateTime> abilityActivated = new();
        public static readonly TimeSpan MaxTime = new (0, 0, 30);

        protected override bool AbilityUsed(Player player)
        {
            if (abilityActivated.ContainsKey(player))
            {
                abilityActivated[player] = DateTime.Now;
            }
            else
            {
                abilityActivated.Add(player, DateTime.Now);
            }

            return base.AbilityUsed(player);
            
            
        }

        private void InteractingDoor(InteractingDoorEventArgs ev)
        {
            Player player = ev.Player; 
            if (ev.IsAllowed) return;
            if (!abilityActivated.ContainsKey(player)) return;
            if (DateTime.Now > abilityActivated[player] + MaxTime) return;

            if (ev.Door.IsOpen) return;
            if (ev.Door.DoorLockType >= DoorLockType.Lockdown079) return;


            int successRate;
            int damage;

            if (ev.Door is Gate)
            {
                successRate = 50;
                damage = 20;
            }
            else if (ev.Door.Type.IsCheckpoint())
            {
                successRate = 50;
                damage = 10;
            }
            else
            {
                successRate = 75;
                damage = 5;
            }


            int proba = UnityEngine.Random.Range(0, 101);

            if (proba <= successRate)
            {
                if(ev.Door is Gate gate)
                {
                    gate.TryPry(player);
                }
                else
                {
                    ev.IsAllowed = true;
                }
            }
            else
            {

                string text = GetTranslation(player, "ForceOpenFail").Replace("%HP%",damage.ToString());
                ShowEffectHint(player, text,0f);
                player.Hurt(damage, "Door too stronk");
            }


            abilityActivated.Remove(player);


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
