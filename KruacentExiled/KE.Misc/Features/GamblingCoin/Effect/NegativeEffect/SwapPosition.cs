using Exiled.API.Features;
using KE.Misc.Features.GamblingCoin;
using KE.Misc.Features.GamblingCoin.Interfaces;
using KE.Misc.Features.GamblingCoin.Types;
using System.Linq;

internal class SwapPosition : ICoinEffect
{
    public string Name { get; set; } = "SwapPosition";
    public string Message { get; set; } = "Congratulations ! You just unlocked the premium teleport Uber service... but with no refunds.";
    public int Weight { get; set; } = 20;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        var playerList = Player.List.Where(x => x.IsAlive).ToList();
        playerList.Remove(player);

        if (playerList.IsEmpty())
        {
            return;
        }

        var targetPlayer = playerList.RandomItem();
        var pos = targetPlayer.Position;

        targetPlayer.Teleport(player.Position);
        PlayerUtils.SendBroadcast(targetPlayer, this.Message);
        player.Teleport(pos);
    }
}