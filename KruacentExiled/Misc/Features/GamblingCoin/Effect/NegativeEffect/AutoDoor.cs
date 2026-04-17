using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using MapGeneration;
using System;
using System.Linq;
using EffectType = KruacentExiled.Misc.Features.GamblingCoin.Types.EffectType;

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

        

        foreach(Door door in Door.List.Where(d => d.Position.GetZone() == randomZone && d.DoorLockType == DoorLockType.None))
        {
            door.IsOpen = true;
        }
        
    }
}