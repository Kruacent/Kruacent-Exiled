using Exiled.API.Features;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using EffectType = KruacentExiled.Misc.Features.GamblingCoin.Types.EffectType;

public class Handcuff : ICoinEffect
{
    public string Name { get; set; } = "Handcuff";
    public string Message { get; set; } = "You were arrested for uhh commiting war crimes... or something";
    public int Weight { get; set; } = 10;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        player.Handcuff();
        player.DropItems();
    }
}