using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp049;
using KE.CustomRoles.API.Features;
using KE.Utils.API.Features;
using LabApi.Events.Arguments.Scp049Events;
using MapGeneration.Rooms;
using PlayerRoles;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.CR.CustomSCPs.SCP049C
{
    public class SCP049CRole : CustomSCP
    {
        public override bool IsSupport => false;
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "SCP049-C",
                    [TranslationKeyDesc] = "WIP",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "SCP049-C",
                    [TranslationKeyDesc] = "WIP",
                }
            };
        }

        public override RoleTypeId Role => RoleTypeId.Scp049;

        public override string InternalName => "C";
        public override int MaxHealth { get; set; } = 2500;
        public override float SpawnChance { get; set; } = 0;
        protected override int SettingId => 10003;

        internal static SCP049CRole instance = null;

        public override void Init()
        {
            instance = this;
            base.Init();
        }


        protected override void RoleAdded(Player player)
        {
            player.ReferenceHub.gameObject.AddComponent<SCP049CLevelSystem>();

            base.RoleAdded(player);
        }

        protected override void RoleRemoved(Player player)
        {

            if(player.ReferenceHub.gameObject.TryGetComponent<SCP049CLevelSystem>(out var lvl))
            {
                UnityEngine.Object.Destroy(lvl);
            }

            base.RoleRemoved(player);
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Scp049.FinishingRecall += OnFinishingRecall;
            LabApi.Events.Handlers.Scp049Events.UsedDoctorsCall += OnUsedDoctorsCall;
            Exiled.Events.Handlers.Player.SpawnedRagdoll += OnSpawnedRagdoll;
            base.SubscribeEvents();
        }
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Scp049.FinishingRecall -= OnFinishingRecall;
            LabApi.Events.Handlers.Scp049Events.UsedDoctorsCall -= OnUsedDoctorsCall;
            Exiled.Events.Handlers.Player.SpawnedRagdoll -= OnSpawnedRagdoll;
            base.UnsubscribeEvents();
        }
        private void OnFinishingRecall(FinishingRecallEventArgs ev)
        {
            if (!Check(ev.Player)) return;
            if (!ev.Player.ReferenceHub.gameObject.TryGetComponent<SCP049CLevelSystem>(out var lvl)) return;
            ev.IsAllowed = false;
            ev.Ragdoll.Destroy();


            if (true)
            {
                lvl.AddLevel();
            }
            else
            {
                lvl.AddKill();
            }

                
        }

        private void OnUsedDoctorsCall(Scp049UsedDoctorsCallEventArgs ev)
        {
            if (!Check(ev.Player)) return;
            ev.Player.GetStatModule<HumeShieldStat>().AddAmount(300f);

        }
        private void OnSpawnedRagdoll(SpawnedRagdollEventArgs ev)
        {
            RagdollArrowComp comp = ev.Ragdoll.GameObject.AddComponent<RagdollArrowComp>();
            comp.Init(ev.Ragdoll);
        }



    }
}
