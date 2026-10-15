using KruacentExiled.GlobalEventFramework.GEFE.API.Enums;
using KruacentExiled.GlobalEventFramework.GEFE.API.Features;
using KruacentExiled.GlobalEventFramework.GEFE.API.Interfaces;
namespace KruacentExiled.GlobalEventFramework.Examples.GE
{
    /// <summary>
    /// Literally does nothing
    /// </summary>
    public class R : GlobalEvent,IOnlyEvent
    {
        ///<inheritdoc/>
        public override string Name { get; set; } = "nothing";
        ///<inheritdoc/>
        public override string Description { get; } = "y'a r";
        ///<inheritdoc/>
        public override int WeightedChance => 10;
        public override ImpactLevel ImpactLevel => ImpactLevel.VeryLow;


    }
}
