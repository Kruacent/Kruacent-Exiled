using Exiled.API.Features;
using Exiled.API.Features.Items;
using InventorySystem.Items.Firearms.Attachments;
using KE.Misc.Features.GamblingCoin.Interfaces;
using KE.Misc.Features.GamblingCoin.Types;

internal class Revolver : ICoinEffect
{
    public string Name { get; set; } = "Revolver";
    public string Message { get; set; } = "It's a gift from the sky !!";
    public int Weight { get; set; } = 5;
    public EffectType Type { get; set; } = EffectType.Positive;

    public void Execute(Player player)
    {
        Firearm revo = (Firearm)Item.Create(ItemType.GunRevolver);
        revo.AddAttachment(new[]
            {AttachmentName.CylinderMag7, AttachmentName.ShortBarrel, AttachmentName.ScopeSight});
        revo.CreatePickup(player.Position);
    }
}