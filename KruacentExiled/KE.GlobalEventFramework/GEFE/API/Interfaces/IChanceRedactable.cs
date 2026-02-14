using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.GlobalEventFramework.GEFE.API.Interfaces
{
    public interface IChanceRedactable
    {

        /// <summary>
        /// 0-100
        /// </summary>
        public abstract float ChanceRedacted { get; }


    }
}
