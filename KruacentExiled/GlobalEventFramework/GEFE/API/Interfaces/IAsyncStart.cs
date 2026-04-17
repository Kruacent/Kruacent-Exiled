using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.GlobalEventFramework.GEFE.API.Interfaces
{
    public interface IAsyncStart
    {
        /// <summary>
        /// Is launched at the start of a round
        /// </summary>
        IEnumerator<float> Start();
    }
}
