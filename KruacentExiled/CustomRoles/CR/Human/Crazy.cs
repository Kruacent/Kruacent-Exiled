using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.API.Interfaces;
using MEC;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KruacentExiled.CustomRoles.CR.Human
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

    internal class Crazy : GlobalCustomRole, IHealable
    {
        private static CoroutineHandle _coroutines;
        private static CoroutineHandle _crazyingCoroutine;
        public override SideEnum Side { get; set; } = SideEnum.Human;
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Crazy",
                    [TranslationKeyDesc] = "Crazy? I Was Crazy Once. They Locked Me In A Room. A Rubber Room. A Rubber Room With Rats. And Rats Make Me Crazy",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Fou de la facilité",
                    [TranslationKeyDesc] = "Je pense que le traitement que t'as eu à la fondation t'as pas aidé",
                },
                ["legacy"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Fou de la facilité",
                    [TranslationKeyDesc] = "Je pense que le traitement que t'as eu à la fondation t'as pas aidé",
                },
            };
        }

        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override float SpawnChance { get; set; } = 100;

        private readonly Dictionary<CrazyBehaviour, int> WeightDictionnary = new Dictionary<CrazyBehaviour, int>()
        {
            { CrazyBehaviour.Jump, 3 },
            { CrazyBehaviour.Shoot, 3 },
            { CrazyBehaviour.Crazying, 1 }
        };
        private List<CrazyBehaviour> WeightedList;
        private float EFFECT_INTERVAL => UnityEngine.Random.Range(180, 300);

        public HashSet<ItemType> HealItem => new HashSet<ItemType>() { ItemType.SCP500 };

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
            WeightedList = new List<CrazyBehaviour>();

            foreach (var cb in WeightDictionnary)
            {
                WeightedList.AddRange(Enumerable.Repeat(cb.Key, cb.Value));
            }
        }

        private IEnumerator<float> ApplyEffect(Player player)
        {
            while (Check(player))
            {
                yield return Timing.WaitForSeconds(EFFECT_INTERVAL);
                CrazyBehaviour behaviour = WeightedList.GetRandomValue();

                switch (behaviour)
                {
                    case CrazyBehaviour.Jump:
                        player.IsJumping = true;
                        break;
                    case CrazyBehaviour.Shoot:
                        Player.List.ToList().Where(p => p != player).ToList().ForEach(p => p.PlayGunSound(player.Position, FirearmType.FRMG0));
                        player.PlayShieldBreakSound();
                        break;
                    case CrazyBehaviour.Crazying:
                        _crazyingCoroutine = Timing.RunCoroutine(Crazying(player));
                        break;
                    default:
                        break;
                }
            }
        }

        private IEnumerator<float> Crazying(Player player)
        {
            float duration = 10f;
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

        }
    }
}