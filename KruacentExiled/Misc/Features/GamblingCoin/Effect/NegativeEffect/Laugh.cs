using Exiled.API.Features;
using KE.Utils.API.Features;
using KruacentExiled.Audio;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;
using MEC;
using System.Collections.Generic;

public class Laugh : IDurationEffect
{
    public string Name { get; set; } = "Laugh";
    public string Message { get; set; } = "Smile you are on a TV show";
    public int Weight { get; set; } = 7;
    public EffectType Type { get; set; } = EffectType.Negative;

    public float Duration { get; set; } = 300f;

    private const float MinDelay = 30f;
    private const float MaxDelay = 60f;

    private readonly Dictionary<Player, CoroutineHandle> _coroutines = new Dictionary<Player, CoroutineHandle>();

    public void Execute(Player player)
    {
        if (_coroutines.TryGetValue(player, out CoroutineHandle running))
        {
            Timing.KillCoroutines(running);
        }

        _coroutines[player] = Timing.RunCoroutine(RandomLaugh(player));
    }

    private IEnumerator<float> RandomLaugh(Player player)
    {
        int life = player.Role.Base.UniqueLifeIdentifier;

        while (true)
        {
            if (!player.IsConnected || player.Role.Base.UniqueLifeIdentifier != life)
            {
                KELog.Debug("breaked");
                yield break;
            }

            AudioHandler.Instance.Play(player, SoundType.Noise, "laugh", player.GameObject, 50);

            yield return Timing.WaitForSeconds(UnityEngine.Random.Range(MinDelay, MaxDelay));
        }


    }

    public void ExecuteAfterDuration(Player player)
    {
        if (_coroutines.TryGetValue(player, out CoroutineHandle running))
        {
            KELog.Debug("killed");
            Timing.KillCoroutines(running);
            _coroutines.Remove(player);
        }
    }
}