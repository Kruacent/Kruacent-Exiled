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


    private Dictionary<Player, Light> _lights = new Dictionary<Player, Light>();
    private Dictionary<Player, AudioClipPlayback> _clips = new Dictionary<Player, AudioClipPlayback>();

    public float Duration { get; set; } = 20;
    private static CoroutineHandle _coroutines;

    public void Execute(Player player)
    {
        Light light = Light.Create(player.Position, null, null, true, UnityEngine.Color.blue);
        light.Transform.parent = player.GameObject.transform;
        light.MovementSmoothing = 0;
        _lights[player] = light;


        var c = KE.Utils.API.Sounds.SoundPlayer.Instance.Play("starman", player.GameObject,volume:.5f);

        _clips[player] = c;
        _coroutines = Timing.RunCoroutine(ColorTransformer(player));
    }

    public IEnumerator<float> ColorTransformer(Player player)
    {
        while (true)
        {
            if(_lights.TryGetValue(player,out var l))
            {
                l.Color = ColorPicker();
            }
            else
            {
                yield break;
            }
            yield return Timing.WaitForSeconds(0.5f);
        }
    }

    public static Color32 ColorPicker()
    {
        byte r = (byte)UnityEngine.Random.Range(0, 256);
        byte g = (byte)UnityEngine.Random.Range(0, 256);
        byte b = (byte)UnityEngine.Random.Range(0, 256);

        return new Color32(r, g, b, 0);
    }

    public void ExecuteAfterDuration(Player player)
    {
        Timing.KillCoroutines(_coroutines);
        Light light = _lights[player];
        light.Destroy();
        _clips[player].IsPaused = true;
        _lights.Remove(player);

    }
}