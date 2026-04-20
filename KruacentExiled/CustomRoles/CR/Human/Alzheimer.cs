using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pools;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using HarmonyLib;
using HintServiceMeow.Core.Enum;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using KE.Utils.API.Features;
using KE.Utils.API.Translations.Events;
using KE.Utils.Extensions;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.API.Interfaces;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using KruacentExiled.Misc.Features.Spawn;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            handles = new Dictionary<Player, CoroutineHandle>();
            
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            handles = null;
        }

        private static HintPosition HintPosition = new OldPosition();

        protected override void RoleAdded(Player player)
        {
            try
            {
                if (Round.ElapsedTime.TotalSeconds <= 20 )
                {
                    Timing.CallDelayed(1f, () => CreateSpawnHint(player));
                }
                handles[player] = Timing.RunCoroutine(Teleport(player));
            }catch(Exception e)
            {
                Log.Error(e);
            }
            
        }

        private void CreateSpawnHint(Player player)
        {
            ReferenceHub randomscp = SCPTeam.SCPs.GetRandomValue(p => p.GetRoleId() != RoleTypeId.Scp0492);

            if (randomscp == null) return;

            ISCPPreferences trueSCP = Spawn.GetSCP(Player.Get(randomscp));

            KELog.Debug(trueSCP.SCPId);

            List<ISCPPreferences> allOtherRole = Spawn.allRoles.Where(s => s != trueSCP).ToList();

            ISCPPreferences fakeSCP1 = allOtherRole.PullRandomItem();
            KELog.Debug(fakeSCP1.SCPId);
            ISCPPreferences fakeSCP2 = allOtherRole.PullRandomItem();
            KELog.Debug(fakeSCP2.SCPId);

            allOtherRole.Clear();

            //todo add params in utils to avoid that
            allOtherRole.Add(trueSCP);
            allOtherRole.Add(fakeSCP1);
            allOtherRole.Add(fakeSCP2);

            allOtherRole.ShuffleListSecure();

            StringBuilder sb = StringBuilderPool.Pool.Get();
            sb.AppendLine("<color=red><size=20> probable SCP(s) :")
                .Append(allOtherRole.ElementAt(0).SCPId)
                .Append(' ')
                .Append(allOtherRole.ElementAt(1).SCPId)
                .Append(' ')
                .Append(allOtherRole.ElementAt(2).SCPId)
                .Append("</size></color>");

            DisplayHandler.Instance.AddHint(HintPosition.HintPlacement, player, StringBuilderPool.Pool.ToStringReturn(sb), 10);
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
            yield return Timing.WaitForSeconds(UnityEngine.Random.Range(300f, 600f));
            while (Check(player))
            {
                EffectPlayer(player);
                yield return Timing.WaitForSeconds(UnityEngine.Random.Range(300f, 600f));
                

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

    public class OldPosition : HintPosition
    {
        public override float Xposition => 0;

        public override float Yposition => 250;

        public override HintAlignment HintAlignment => HintAlignment.Center;

        public override string Name => "OldSCP";
    }
}
