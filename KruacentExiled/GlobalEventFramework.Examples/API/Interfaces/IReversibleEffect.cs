using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.GlobalEventFramework.Examples.API.Interfaces
{
    internal interface IReversibleEffect
    {
        string VoiceLineDeactivate { get; }
        string VoiceLineDeactivateTranslated { get; }
        sbyte MalfunctionDeactivation { get; }
        void DeactivateEffect();
    }
}
