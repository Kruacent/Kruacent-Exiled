using GlobalEventFramework.API.Enums;
using GlobalEventFramework.API.Features;
using GlobalEventFramework.API.Interfaces;
namespace KruacentExiled.GlobalEvents.GE
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

        public override string[] AltDescription => new string[]
        {
            "faut tuer le SCP",
        };
    }
}
