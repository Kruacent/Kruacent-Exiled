using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Scp049;
using KE.CustomRoles.API.Features;
using LabApi.Events.Arguments.Scp049Events;
using MapGeneration.Rooms;
using PlayerRoles;
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

        public override string PublicName { get; set; } = "SCP049-C";

        public override RoleTypeId Role => RoleTypeId.Scp049;

        public override int MaxHealth { get; set; } = 2500;
        public override string Description { get; set; }
        public override float SpawnChance { get; set; } = 0;
        protected override int SettingId => 10003;



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
            LabApi.Events.Handlers.Scp049Events.UsingDoctorsCall += OnUsingDoctorsCall;
            LabApi.Events.Handlers.Scp049Events.UsedDoctorsCall += OnUsedDoctorsCall;
            base.SubscribeEvents();
        }
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Scp049.FinishingRecall -= OnFinishingRecall;
            LabApi.Events.Handlers.Scp049Events.UsingDoctorsCall -= OnUsingDoctorsCall;
            LabApi.Events.Handlers.Scp049Events.UsedDoctorsCall -= OnUsedDoctorsCall;
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

        private void OnUsingDoctorsCall(Scp049UsingDoctorsCallEventArgs ev)
        {
            if (!Check(ev.Player)) return;

            ev.Player.HumeShieldRegenRate = 15*2;
            ev.Player.HumeShieldRegenCooldown = 10/2;

        }

        private void OnUsedDoctorsCall(Scp049UsedDoctorsCallEventArgs ev)
        {
            if (!Check(ev.Player)) return;

            ev.Player.HumeShieldRegenRate = 15;
            ev.Player.HumeShieldRegenCooldown = 10;


        }

    }
}
