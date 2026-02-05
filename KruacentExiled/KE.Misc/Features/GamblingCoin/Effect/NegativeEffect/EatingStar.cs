using Exiled.API.Features;
using Exiled.API.Features.Toys;
using KE.Misc.Features.GamblingCoin.Interfaces;
using KE.Misc.Features.GamblingCoin.Types;
using MEC;
using System.Collections.Generic;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

public class EatingStar : IDurationEffect
{
    public string Name { get; set; } = "EatingStar";
    public string Message { get; set; } = "Eating the disco ball wasn't good idea";
    public int Weight { get; set; } = 2;
    public EffectType Type { get; set; } = EffectType.Negative;


    private Dictionary<Player, Light> _lights;

    public float Duration { get; set; } = 20;
    private static CoroutineHandle _coroutines;

    public void Execute(Player player)
    {
        _lights[player] = Light.Create(player.Position, null, null, true, UnityEngine.Color.blue);
        Light light = _lights[player];
        light.Transform.parent = player.Transform;
        light.MovementSmoothing = 0;

        _coroutines = Timing.RunCoroutine(ColorTransformer());
    }

    public IEnumerator<float> ColorTransformer()
    {
        while (true)
        {
            foreach(var kvp in _lights)
            {
                kvp.Value.Color = ColorPicker();
            }

            yield return Timing.WaitForSeconds(0.5f);
        }
    }

    public static UnityEngine.Color ColorPicker()
    {
        byte r = (byte)UnityEngine.Random.Range(0, 256);
        byte g = (byte)UnityEngine.Random.Range(0, 256);
        byte b = (byte)UnityEngine.Random.Range(0, 256);

        return new UnityEngine.Color32(r, g, b, 0);
    }

    public void ExecuteAfterDuration(Player player)
    {
        Timing.KillCoroutines(_coroutines);
    }
}