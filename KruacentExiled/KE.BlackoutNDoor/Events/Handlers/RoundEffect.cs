using Exiled.Events.Features;
using KE.BlackoutNDoor.Events.EventArgs.RoundEffect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.BlackoutNDoor.Events.Handlers
{
    public static class RoundEffect
    {
        /// <summary>
        /// Invoked before a <see cref="API.Features.RoundEffects.RoundEffect"/> occurr
        /// </summary>
        public static Event<PreRoundEffectEventArgs> PreRoundEffect = new();
        /// <summary>
        /// Invoked after a <see cref="API.Features.RoundEffects.RoundEffect"/>  occurr
        /// </summary>
        public static Event<PostRoundEffectEventArgs> PostRoundEffect = new();


        /// <summary>
        /// Called before a <see cref="API.Features.RoundEffects.RoundEffect"/>  occurr
        /// </summary>
        /// <param name="ev"></param>
        public static void OnPreRoundEffect(PreRoundEffectEventArgs ev)
        {
            PreRoundEffect.InvokeSafely(ev);
        }
        /// <summary>
        /// Called after a <see cref="API.Features.RoundEffects.RoundEffect"/>  occurr
        /// </summary>
        /// <param name="ev"></param>
        public static void OnPostRoundEffect(PostRoundEffectEventArgs ev)
        {
            PostRoundEffect.InvokeSafely(ev);
        }
    }
}
