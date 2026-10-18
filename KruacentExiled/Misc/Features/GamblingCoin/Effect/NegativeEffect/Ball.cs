using Exiled.API.Features;
using Exiled.API.Features.Items;
using InventorySystem.Items.ThrowableProjectiles;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using UnityEngine;
using EffectType = KruacentExiled.Misc.Features.GamblingCoin.Types.EffectType;

internal class Ball : ICoinEffect
{
    public string Name { get; set; } = "Ball";
    public string Message { get; set; } = "Look a BALL";
    public int Weight { get; set; } = 15;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        Scp018 theBALL = (Scp018)Item.Create(ItemType.SCP018);
        theBALL.Scale = Vector3.one * 20;

        ThrowableItem.ProjectileSettings projectileSettings = (theBALL.Base.FullThrowSettings);
        ThrownProjectile aBallThrowned = theBALL.Base.ServerThrow(projectileSettings.StartVelocity, projectileSettings.UpwardsFactor, projectileSettings.StartTorque, ThrowableNetworkHandler.GetLimitedVelocity(Vector3.one));

        aBallThrowned.Position = player.Position;
    }
}