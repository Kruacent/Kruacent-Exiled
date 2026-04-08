using Exiled.API.Features;
using KE.GlobalEventFramework.Examples.API.Feature;
using KE.GlobalEventFramework.Examples.API.Interfaces;
using KE.GlobalEventFramework.Examples.GE;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.GlobalEventFramework.Examples.API.MalfunctionEffects
{
    internal class AutoNuke : MalfunctionEffect
    {
        public override string Name { get; } = "Automatic Warhead";
        public override string VoiceLine { get; } = "Malfunctions levels above . 90 percent . . starting emergency warhead";
        public override string VoiceLineTranslated { get; } = "Malfunctions levels above 90%, starting emergency warhead";
        public override sbyte MalfunctionActivation { get; } = 90;


        public sbyte MalfunctionDeactivation { get; } = 85;
        private CoroutineHandle _checkNuke;

        public override void ActivateEffect()
        {
            if (!Warhead.IsInProgress)
            {
                Warhead.Start();
                Warhead.IsLocked = true;
                Timing.KillCoroutines(_checkNuke);
                _checkNuke = Timing.RunCoroutine(CheckNuke());
            }
        }

        public IEnumerator<float> CheckNuke()
        {
            while (Warhead.IsInProgress)
            {
                yield return Timing.WaitForSeconds(5);
                var malfunction = SystemMalfunction.Malfunction.Malfunction;
                if (malfunction <= 85)
                {
                    Log.Debug($"Malfunction low enough ({malfunction}) disabling the nuke");
                    Warhead.IsLocked = false;
                    Warhead.Stop();
                    break;
                }
            }
        }
    }
}
