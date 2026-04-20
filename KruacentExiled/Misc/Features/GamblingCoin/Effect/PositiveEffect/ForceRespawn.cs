using Exiled.API.Features;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;
using Respawning;

internal class ForceRespawn : ICoinEffect
{
    public string Name { get; set; } = "ForceRespawn";
    public string Message { get; set; } = "Someone respawned... probably.";
    public int Weight { get; set; } = 15;
    public EffectType Type { get; set; } = EffectType.Positive;

    public void Execute(Player player)
    {
        Respawn.ForceWave(WaveManager.Waves.RandomItem());
    }
}