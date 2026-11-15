using Exiled.API.Extensions;
using Exiled.API.Features;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;

internal class TransformZombieFake : ICoinEffect
{
    public string Name { get; set; } = "TransformZombieFake";
    public string Message { get; set; } = "Good luck";
    public int Weight { get; set; } = 8;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        player.ChangeAppearance(PlayerRoles.RoleTypeId.Scp0492);
    }
}