using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Features;
using PlayerRoles.FirstPersonControl;
using System;
using System.Collections.Generic;
using UnityEngine.Windows.Speech;
using LabPlayer = LabApi.Features.Wrappers.Player;
namespace KE.CustomRoles.CR.CustomSCPs.SCP049C.UnlockableAbilities.Tier2
{
    internal class DeflectDamageUnlockable : Unlockable
    {
        public override byte Tier => 2;
        public override string GetName(ReferenceHub hub)
        {
            return "Deflect Damages";
        }
        public override string GetDescription(ReferenceHub hub)
        {
            return "1 chance sur 8 d'annuler un dégât (.5s par hp sauvé de cooldown)";
        }


        private static Dictionary<ReferenceHub, LastDamage> cooldowns = new Dictionary<ReferenceHub, LastDamage>();

        public override void Grant(ReferenceHub hub)
        {
            cooldowns.Add(hub, null);
            
        }

        public override void Remove(ReferenceHub hub)
        {

            cooldowns.Remove(hub);
        }


        internal static void OnHurting(HurtingEventArgs ev)
        {

            ReferenceHub hub = ev.Player.ReferenceHub;
            if (ev.IsAllowed && cooldowns.ContainsKey(hub))
            {
                LastDamage lastDamage = cooldowns[hub];

                if(lastDamage is null || lastDamage.IsDeflectable())
                {
                    ev.IsAllowed = false;
                    cooldowns[hub] = new LastDamage(ev.Amount);
                }
            }
        }

        private class LastDamage
        {
            public float DamageAmount;
            public DateTime Time;
            public LastDamage(float damageAmount)
            {
                DamageAmount = damageAmount;
                Time = DateTime.Now;
            }


            public bool IsDeflectable()
            {
                return DateTime.Now > (Time + GetCooldown(DamageAmount));
            }


            public static TimeSpan GetCooldown(float damage)
            {
                return TimeSpan.FromSeconds(damage * .5f);
            }

        }
    }



    
}
