using Exiled.API.Features;
using Exiled.API.Features.Items;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using EffectType = KruacentExiled.Misc.Features.GamblingCoin.Types.EffectType;

internal class DetonatedFlash : ICoinEffect
{
    public string Name { get; set; } = "DetonatedFlash";
    public string Message { get; set; } = "a gift for your eyes";
    public int Weight { get; set; } = 30;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        FlashGrenade flash = (FlashGrenade)Item.Create(ItemType.GrenadeFlash, player);
        flash.FuseTime = 1f;
        flash.SpawnActive(player.Position);
    }
}