using Exiled.API.Features;
using KE.Misc.Features.GamblingCoin.Interfaces;
using KE.Misc.Features.GamblingCoin.Types;

internal class InventoryReset : ICoinEffect
{
    public string Name { get; set; } = "InventoryReset";
    public string Message { get; set; } = "have you ever got item in your inventory ?";
    public int Weight { get; set; } = 20;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        player.DropHeldItem();
        player.ClearInventory();
    }
}