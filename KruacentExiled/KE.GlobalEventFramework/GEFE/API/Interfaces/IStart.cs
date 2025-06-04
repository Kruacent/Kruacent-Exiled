using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.GlobalEventFramework.GEFE.API.Interfaces
{
    public interface IStart
    {
        /// <summary>
        /// Is launched at the start of a round
        /// </summary>
        IEnumerator<float> Start();
    }
}
