using Exiled.API.Features;
using Exiled.API.Features.Toys;
using KE.Misc.Features.GamblingCoin.Interfaces;
using KE.Misc.Features.GamblingCoin.Types;
using MEC;
using System.Collections.Generic;
using UnityEngine;

public class EatingStar : IDurationEffect
{
    public string Name { get; set; } = "EatingStar";
    public string Message { get; set; } = "Eating the disco ball wasn't good idea";
    public int Weight { get; set; } = 2;
    public EffectType Type { get; set; } = EffectType.Negative;
    public static Exiled.API.Features.Toys.Light Light { get; set; }
    public float Duration { get; set; } = -1;
    private static CoroutineHandle _coroutines;

    public void Execute(Player player)
    {
        Light = Exiled.API.Features.Toys.Light.Create(player.Position, null, null, true, UnityEngine.Color.blue);
        Light.Transform.parent = player.Transform;

        _coroutines = Timing.RunCoroutine(ColorTransformer());
    }

    public IEnumerator<float> ColorTransformer()
    {
        while (true)
        {
            Light.Color = ColorPicker();
            Timing.WaitForSeconds(0.5f);
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