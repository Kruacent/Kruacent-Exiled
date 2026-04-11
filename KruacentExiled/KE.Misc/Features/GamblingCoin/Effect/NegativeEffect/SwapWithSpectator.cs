using Exiled.API.Extensions;
using Exiled.API.Features;
using KE.Misc.Features.GamblingCoin;
using KE.Misc.Features.GamblingCoin.Interfaces;
using KE.Misc.Features.GamblingCoin.Types;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;

public class SwapWithSpectator : ICoinEffect
{
    public string Name { get; set; } = "SwapWithSpectator";
    public string Message { get; set; } = "You just made someone's round better !";
    public int Weight { get; set; } = 10;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        var spectList = Player.List.Where(x => x.Role.Type == RoleTypeId.Spectator).ToList();

        if (spectList.IsEmpty())
        {
            PlayerUtils.SendBroadcast(player, "you got lucky !");
            return;
        }

        var spect = spectList.RandomItem();

        spect.Role.Set(player.Role.Type, RoleSpawnFlags.None);
        spect.Teleport(player);
        spect.Health = player.Health;

        List<ItemType> playerItems = player.Items.Select(item => item.Type).ToList();

        foreach (var item in playerItems)
        {
            spect.AddItem(item);
        }


        for (int i = 0; i < player.Ammo.Count; i++)
        {
            spect.AddAmmo(player.Ammo.ElementAt(i).Key.GetAmmoType(), player.Ammo.ElementAt(i).Value);
            player.SetAmmo(player.Ammo.ElementAt(i).Key.GetAmmoType(), 0);
        }

        player.ClearInventory();
        player.Role.Set(RoleTypeId.Spectator);
    }
}