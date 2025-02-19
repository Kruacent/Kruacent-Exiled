using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.GlobalEventFramework.Examples.API.Feature
{
    public abstract class MalfunctionEffect
    {
        public abstract string Name { get; }
        public abstract string VoiceLine { get; }
        public abstract string VoiceLineTranslated { get; }
        public abstract sbyte MalfunctionActivation { get; }

        public abstract void ActivateEffect();

    }
}
