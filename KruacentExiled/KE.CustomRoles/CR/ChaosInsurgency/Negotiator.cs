using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.CustomRoles.API.Features;
using KE.Items.API.Features;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.CR.ChaosInsurgency
{
    public class Negotiator : KECustomRole
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Negotiator",
                    [TranslationKeyDesc] = "You're immune to friendly fire and you can convert zombie into your team, isn't that nice?",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Negociateur",
                    [TranslationKeyDesc] = "T'es immunisé au tire allié et tu peux convertir des zombies",
                },
                ["legacy"] = new()
                {
                    [TranslationKeyName] = "Negotiator",
                    [TranslationKeyDesc] = "You're immune to friendly fire and you can convert zombie into your team, isn't that nice?",
                },
            };
        }
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.ChaosRifleman;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;

        public override float SpawnChance { get; set; } = 100;


        public override List<string> Inventory { get; set; } = new()
        {
            "KeycardChaosInsurgency",
            "GunAK",
            "Medkit",
            "Painkillers",
            "ArmorCombat",
            "Radio"
        };

        
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            base.UnsubscribeEvents();
        }

        protected override void GiveInventory(Player player)
        {
            base.GiveInventory(player);

            KECustomItem.TryGive(player, "Friend Maker", false);

        }


        private void OnHurting(HurtingEventArgs ev)
        {
            if (!Check(ev.Player)) return;
            if (!ev.IsAllowed) return;
            if (ev.Attacker is null) return;

            if(ev.Attacker.Role.Side == ev.Player.Role.Side)
            {
                ev.IsAllowed = false;
            }

        } 


    }
}
