using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.GlobalEventFramework.GEFE.API.Interfaces
{
    /// <summary>
    /// Override the redactable chance
    /// </summary>
    public interface IChanceRedactable
    {

        /// <summary>
        /// 0-100
        /// </summary>
        public abstract float ChanceRedacted { get; }


    }
}
