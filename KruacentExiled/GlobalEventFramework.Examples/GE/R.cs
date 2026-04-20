using KruacentExiled.GlobalEventFramework.GEFE.API.Enums;
using KruacentExiled.GlobalEventFramework.GEFE.API.Features;
namespace KruacentExiled.GlobalEventFramework.Examples.GE
{
    /// <summary>
    /// Literally does nothing
    /// </summary>
    public class R : GlobalEvent
    {
        ///<inheritdoc/>
        public override uint Id { get; set; } = 0;
        ///<inheritdoc/>
        public override string Name { get; set; } = "nothing";
        ///<inheritdoc/>
        public override string Description { get; } = "y'a r";
        ///<inheritdoc/>
        public override int WeightedChance => 10;
        public override ImpactLevel ImpactLevel => ImpactLevel.VeryLow;


    }
}
