using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp106;
using Exiled.Events.Patches.Generic;
using HintServiceMeow.Core.Models.Hints;
using HintServiceMeow.Core.Utilities;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using KE.Utils.API.Features;
using KE.Utils.API.Features.SCPs;
using KE.Utils.API.Interfaces;
using KE.Utils.API.Translations;
using KE.Utils.Extensions;
using LabApi.Events.Arguments.PlayerEvents;
using PlayerRoles;
using PlayerRoles.FirstPersonControl.Thirdperson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KE.Misc.Features.LastHuman
{
    public class LastHumanHandler : IUsingEvents
    {





        public static readonly IReadOnlyCollection<string> TextLast = new HashSet<string>()
        {
            "texthuman1",
            "texthuman2",
        };

        public static readonly string TextSCP = "textscp";



        public static HintPosition position = new LastHumanPosition();
        private DateTime _nextPossibleHint;

        public static readonly TimeSpan Cooldown = TimeSpan.FromSeconds(20);
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

            if (SCPTeam.IsSCP(ev.Player.ReferenceHub)) return;


            if(TryGetLastTarget(out Player lastTarget))
            {
                foreach (Player player in Player.Enumerable)
                {
                    AbstractHint hint = DisplayHandler.Instance.GetHint(lastTarget, position.HintPlacement);

                    if (hint is null || hint.Hide)
                    {

                        string translatedZone = lastTarget.Zone.GetTranslatedName(TranslationHub.GetLang(player));

                        string msg = string.Empty;
                        if (player == lastTarget)
                        {
                            msg = MainPlugin.GetTranslation(player,TextLast.GetRandomValue());
                        }
                        else if(!player.IsDead)
                        {
                            
                            msg = MainPlugin.GetTranslation(player, TextSCP).Replace("%Zone%", translatedZone);
                        }


                        

                        if (!player.IsDead && DateTime.Now > _nextPossibleHint)
                        {
                            DisplayHandler.Instance.AddHint(position.HintPlacement, player, msg, 10);
                            KELog.Debug("show message to " + lastTarget.Nickname);
                            _nextPossibleHint = DateTime.Now.Add(Cooldown);
                        }
                        

                        
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
                KELog.Debug(allHub.nicknameSync.DisplayName);
                if (allHub.IsHuman() && !SCPTeam.IsSCP(allHub))
                {
                    num++;
                    lastTarget = Player.Get(allHub);
                }
                else if (SCPTeam.IsSCP(allHub))
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
