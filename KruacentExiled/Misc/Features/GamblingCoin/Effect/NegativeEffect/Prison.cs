using AdminToys;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using EffectType = KruacentExiled.Misc.Features.GamblingCoin.Types.EffectType;

public class Prison : IDurationEffect
{
    public string Name { get; set; } = "Prison";
    public string Message { get; set; } = "Not another warcrime mate, what are you doing";
    public int Weight { get; set; } = 10;
    public EffectType Type { get; set; } = EffectType.Negative;

    public float Duration { get; set; } = 20f;

    private static readonly Color BarColor = new Color(255, 255, 255);

    private readonly Queue<List<Primitive>> _prisons = new Queue<List<Primitive>>();

    public Prison()
    {
        Exiled.Events.Handlers.Server.RestartingRound += OnRestartingRound;
    }

    public void Execute(Player player)
    {
        _prisons.Enqueue(CreatePrison(player.Position));
    }

    public void ExecuteAfterDuration(Player player)
    {
        if (_prisons.Count > 0)
        {
            DestroyPrison(_prisons.Dequeue());
        }
    }

    private List<Primitive> CreatePrison(Vector3 center)
    {
        Vector3 bar = new Vector3(0.05f, 3f, 0.05f);
        Vector3 roof = new Vector3(0.05f, 0.05f, 1.6f);

        return new List<Primitive>()
        {
            CreateBar(center + new Vector3(-0.8f, 0, -0.8f), bar),
            CreateBar(center + new Vector3(-0.8f, 0, -0.4f), bar),
            CreateBar(center + new Vector3(-0.8f, 0, 0f), bar),
            CreateBar(center + new Vector3(-0.8f, 0, 0.4f), bar),
            CreateBar(center + new Vector3(-0.8f, 0, 0.8f), bar),

            CreateBar(center + new Vector3(0.8f, 0, -0.8f), bar),
            CreateBar(center + new Vector3(0.8f, 0, -0.4f), bar),
            CreateBar(center + new Vector3(0.8f, 0, 0f), bar),
            CreateBar(center + new Vector3(0.8f, 0, 0.4f), bar),
            CreateBar(center + new Vector3(0.8f, 0, 0.8f), bar),

            CreateBar(center + new Vector3(-0.4f, 0, -0.8f), bar),
            CreateBar(center + new Vector3(0f, 0, -0.8f), bar),
            CreateBar(center + new Vector3(0.4f, 0, -0.8f), bar),

            CreateBar(center + new Vector3(-0.4f, 0, 0.8f), bar),
            CreateBar(center + new Vector3(0f, 0, 0.8f), bar),
            CreateBar(center + new Vector3(0.4f, 0, 0.8f), bar),

            CreateBar(center + new Vector3(-0.8f, 1.4f, 0), roof),
            CreateBar(center + new Vector3(-0.4f, 1.4f, 0), roof),
            CreateBar(center + new Vector3(0f, 1.4f, 0), roof),
            CreateBar(center + new Vector3(0.4f, 1.4f, 0), roof),
            CreateBar(center + new Vector3(0.8f, 1.4f, 0), roof),
        };
    }

    private Primitive CreateBar(Vector3 position, Vector3 scale)
    {
        return Primitive.Create(PrimitiveType.Cube, PrimitiveFlags.Collidable | PrimitiveFlags.Visible, position, null, scale, true, BarColor);
    }

    private void DestroyPrison(List<Primitive> bars)
    {
        foreach (Primitive bar in bars)
        {
            bar.Destroy();
        }
    }

    private void OnRestartingRound()
    {
        while (_prisons.Count > 0)
        {
            DestroyPrison(_prisons.Dequeue());
        }
    }
}
