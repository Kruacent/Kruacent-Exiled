using Exiled.API.Features;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;
using System.Linq;

internal class YouLoveTesla : ICoinEffect
{
    public string Name { get; set; } = "YouLoveTesla";
    public string Message { get; set; } = "You love Tesla much more than Elon Musk";
    public int Weight { get; set; } = 15;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        player.DropHeldItem();

        player.Teleport(Exiled.API.Features.TeslaGate.List.ToList().RandomItem());

        if (Warhead.IsDetonated)
        {
            player.Kill(Exiled.API.Enums.DamageType.Decontamination);
        }
    }
}