using Exiled.API.Features;
using KE.Misc.Features.GamblingCoin.Interfaces;
using KE.Misc.Features.GamblingCoin.Types;

internal class TransformZombie : ICoinEffect
{
    public string Name { get; set; } = "TransformZombie";
    public string Message { get; set; } = string.Empty;
    public int Weight { get; set; } = 30;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        player.Role.Set(PlayerRoles.RoleTypeId.Scp0492);
    }
}