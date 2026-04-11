using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using KE.Misc.Features.GamblingCoin.Interfaces;
using KE.Misc.Features.GamblingCoin.Types;
using UnityEngine;

internal class GiveKeycard : ICoinEffect
{
    public string Name { get; set; } = "GiveKeycard";
    public string Message { get; set; } = "You got a keycard !";
    public int Weight { get; set; } = 20;
    public EffectType Type { get; set; } = EffectType.Positive;

    /// <summary>
    /// Chance in % to get a FacilityManager Card instead of containement engineer one.
    /// </summary>
    public static float FacilityManagerCard = 15;

    public void Execute(Player player)
    {
        float random = UnityEngine.Random.Range(1f, 100f);
        ItemType keycard = ItemType.KeycardContainmentEngineer;

        if(random <= FacilityManagerCard)
        {
            keycard = ItemType.KeycardFacilityManager;
        }

        Pickup.CreateAndSpawn(keycard, player.Position, new Quaternion());
    }
}