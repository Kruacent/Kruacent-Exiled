using KE.CustomRoles.API.Features;
using Exiled.API.Features;
using UnityEngine;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Mirror;
using System.Linq;
using Exiled.Events.EventArgs.Player;
using KE.CustomRoles.API.Interfaces;

namespace KE.CustomRoles.CR.Human
{
    public class Inverted : GlobalCustomRole, IColor
    {
        public override SideEnum Side { get; set; } = SideEnum.Human;
        public override string Description { get; set; } = "? emsizan-itna'd te emsinummoc itna'd eiv enu sèrpa\n7691 lirva 91 el trom te engoloc à 6781 reivnaj 5 el én\n,etarcoméd neitérhc itrap ud eitrap tnasiaf 5691 à 9491 ed reilecnahC\n?elaidnom erreug ednoces al sèrpa dnamella reilecnahc re1 el darnoK erid xuev uT ?drannoC";
        public override uint Id { get; set; } = 1066;
        public override string PublicName { get; set; } = "SCP-202";
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = true;

        public override float SpawnChance { get; set; } = 100;
        public override Vector3 Scale { get; set; } = new Vector3(1, 1, 1);
        public Color32 Color => new Color32(191, 255, 183, 0);

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
        protected override void RoleAdded(Player player)
        {
            player.Scale = new Vector3(1f, 1f, -1f);
        }

        protected override void RoleRemoved(Player player)
        {
            player.Scale = new Vector3(1f, 1f, 1f);

        }
    }
}