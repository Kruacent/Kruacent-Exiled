using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.CustomRoles.API.Features;
using PlayerRoles;
using UnityEngine;
using Utils.NonAllocLINQ;

namespace KE.CustomRoles.CR.Human
{
    public class Asthmatique : GlobalCustomRole
    {
        public override SideEnum Side { get; set; } = SideEnum.Human;
        public override string Description { get; set; } = "Tu es <color=#BFFF00>asthmatique</color>\nT'as stamina est réduit de moitié\nMais tu vises mieux";
        public override uint Id { get; set; } = 1042;
        public override string PublicName { get; set; } = "Asthmatique";
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = true;
        public override float SpawnChance { get; set; } = 100;


        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsedItem += OnUsedItem;
            base.SubscribeEvents();
        }
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsedItem -= OnUsedItem;
            base.UnsubscribeEvents();
        }

        private void OnUsedItem(UsedItemEventArgs ev)
        {
            if (!Check(ev.Player)) return;
            if(ev.Item.Type == ItemType.SCP500)
            {
                RemoveRole(ev.Player);
            }
        }

        protected override void RoleAdded(Player player)
        {
            player.EnableEffect(EffectType.Scp1853, -1, true);
            player.EnableEffect(EffectType.Exhausted, -1, true);
        }

        protected override void RoleRemoved(Player player)
        {
            player.DisableEffect(EffectType.Scp1853);
            player.DisableEffect(EffectType.Exhausted);
        }
    }
}
