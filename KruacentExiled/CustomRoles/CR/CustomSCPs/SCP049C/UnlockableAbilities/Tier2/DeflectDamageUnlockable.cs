using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using KruacentExiled.CustomRoles.API.Features;
using System.Collections.Generic;
namespace KruacentExiled.CustomRoles.CR.CustomSCPs.SCP049C.UnlockableAbilities.Tier2
{
    internal class DeflectDamageUnlockable : UnlockableAbility
    {
        public override byte Tier => 2;

        public override KEAbilities Ability => KEAbilities.Get("Parry");

        public override string GetName(ReferenceHub hub)
        {
            return "Deflect Damages";
        }
        public override string GetDescription(ReferenceHub hub)
        {
            return "1 chance sur 8 d'annuler un dégât (.5s par hp sauvé de cooldown)";
        }


        private static HashSet<ReferenceHub> _players = new HashSet<ReferenceHub>();

        public override void Grant(ReferenceHub hub)
        {
            _players.Add(hub);
        }

        public override void Remove(ReferenceHub hub)
        {
            _players.Remove(hub);
        }
        internal static void OnHurting(HurtingEventArgs ev)
        {
            
            ReferenceHub hub = ev.Player.ReferenceHub;
            if (ev.IsAllowed && _players.Contains(hub))
            {
                Log.Info("hurted");
                ev.IsAllowed = false;


            }
        }



    }



    
}
