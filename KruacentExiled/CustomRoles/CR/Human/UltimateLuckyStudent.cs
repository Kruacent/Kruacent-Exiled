using Exiled.Events.EventArgs.Player;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.API.Interfaces;
using LabApi.Events.Arguments.PlayerEvents;
using MEC;
using PlayerStatsSystem;
using System.Collections.Generic;
using UnityEngine;

namespace KruacentExiled.CustomRoles.CR.Human
{
    public class UltimateLuckyStudent : GlobalCustomRole, IColor, ILucky
    {
        public override SideEnum Side { get; set; } = SideEnum.Human;
        public LuckProfile LuckProfile => LuckProfile.UltimateLucky;
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Ultimate Lucky Student",
                    [TranslationKeyDesc] = "You are extremely lucky from now on, use it wisely.",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Chanceux Uttime",
                    [TranslationKeyDesc] = "Tu es extrêmement chanceux à partir de maintenant, utilise la à bon escient",
                },
                ["legacy"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Ultimate Lucky Student",
                    [TranslationKeyDesc] = "Romain dit que la description devrait être : 'Gamble positif'",
                }
            };
        }
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override float SpawnChance { get; set; } = 100;
        public Color32 Color => new Color32(3, 238, 255, 0);

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting += DisableDamageFor018;
            LabApi.Events.Handlers.PlayerEvents.LeavingPocketDimension += AlwaysComeback;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            Exiled.Events.Handlers.Player.Hurting -= DisableDamageFor018;
            LabApi.Events.Handlers.PlayerEvents.LeavingPocketDimension -= AlwaysComeback;
        }

        public void DisableDamageFor018(HurtingEventArgs ev)
        {
            if (!Check(ev.Player)) return;

            if(ev.DamageHandler.Base is Scp018DamageHandler ball)
            {
                ev.IsAllowed = false;
            }
        }

        public void AlwaysComeback(PlayerLeavingPocketDimensionEventArgs ev)
        {
            if (!Check(ev.Player)) return;

            ev.IsSuccessful = true;
        }

    }
}
