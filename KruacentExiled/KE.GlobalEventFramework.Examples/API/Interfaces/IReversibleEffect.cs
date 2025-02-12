using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.GlobalEventFramework.Examples.API.Interfaces
{
    internal interface IReversibleEffect
    {
        sbyte MalfunctionDeactivation { get; }
        void EffectDeactivate();
    }
}
