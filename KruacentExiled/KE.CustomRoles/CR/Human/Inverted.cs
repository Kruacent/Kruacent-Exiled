using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.CustomRoles.API.Features;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace KE.CustomRoles.CR.Human
{
    public class Inverted : GlobalCustomRole
    {
        public override SideEnum Side { get; set; } = SideEnum.Human;
        public override string Description { get; set; } = "Tu as un talent assez exceptionnel !";
        public override uint Id { get; set; } = 1066;
        public override string PublicName { get; set; } = "Inverted";
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;

        public override float SpawnChance { get; set; } = 20;
        public override Vector3 Scale { get; set; } = new Vector3(1, 1, 1);

        protected override void RoleAdded(Player player)
        {
            player.Scale = new Vector3(1f, -1f, 1f);
            player.EnableEffect(EffectType.Slowness, 200, 99999);
        }

        protected override void RoleRemoved(Player player)
        {
            player.Scale = new Vector3(1f, 1f, 1f);
            player.DisableEffect(EffectType.Slowness);
        }


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
            if (ev.Item.Type == ItemType.SCP500)
            {
                RemoveRole(ev.Player);
            }
        }
    }
}