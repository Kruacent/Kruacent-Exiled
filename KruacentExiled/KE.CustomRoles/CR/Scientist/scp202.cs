using KE.CustomRoles.API.Features;
using Exiled.API.Features;
using UnityEngine;
using Exiled.Events.EventArgs.Player;
using KE.CustomRoles.API.Interfaces;
using PlayerRoles;
using System.Collections.Generic;

using Exiled.API.Extensions;

namespace KE.CustomRoles.CR.Scientist
{
    public class SCP202 : KECustomRole, IColor
    {

        public override int MaxHealth { get; set; } = 100;

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "SCP-202",
                    [TranslationKeyDesc] = "? emsizan-itna'd te emsinummoc itna'd eiv enu sèrpa\n7691 lirva 91 el trom te engoloc à 6781 reivnaj 5 el én\n,etarcoméd neitérhc itrap ud eitrap tnasiaf 5691 à 9491 ed reilecnahC\n?elaidnom erreug ednoces al sèrpa dnamella reilecnahc re1 el darnoK erid xuev uT ?drannoC",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "SCP-202",
                    [TranslationKeyDesc] = "? emsizan-itna'd te emsinummoc itna'd eiv enu sèrpa\n7691 lirva 91 el trom te engoloc à 6781 reivnaj 5 el én\n,etarcoméd neitérhc itrap ud eitrap tnasiaf 5691 à 9491 ed reilecnahC\n?elaidnom erreug ednoces al sèrpa dnamella reilecnahc re1 el darnoK erid xuev uT ?drannoC",
                }
            };
        }

        public override RoleTypeId Role { get; set; } = RoleTypeId.Scientist;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = true;

        public override float SpawnChance { get; set; } = 100;
        public Color32 Color => new Color32(191, 255, 183, 0);

        public static readonly Vector3 scale = new(-1,1,1);

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

        protected override void ClearInventory(Player player)
        {
            //empty to keep the inventory
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
            player.SetFakeScale(scale, Player.Enumerable);
        }

        protected override void RoleRemoved(Player player)
        {
            player.SetFakeScale(player.Scale, Player.Enumerable);

        }
    }
}