using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using KE.Misc.Features.GamblingCoin.Interfaces;
using EffectType = KE.Misc.Features.GamblingCoin.Types.EffectType;

internal class TeleportToDBoyCell : ICoinEffect
{
    public string Name { get; set; } = "TeleportToDBoyCell";
    public string Message { get; set; } = "You got teleported to Class D cells !";
    public int Weight { get; set; } = 5;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        player.DropHeldItem();
        player.Teleport(Door.Get(DoorType.PrisonDoor));

        if (Warhead.IsDetonated)
        {
            player.Kill(DamageType.Decontamination, "You were teleported into a radioactive zone.");
        }
    }
}