using Exiled.API.Features;
using Exiled.API.Features.Core.Generic;
using Exiled.Events.EventArgs.Warhead;
using KE.Utils.API.Features;
using KE.Utils.API.Features.SCPs;
using LabApi.Events.Arguments.WarheadEvents;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features.PostNuke
{
    internal class PostNukeHandler : MiscFeature
    {

        private Faction trigger = Faction.Unclassified;

        public override void SubscribeEvents()
        {
            LabApi.Events.Handlers.WarheadEvents.Detonated += OnDetonated;
            LabApi.Events.Handlers.WarheadEvents.Starting += OnStarting;

            base.SubscribeEvents();
        }

        public override void UnsubscribeEvents()
        {
            LabApi.Events.Handlers.WarheadEvents.Detonated -= OnDetonated;
            LabApi.Events.Handlers.WarheadEvents.Starting -= OnStarting;
            base.UnsubscribeEvents();
        }

        private void OnStarting(WarheadStartingEventArgs ev)
        {
            if (SCPTeam.IsSCP(ev.Player.ReferenceHub))
            {
                trigger = Faction.SCP;
            }
            else
            {
                trigger = ev.Player.Role.GetFaction();
            }

                
        }

        private void OnDetonated(WarheadDetonatedEventArgs ev)
        {
            Player player = ev.Player;

            AlphaWarheadSyncInfo info = AlphaWarheadController.Singleton.Info;

            if (info.ScenarioType == WarheadScenarioType.DeadmanSwitch) return;

            if (player is null || player.IsHost)
            {
                if(UnityEngine.Random.Range(0,2) == 0)
                {
                    trigger = Faction.FoundationStaff;
                }
                else
                {
                    trigger = Faction.FoundationEnemy;
                }
            }

            KELog.Debug(trigger);

            if (trigger == Faction.Unclassified 
                || trigger == Faction.FoundationStaff 
                || trigger == Faction.Flamingos) return;


            Timing.CallDelayed(1, () =>
            {
                Respawn.GrantTokens(trigger, 1);
                Respawn.AdvanceTimer(trigger, 150);
            });


        }





    }
}
