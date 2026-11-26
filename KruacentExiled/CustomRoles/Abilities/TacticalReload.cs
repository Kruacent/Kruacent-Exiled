using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.GifAnimator;
using KE.Utils.API.Interfaces;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.API.Interfaces;
using KruacentExiled.CustomRoles.API.Interfaces.Ability;
using KruacentExiled.CustomRoles.CR.ChaosInsurgency;
using MEC;
using System;
using System.Collections.Generic;

namespace KruacentExiled.CustomRoles.Abilities
{
    public class TacticalReload : KEAbilities, ICustomIcon, IDuration
    {
        public override string Name { get; } = "TacticalReload";

        public float Duration { get; } = 5f;

        protected List<Player> PlayerList = new List<Player>();

        public void ActionAfterAbility(Player player) 
        { 
            PlayerList.Remove(player);
        }

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Tactical Reload",
                    [TranslationKeyDesc] = "Reload your Logicer when killing a player",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Recharge tactique",
                    [TranslationKeyDesc] = "Recharge ton Logicer quand tu tues un joueur",
                }
            };
        }

        public override float Cooldown { get; } = 120f;

        public TextImage IconName => MainPlugin.Instance.icons["Trade"];

        private void OnKill(DiedEventArgs ev)
        {
            Player suppressor = ev.Attacker;
            KECustomRole role = KECustomRole.Get<Suppressor>();

            if (role.Check(suppressor) && IsAbilityActive(suppressor) && !PlayerList.Contains(suppressor))
            {
                if(suppressor.CurrentItem != null && suppressor.CurrentItem.Type == ItemType.GunLogicer)
                {
                    Firearm logicer = (Firearm)suppressor.CurrentItem;
                    logicer.MagazineAmmo = logicer.MaxMagazineAmmo;
                    PlayerList.Add(suppressor);
                }
            }
        }
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Died += OnKill;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Died -= OnKill;
            base.UnsubscribeEvents();
        }

        protected override bool AbilityUsed(Player player)
        {
            return true;
        }

        
    }
}
