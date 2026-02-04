using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp106;
using HintServiceMeow.Core.Models.Hints;
using HintServiceMeow.Core.Utilities;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using KE.Utils.API.Features.SCPs;
using KE.Utils.API.Interfaces;
using LabApi.Events.Arguments.PlayerEvents;
using PlayerRoles;
using PlayerRoles.FirstPersonControl.Thirdperson;
using System.Collections.Generic;
using System.Linq;

namespace KE.Misc.Features.LastHuman
{
    public class LastHumanHandler : IUsingEvents
    {


        private static readonly IReadOnlyCollection<string> TextLast = new HashSet<string>()
        {
            "You feel like everyone is counting on you",
            "You feel suddenly very lonely"
        };


        public static HintPosition position = new LastHumanPosition();

        public void SubscribeEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.ChangedRole += OnChangedRole;
            Exiled.Events.Handlers.Scp106.Attacking += OnAttacking;
        }

        public void UnsubscribeEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.ChangedRole -= OnChangedRole;
            Exiled.Events.Handlers.Scp106.Attacking -= OnAttacking;
        }



        private void OnAttacking(AttackingEventArgs ev)
        {
            if (ev.IsAllowed && TryGetLastTarget(out _))
            {
                ev.Target.Kill(Exiled.API.Enums.DamageType.Scp106);
            }
        }
        private void OnChangedRole(PlayerChangedRoleEventArgs ev)
        {

            if (Round.ElapsedTime.TotalSeconds < 10)
                return;

            if (ev.OldRole.IsScp()) return;

            if (ev.NewRole.RoleTypeId.IsScp()) return;


            if(TryGetLastTarget(out Player _))
            {
                Player lastTarget = Player.Enumerable.Where(p => p.IsAlive && !p.IsScp && !p.IsTutorial).FirstOrDefault();
                foreach (Player player in Player.Enumerable)
                {
                    AbstractHint hint = DisplayHandler.Instance.GetHint(lastTarget, position.HintPlacement);

                    if (hint is null || hint.Hide)
                    {

                        string msg = string.Empty;
                        if (player == lastTarget)
                        {
                            msg = TextLast.GetRandomValue();
                        }
                        else if(!player.IsDead)
                        {
                            msg = "The last human is at " + player.Zone;
                        }





                        if (!player.IsDead)
                        {
                            DisplayHandler.Instance.AddHint(position.HintPlacement, player, msg, 10);
                        }
                        

                        Log.Debug("show message to " + lastTarget.Nickname);
                    }
                }





                
            }
        }


        private static bool TryGetLastTarget(out Player lastTarget)
        {
            lastTarget = null;
            int num = 0;
            int num2 = 0;
            foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
            {
                Log.Debug(allHub.nicknameSync.DisplayName);
                if (allHub.IsHuman())
                {
                    num++;
                    lastTarget = Player.Get(allHub);
                }
                else if (allHub.IsSCP())
                {
                    num2++;
                }
            }

 
            
            if (num == 1)
            {
                return num2 > 0;
            }
            return false;
        }


    }
}
