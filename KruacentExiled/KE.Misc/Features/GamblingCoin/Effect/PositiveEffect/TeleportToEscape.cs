using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using KE.Misc.Features.GamblingCoin.Interfaces;
using EffectType = KE.Misc.Features.GamblingCoin.Types.EffectType;

internal class TeleportToEscape : ICoinEffect
{
    public string Name { get; set; } = "TeleportToEscape";
    public string Message { get; set; } = string.Empty;
    public int Weight { get; set; } = 5;
    public EffectType Type { get; set; } = EffectType.Positive;

    public void Execute(Player player)
    {
        player.Teleport(Door.Get(DoorType.EscapeSecondary));
    }
}