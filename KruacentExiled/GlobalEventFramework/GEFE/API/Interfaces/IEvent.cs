using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.GlobalEventFramework.GEFE.API.Interfaces
{
    public interface IEvent
    {
        /// <summary>
        /// The method used to subcribe to event like with normal plugins
        /// </summary>
        void SubscribeEvent();
        /// <summary>
        /// The method used to unsubcribe to event like with normal plugins
        /// </summary>
        void UnsubscribeEvent();
    }
}
