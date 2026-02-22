using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.API.Features;
using KE.Utils.Extensions;
using MEC;
using NetworkManagerUtils.Dummies;
using PlayerRoles;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KE.CustomRoles.CR.Human
{
    /// <summary>
    /// Behaviour of the crazy custom role, with the name and the weight (the higher the weight, the more it will happen)
    /// </summary>
    public enum CrazyBehaviour
    {
        Jump,
        Shoot,
        Vision,
        Crazying
    }

    internal class Crazy : GlobalCustomRole
    {
        private static CoroutineHandle _coroutines;
        private static CoroutineHandle _crazyingCoroutine;
        public override SideEnum Side { get; set; } = SideEnum.Human;
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Crazy",
                    [TranslationKeyDesc] = "Crazy? I Was Crazy Once. They Locked Me In A Room. A Rubber Room. A Rubber Room With Rats. And Rats Make Me Crazy",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Fou de la facilité",
                    [TranslationKeyDesc] = "Je pense que le traitement que t'as eu à la fondation t'as pas aidé",
                }
            };
        }

        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override float SpawnChance { get; set; } = 100;

        private readonly Dictionary<CrazyBehaviour, int> WeightDictionnary = new()
        {
            { CrazyBehaviour.Jump, 3 },
            { CrazyBehaviour.Shoot, 3 },
            { CrazyBehaviour.Vision, 2 },
            { CrazyBehaviour.Crazying, 1 }
        };
        private List<CrazyBehaviour> WeightedList;
        private readonly float EFFECT_INTERVAL = UnityEngine.Random.Range(180, 300);

        protected override void RoleAdded(Player player)
        {
            PreparationEffect();
            _coroutines = Timing.RunCoroutine(ApplyEffect(player));
        }

        protected override void RoleRemoved(Player player)
        {
            Timing.KillCoroutines(_coroutines);
        }

        private void PreparationEffect()
        {
            this.WeightedList = new List<CrazyBehaviour>();

            foreach (var cb in WeightDictionnary)
            {
               this.WeightedList.AddRange(Enumerable.Repeat(cb.Key, cb.Value));
            }
        }

        private IEnumerator<float> ApplyEffect(Player player)
        {
            while (Check(player))
            {
                yield return Timing.WaitForSeconds(this.EFFECT_INTERVAL);
                CrazyBehaviour behaviour = this.WeightedList.GetRandomValue();

                switch (behaviour)
                {
                    case CrazyBehaviour.Jump:
                        player.IsJumping = true;
                        break;
                    case CrazyBehaviour.Shoot:
                        Player.List.ToList().Where(p => p != player).ToList().ForEach(p => p.PlayGunSound(player.Position, FirearmType.FRMG0));
                        player.PlayShieldBreakSound();
                        break;
                    case CrazyBehaviour.Vision:
                        //Vision(player);
                        break;
                    case CrazyBehaviour.Crazying:
                        _crazyingCoroutine = Timing.RunCoroutine(Crazying(player));
                        break;
                    default:
                        break;
                }
            }
        }

        private void Vision(Player player)
        {
            List<RoleTypeId> scpRole = new List<RoleTypeId> { RoleTypeId.Scp3114, RoleTypeId.Scp173, RoleTypeId.Scp096, RoleTypeId.Scp106, RoleTypeId.Scp049, RoleTypeId.Scp939 };

            DummyUtils.SpawnDummy(Player.List.ToList().Where(p => p.IsScp).First().DisplayNickname); // Setting name of the dummy
            Player bot = Player.List.ToList().Where(p => p.IsNPC).First(); // Getting the bot
            bot.Role.Set(RoleTypeId.Tutorial);
            bot.ChangeAppearance(scpRole.GetRandomValue());
            bot.Health = 5000;

            bot.Position = player.Position + (-player.CameraTransform.forward) * 2;
            player.PlayGunSound(bot.Position, FirearmType.ParticleDisruptor);
            Timing.CallDelayed(2f, () => bot.Kick(""));
        }

        private IEnumerator<float> Crazying(Player player)
        {
            float duration = 15f;
            float timer = 0f;

            while (timer < duration)
            {
                float randomYaw = UnityEngine.Random.Range(-180f, 180f);
                float randomPitch = UnityEngine.Random.Range(-30f, 30f);

                Quaternion crazyRot = Quaternion.Euler(randomPitch, randomYaw, 0f);
                player.Rotation = crazyRot;

                for (int i = 0; i < 5; i++)
                {
                    float shakeYaw = UnityEngine.Random.Range(-5f, 5f);
                    float shakePitch = UnityEngine.Random.Range(-5f, 5f);

                    Quaternion shakeRot = Quaternion.Euler(shakePitch, shakeYaw, 0f);
                    player.Rotation *= shakeRot;

                    yield return Timing.WaitForSeconds(0.05f);
                    timer += 0.05f;
                }
            }

            Timing.KillCoroutines(_crazyingCoroutine);
        }
    }
}