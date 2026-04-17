using Exiled.API.Features;
using Exiled.API.Features.Items;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;

internal class PrimedVase : ICoinEffect
{
    public string Name { get; set; } = "PrimedVase";
    public string Message { get; set; } = "You're grandma come to visit you";
    public int Weight { get; set; } = 20;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        Scp244 vase = (Scp244)Item.Create(ItemType.SCP244a);
        vase.Primed = true;
        vase.CreatePickup(player.Position);
    }
}