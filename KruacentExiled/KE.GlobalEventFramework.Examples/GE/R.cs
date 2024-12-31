using KE.GlobalEventFramework.GEFE.API.Features;
using System.Collections.Generic;

namespace KE.GlobalEventFramework.Examples.GE
{
    /// <summary>
    /// Literally does nothing
    /// </summary>
    public class R : GlobalEvent
    {
        ///<inheritdoc/>
        public override int Id { get; set; } = 32;
        ///<inheritdoc/>
        public override string Name { get; set; } = "nothing";
        ///<inheritdoc/>
        public override string Description { get; set; } = "y'a r";
        ///<inheritdoc/>
        public override int Weight { get; set; } = 3;
        ///<inheritdoc/>
        public override IEnumerator<float> Start()
        {
            yield return 0;
        }

    }
}
