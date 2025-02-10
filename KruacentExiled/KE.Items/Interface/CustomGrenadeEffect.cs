using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Items.Interface
{
    public abstract class CustomGrenadeEffect : CustomItemEffect
    {
        //
        // Summary:
        //     Handles tracking thrown requests by custom grenades.
        //
        // Parameters:
        //   ev:
        //     Exiled.Events.EventArgs.Player.ThrowingRequestEventArgs.
        public virtual void OnThrowingRequest(ThrowingRequestEventArgs ev)
        {
        }

        //
        // Summary:
        //     Handles tracking thrown custom grenades.
        //
        // Parameters:
        //   ev:
        //     Exiled.Events.EventArgs.Player.ThrownProjectileEventArgs.
        public virtual void OnThrownProjectile(ThrownProjectileEventArgs ev)
        {
        }

        //
        // Summary:
        //     Handles tracking exploded custom grenades.
        //
        // Parameters:
        //   ev:
        //     Exiled.Events.EventArgs.Map.ExplodingGrenadeEventArgs.
        public virtual void OnExploding(ExplodingGrenadeEventArgs ev)
        {
        }

        //
        // Summary:
        //     Handles the tracking of custom grenade pickups that are changed into live grenades
        //     by a frag grenade explosion.
        //
        // Parameters:
        //   ev:
        //     Exiled.Events.EventArgs.Map.ChangedIntoGrenadeEventArgs.
        public virtual void OnChangedIntoGrenade(ChangedIntoGrenadeEventArgs ev)
        {
        }
    }
}
