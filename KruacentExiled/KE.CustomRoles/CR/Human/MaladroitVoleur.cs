using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using UnityEngine;
using Utils.NonAllocLINQ;

namespace KE.CustomRoles.CR.Human
{
    public class MaladroitVoleur : GlobalCustomRole, IColor
    {

        public override SideEnum Side { get; set; } = SideEnum.Human;
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Butter Finger Thief",
                    [TranslationKeyDesc] = "Be careful of \"your\" items!",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Maladroit Voleur",
                    [TranslationKeyDesc] = "Fais attention à \"tes\" objets !",
                },
                ["legacy"] = new()
                {
                    [TranslationKeyName] = "Maladroit Voleur",
                    [TranslationKeyDesc] = "Fais attention à \"tes\" objets !",
                }
            };
        }
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override float SpawnChance { get; set; } = 100;

        public Color32 Color => new(211, 110, 112, 0);

        public override HashSet<string> Abilities { get; } = new()
        {
            "Thief"
        };

        protected override void RoleAdded(Player player)
        {
            Timing.RunCoroutine(ThrowingItem(player));
        }


        private IEnumerator<float> ThrowingItem(Player player)
        {

            while (TrackedPlayers.Contains(player))
            {
                yield return Timing.WaitForSeconds(UnityEngine.Random.Range(90f, 120f));
                EffectPlayer(player);


            }
        }


        private void EffectPlayer(Player player)
        {
            if (UnityEngine.Random.Range(0f, 100f) > .5f)
            {
                player.DropHeldItem();
            }
        }
    }
}