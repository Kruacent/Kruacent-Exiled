using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEC;

namespace KE.GlobalEventFramework.GEFE.API.Interfaces
{
    public interface IGlobalEvent
    {
        /// <summary>
        /// the UNIQUE id of the Global Event
        /// </summary>
        uint Id { get; set; }
        /// <summary>
        /// Name used in the logs on the RA
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The description that will be shown to the player when the round start
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// The chance this GE will be choosed at the start of a round
        /// </summary>
        int Weight { get; set; }
        /// <summary>
        /// The ids of incompatible Globals Events 
        /// Note: You can't have the same GE twice in the same round
        /// </summary>
        uint[] IncompatibleGE { get; set; }

        /// <summary>
        /// Is launched at the start of a round
        /// </summary>
        IEnumerator<float> Start();
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
