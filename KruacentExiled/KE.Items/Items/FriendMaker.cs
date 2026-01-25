using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.Events.EventArgs.Player;
using HintServiceMeow.UI.Utilities;
using KE.Items.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Items.Items
{

    [CustomItem(ItemType.GunCOM15)]
    public class FriendMaker : KECustomWeapon
    {
        public override uint Id { get; set; } = 8520;
        public override string Name { get; set; } = "Friend Maker";
        public override string Description { get; set; } = "The number one (1) method to makes friends";
        public override float Weight { get; set; } = 1f;
        public override SpawnProperties SpawnProperties { get; set; } = null;



        public override bool FriendlyFire { get; set; } = true;


        protected override void OnHurting(HurtingEventArgs ev)
        {

            if (!Check(ev.Attacker)) return;
            ev.IsAllowed = false;

            Convert(ev.Player,ev.Attacker);


        }



        private void Convert(Player player,Player attacker)
        {
            if (player == null)
            {
                KECustomItem.ItemEffectHint(player, "But nobody's here");
                return;
            }

            if (attacker.Role.Side == player.Role.Side)
            {
                KECustomItem.ItemEffectHint(player, "I know you don't like them but they're in your team");
                return;
            }


            if (player.IsScp && player.Role != RoleTypeId.Scp0492)
            {
                KECustomItem.ItemEffectHint(player, "That ain't a zombie");
                return;
            }

            if (player.IsScp)
            {
                player.Role.Set(player.Role, RoleSpawnFlags.AssignInventory);
            }
            else
            {
                player.Role.Set(player.Role, RoleSpawnFlags.None);
            }

            KECustomItem.ItemEffectHint(player, "New friend acquired!");
        }



    }
}
