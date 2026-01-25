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
using System.Linq;

namespace KE.Misc.Features.LastHuman
{
    public class LastHumanHandler : IUsingEvents
    {

        public static readonly string TextLast1 = "You feel like everyone is counting on you";
        public static readonly string TextLast2 = "You feel suddenly very lonely";

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


            if(TryGetLastTarget(out Player player))
            {
                AbstractHint hint = DisplayHandler.Instance.GetHint(player, position.HintPlacement);

                if (hint is null || hint.Hide)
                {
                    string msg = TextLast1;
                    if (UnityEngine.Random.Range(1,3) % 2 == 0)
                    {
                        msg = TextLast2;
                    }


                    DisplayHandler.Instance.AddHint(position.HintPlacement, player, msg, 10);

                    Log.Debug("show message to "+player.Nickname);
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
