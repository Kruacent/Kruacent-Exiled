using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using KE.Misc.Features.GamblingCoin;
using KE.Misc.Features.GamblingCoin.Interfaces;
using System.Collections.Generic;
using System.Linq;
using EffectType = KE.Misc.Features.GamblingCoin.Types.EffectType;

public class SwapInventory : ICoinEffect
{
    public string Name { get; set; } = "SwapInventory";
    public string Message { get; set; } = "I think you don't deserve this stuff, i take it away";
    public int Weight { get; set; } = 20;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        List<Player> playerList = Player.List.Where(x => x != player && !x.IsScp).ToList();

        if (playerList.Count(x => x.IsAlive) <= 1)
        {
            player.Hurt(50);
            return;
        }

        var target = playerList.Where(x => x != player).ToList().RandomItem();

        List<ItemType> items1 = player.Items.Select(item => item.Type).ToList();
        List<ItemType> items2 = target.Items.Select(item => item.Type).ToList();

        Dictionary<AmmoType, ushort> ammo1 = new Dictionary<AmmoType, ushort>();
        Dictionary<AmmoType, ushort> ammo2 = new Dictionary<AmmoType, ushort>();
        for (int i = 0; i < player.Ammo.Count; i++)
        {
            ammo1.Add(player.Ammo.ElementAt(i).Key.GetAmmoType(), player.Ammo.ElementAt(i).Value);
            player.SetAmmo(ammo1.ElementAt(i).Key, 0);
        }
        for (int i = 0; i < target.Ammo.Count; i++)
        {
            ammo2.Add(target.Ammo.ElementAt(i).Key.GetAmmoType(), target.Ammo.ElementAt(i).Value);
            target.SetAmmo(ammo2.ElementAt(i).Key, 0);
        }

        target.ResetInventory(items1);
        player.ResetInventory(items2);

        foreach (var ammo in ammo2)
        {
            player.SetAmmo(ammo.Key, ammo.Value);
        }
        foreach (var ammo in ammo1)
        {
            target.SetAmmo(ammo.Key, ammo.Value);
        }

        PlayerUtils.SendBroadcast(target, this.Message);
    }
}