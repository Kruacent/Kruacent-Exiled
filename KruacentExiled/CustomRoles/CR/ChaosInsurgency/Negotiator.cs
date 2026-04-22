using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using KruacentExiled.CustomRoles.API.Features;
using PlayerRoles;
using System.Collections.Generic;

namespace KruacentExiled.CustomRoles.CR.ChaosInsurgency
{
    public class Negotiator : KECustomRole
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Negotiator",
                    [TranslationKeyDesc] = "You're immune to friendly fire and you can convert zombie into your team, isn't that nice?",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Negociateur",
                    [TranslationKeyDesc] = "T'es immunisé au tire allié et tu peux convertir des zombies",
                },
                ["legacy"] = new Dictionary<string, string>()
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


        public override List<string> Inventory { get; set; } = new List<string>()
        {
            "KeycardChaosInsurgency",
            "GunAK",
            "Medkit",
            "Painkillers",
            "ArmorCombat",
            "Radio"
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Ammo12Gauge, 7 }, { AmmoType.Nato762, 90 }
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

            Exiled.CustomItems.API.Features.CustomItem.TryGive(player, "FriendMaker", false);

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
