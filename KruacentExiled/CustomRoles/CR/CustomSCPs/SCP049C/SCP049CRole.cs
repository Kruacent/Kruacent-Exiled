using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp049;
using KE.Utils.API.Displays.DisplayMeow;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.CR.CustomSCPs.SCP049C.Positions;
using KruacentExiled.CustomRoles.CR.CustomSCPs.SCP049C.UnlockableAbilities.Tier2;
using LabApi.Events.Arguments.Scp049Events;
using NorthwoodLib.Pools;
using PlayerRoles;
using PlayerStatsSystem;
using System.Collections.Generic;
using System.Text;

namespace KruacentExiled.CustomRoles.CR.CustomSCPs.SCP049C
{
    public class SCP049CRole : CustomSCP
    {
        public override bool IsSupport => false;
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "SCP049-C",
                    [TranslationKeyDesc] = "A modified SCP049 instance which does not create any SCP049-2\ninstead consuming the body to gain powerful abilities\nDoctor's call gives 300 Hume Shield\nExpired body can still be consumed but need more time to do so (stay near the arrow until it disappear (10s))",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "SCP049-C",
                    [TranslationKeyDesc] = "A modified SCP049 instance which does not create any SCP049-2\ninstead consuming the body to gain powerful abilities\nDoctor's call gives 300 Hume Shield\nExpired body can still be consumed but need more time to do so (stay near the arrow until it disappear (10s))",
                },
                ["legacy"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "SCP049-C",
                    [TranslationKeyDesc] = "A modified SCP049 instance which does not create any SCP049-2\ninstead consuming the body to gain powerful abilities\nDoctor's call gives 300 Hume Shield\nExpired body can still be consumed but need more time to do so (stay near the arrow until it disappear (10s))",
                }
            };
        }

        public override RoleTypeId Role => RoleTypeId.Scp049;

        public override string InternalName => "C";
        public override int MaxHealth { get; set; } = 2500;
        public override float SpawnChance { get; set; } = 100;
        protected override int SettingId => 10003;

        internal static SCP049CRole instance = null;

        public override void Init()
        {
            instance = this;
            base.Init();
        }
        
        public static readonly SCP049CLevelPosition HintPosition = new SCP049CLevelPosition();

        protected override void RoleAdded(Player player)
        {
            player.ReferenceHub.gameObject.AddComponent<SCP049CLevelSystem>();
            if (!DisplayHandler.Instance.HasHint(player, HintPosition.HintPlacement))
            {
                DisplayHandler.Instance.CreateAuto(player, (arg) => GetContent(player), HintPosition.HintPlacement);
            }


            player.Position = RoleTypeId.Scp939.GetRandomSpawnLocation().Position;
            base.RoleAdded(player);
        }

        

        public string GetContent(Player player)
        {
            if(!player.GameObject.TryGetComponent<SCP049CLevelSystem>(out var comp))
            {
                return " ";
            }

            StringBuilder sb = StringBuilderPool.Shared.Rent();
            bool flag = comp.MaxLevelReached;

            if (!flag)
            {
                sb.Append("Tier : ");
                sb.AppendLine(comp.Level.ToString());
            }
            else
            {
                sb.AppendLine("Max Tier");
            }

            sb.Append("Kills : ");
            sb.Append(comp.CurrentKill);
            if (!flag)
            {
                sb.Append("/");
                sb.Append(comp.KillObjective);
            }
            return StringBuilderPool.Shared.ToStringReturn(sb);
        }

        protected override void RoleRemoved(Player player)
        {
            DisplayHandler.Instance.RemoveHint(player, HintPosition.HintPlacement);


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
            Exiled.Events.Handlers.Player.Hurting += DeflectDamageUnlockable.OnHurting;
            base.SubscribeEvents();
        }
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Scp049.FinishingRecall -= OnFinishingRecall;
            LabApi.Events.Handlers.Scp049Events.UsedDoctorsCall -= OnUsedDoctorsCall;
            Exiled.Events.Handlers.Player.SpawnedRagdoll -= OnSpawnedRagdoll;
            Exiled.Events.Handlers.Player.Hurting -= DeflectDamageUnlockable.OnHurting;
            base.UnsubscribeEvents();
        }
        private void OnFinishingRecall(FinishingRecallEventArgs ev)
        {
            if (!Check(ev.Player)) return;
            if (!ev.Player.ReferenceHub.gameObject.TryGetComponent<SCP049CLevelSystem>(out var lvl)) return;
            ev.IsAllowed = false;
            ev.Ragdoll.Destroy();

            lvl.AddKill();

                
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
