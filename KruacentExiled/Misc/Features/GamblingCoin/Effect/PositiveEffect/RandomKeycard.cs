using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;
using UnityEngine;

internal class GiveRandomKeycard : ICoinEffect
{
    public string Name { get; set; } = "GiveRandomKeycard";
    public string Message { get; set; } = "You got a keycard !";
    public int Weight { get; set; } = 20;
    public EffectType Type { get; set; } = EffectType.Positive;

    public void Execute(Player player)
    {
        LabApi.Features.Wrappers.KeycardItem keycard = LabApi.Features.Wrappers.KeycardItem.CreateCustomKeycardManagement(player, "Keycard", "Keycard",
            new Interactables.Interobjects.DoorUtils.KeycardLevels(Random.Range(0, 4), Random.Range(0, 4), Random.Range(0, 4)), RandomColor(), RandomColor(), RandomColor());

        
        player.DropItem(Item.Get(keycard.Base));
    }

    private Color32 RandomColor()
    {
        return new Color32
            (
                (byte)Random.Range(0, 256),
                (byte)Random.Range(0, 256),
                (byte)Random.Range(0, 256),
                (byte)Random.Range(0, 256)
            );
    }
}