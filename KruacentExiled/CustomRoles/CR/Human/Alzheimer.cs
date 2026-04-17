using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Translations.Events;
using KE.Utils.Extensions;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.API.Interfaces;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace KruacentExiled.CustomRoles.CR.Human
{
    public class Alzheimer : GlobalCustomRole, IColor, IHealable
    {
        public override SideEnum Side { get; set; } = SideEnum.Human;
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Old man",
                    [TranslationKeyDesc] = "I'm old",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Vieux",
                    [TranslationKeyDesc] = "Je suis vieux",
                },
                ["legacy"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Vieux",
                    [TranslationKeyDesc] = "POV Mishima",
                }
            };
        }
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override float SpawnChance { get; set; } = 100;

        public Color32 Color => new Color32(112,112,112,0);

        public HashSet<ItemType> HealItem => new HashSet<ItemType>() { ItemType.SCP500 };
        private Dictionary<Player, CoroutineHandle> handles;

        protected override void SubscribeEvents()
        {
            handles = DictionaryPool<Player, CoroutineHandle>.Get();
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            DictionaryPool<Player, CoroutineHandle>.Release(handles);
        }

        protected override void RoleAdded(Player player)
        {
            handles[player] = Timing.RunCoroutine(Teleport(player));
        }

        protected override void RoleRemoved(Player player)
        {
            if(handles.TryGetValue(player, out var handle))
            {
                Timing.KillCoroutines(handle);
            }
        }
        private IEnumerator<float> Teleport(Player player)
        {
            while (Check(player))
            {

                yield return Timing.WaitForSeconds(Random.Range(300f, 600f));
                EffectPlayer(player);

            }
        }



        private void EffectPlayer(Player player)
        {
            if (player is null) return;
            player.EnableEffect(EffectType.Flashed, 1, 5);
            player.EnableEffect(EffectType.Invisible, 1, 6);
            player.Teleport(player.Zone.RandomSafeRoom());
        }


    }
}
