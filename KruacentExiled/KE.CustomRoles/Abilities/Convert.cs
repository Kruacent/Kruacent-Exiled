using DrawableLine;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Toys;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.Abilities
{
    public class Convert : KEAbilities, ICustomIcon
    {
        public override string Name { get;  } = "Convert";

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Convert",
                    [TranslationKeyDesc] = "Convert a zombie to your team",
                    ["ConvertNobody"] = "But nobody's here",
                    ["ConvertSameTeam"] = "I know you don't like them, but they're in your team",
                    ["ConvertNonZombie"] = "That ain't a zombie",
                    ["ConvertSuccess"] = "New friend acquired!",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Convert",
                    [TranslationKeyDesc] = "Converti un zombie à la bonne foi",
                    ["ConvertNobody"] = "Mais personne n'est venu",
                    ["ConvertSameTeam"] = "Je sais que tu l'aimes pas mais il est bien avec toi",
                    ["ConvertNonZombie"] = "C'est pas un zombie ça",
                    ["ConvertSuccess"] = "Ami obtenu!",
                }
            };
        }

        public override float Cooldown { get;  } = 10*60f;

        public float MaxDistance { get; set; } = 15f;

        public Utils.API.GifAnimator.TextImage IconName => MainPlugin.Instance.icons["Convert"];
        protected override bool AbilityUsed(Player player)
        {
            Vector3 start = player.CameraTransform.position+ player.CameraTransform.forward*.2f;
            Vector3 end = start + player.CameraTransform.forward * 5f;

            Vector3 basePosition = player.Position + player.CameraTransform.rotation * Vector3.forward;
            DrawableLines.IsDebugModeEnabled = MainPlugin.Instance.Config.Debug;
            DrawableLines.ServerGenerateLine(10f,null,start, end);


            

            if (!Physics.Linecast(start, end, out RaycastHit hit)) return false;


            Player playerHit = Player.Get(hit.collider);

            if (playerHit == null || playerHit == player)
            {
                MainPlugin.ShowEffectHint(player, "But nobody's here");
                return false;
            }


            if (playerHit.Role.Side == player.Role.Side)
            {
                MainPlugin.ShowEffectHint(player, "I know you don't like them but they're in your team");
                return false;
            }

            if (playerHit.IsScp && playerHit.Role != RoleTypeId.Scp0492)
            {
                MainPlugin.ShowEffectHint(player, "That ain't a zombie");
                return false;
            }


            if (playerHit.IsScp)
            {
                playerHit.Role.Set(player.Role, RoleSpawnFlags.AssignInventory);
            }
            else
            {
                playerHit.Role.Set(player.Role, RoleSpawnFlags.None);
            }

            MainPlugin.ShowEffectHint(player, "New friend acquired!");
            return base.AbilityUsed(player);
        }

    }
}
