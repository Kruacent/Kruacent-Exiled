using Exiled.API.Features;
using Exiled.API.Features.Doors;
using KE.Misc.Features.GamblingCoin;
using KE.Misc.Features.GamblingCoin.Interfaces;
using MapGeneration;
using System;
using System.Linq;
using EffectType = KE.Misc.Features.GamblingCoin.Types.EffectType;

internal class AutoDoor : ICoinEffect
{
    public string Name { get; set; } = "AutoDoor";
    public string Message { get; set; } = "You are very talented in messing up the game !";
    public int Weight { get; set; } = 5;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        Array values = Enum.GetValues(typeof(FacilityZone));
        FacilityZone randomZone = (FacilityZone)values.GetValue(UnityEngine.Random.Range(0, values.Length));

        Door.List.Where(d => d.Position.GetZone() == randomZone).ToList().ForEach(d => d.IsOpen = true);

        PlayerUtils.SendBroadcast(player, "Clap clap clap ! You opened door in " + randomZone);
    }
}