using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using KE.Utils.API.Translations.Events;
using KE.Utils.Extensions;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.CR.Human
{
    public class Alzheimer : GlobalCustomRole, IColor, IHealable
    {
        public override SideEnum Side { get; set; } = SideEnum.Human;
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Old man",
                    [TranslationKeyDesc] = "I'm old",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Vieux",
                    [TranslationKeyDesc] = "Je suis vieux",
                },
                ["legacy"] = new()
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

        public HashSet<ItemType> HealItem => [ItemType.SCP500];

        private static CoroutineHandle coroutine;
        protected override void RoleAdded(Player player)
        {
            Timing.RunCoroutineSingleton(Teleport(), coroutine, SingletonBehavior.Abort);
        }
        private IEnumerator<float> Teleport()
        {
            while (true)
            {

                yield return Timing.WaitForSeconds(UnityEngine.Random.Range(300f, 600f));

                foreach(Player player in TrackedPlayers)
                {
                    player.EnableEffect(EffectType.Flashed, 1, 5);
                    player.EnableEffect(EffectType.Invisible, 1, 6);
                    player.Teleport(player.Zone.RandomSafeRoom());
                }

            }
        }





    }
}
