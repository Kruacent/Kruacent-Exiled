using CommandSystem.Commands.RemoteAdmin.Broadcasts;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Server;
using HintServiceMeow.Core.Models.Hints;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Interfaces;
using KruacentExiled.GlobalEventFramework;
using KruacentExiled.GlobalEventFramework.GEFE.API.Enums;
using KruacentExiled.GlobalEventFramework.GEFE.API.Extensions;
using KruacentExiled.GlobalEventFramework.GEFE.API.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KruacentExiled.GlobalEventFramework.GEFE.API.Features
{
    public abstract class GlobalEvent : KEEvents
    {

        private class GlobalEventHandler : IUsingEvents
        {
            private bool _eventsub = false;

            public void SubscribeEvents()
            {
                if (_eventsub) return;

                Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
                Exiled.Events.Handlers.Server.RoundEnded += OnEndingRound;

                _eventsub = true;
            }

            public void UnsubscribeEvents()
            {
                if (!_eventsub) return;
                Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
                Exiled.Events.Handlers.Server.RoundEnded -= OnEndingRound;

                _eventsub = false;
            }

            private void OnEndingRound(RoundEndedEventArgs _)
            {
                Log.Warn("ending round");
                DisableEvents(_activeGE);
            }
            private void OnRoundStarted()
            {
                if(ForcedGE.Count == 0)
                {
                    Activate();
                }
                else
                {
                    _activeGE = ForcedGE.ToHashSet();
                    ForcedGE.Clear();
                    EnableEvents(_activeGE);
                    
                    Show();
                }
            }


        }


        private static Config Config => MainPlugin.Configs;
        private static GlobalEventHandler _handler = new GlobalEventHandler();

        private static HashSet<GlobalEvent> _activeGE = new HashSet<GlobalEvent>();

        public static IReadOnlyDictionary<ImpactLevel, string> ImpactToColor = new Dictionary<ImpactLevel, string>()
        {
            { ImpactLevel.VeryLow, "#d8d8ff" },
            { ImpactLevel.Low, "#d8e8f0" },
            { ImpactLevel.Medium, "#d8fcde" },
            { ImpactLevel.High, "#fbfbd8" },
            { ImpactLevel.VeryHigh, "#f0e8d8" },
            { ImpactLevel.Insane, "#ffd8d8" },
        };


        public static HashSet<GlobalEvent> ForcedGE { get; } = new HashSet<GlobalEvent>();


        /// <summary>
        /// A list of all registered GlobalEvents
        /// </summary>
        public static IEnumerable<GlobalEvent> GlobalEventsList => List.Where(ev => ev is GlobalEvent).Cast<GlobalEvent>();
        ///<inheritdoc/>
        public abstract string Description { get; }
        public virtual string[] AltDescription { get; } = null;

        public virtual ImpactLevel ImpactLevel { get; } = ImpactLevel.Medium;
        public bool IsActive
        {
            get
            {
                return _activeGE.Contains(this);
            }
        }


        public static int NumberOfGE { get; set; } = -1;



        protected sealed override void SubscribeEvents()
        {
            _handler.SubscribeEvents();
            base.SubscribeEvents();
        }

        protected sealed override void UnsubscribeEvents()
        {
            _handler.UnsubscribeEvents();
            
            base.UnsubscribeEvents();
        }


        protected override void Disable(KEEvents ev)
        {

            _activeGE.Remove(ev as GlobalEvent);
            base.Disable(ev);
        }

        private static void Activate()
        {
            if(NumberOfGE == -1)
            {
                NumberOfGE = Random.value < .1f ? 2 : 1;
            }


            _activeGE = GetRandomEvent<GlobalEvent>(NumberOfGE).ToHashSet();
            
            EnableEvents(_activeGE);
            Show();
        }

        private static void Show()
        {
            

            ShowConsole();


            string text = ShowText();

            foreach (Player player in Player.List)
            {
                AbstractHint hint = DisplayHandler.Instance.AddHint(MainPlugin.GEAnnouncement, player, text, 10);

                hint.FontSize = 30;
                
            }
        }

        private static void ShowConsole()
        {
            Log.Info($"Global Event(s) ({_activeGE.Count()}): ");
            foreach (GlobalEvent ge in _activeGE)
            {
                Log.Info(ge.Name);
            }

        }

        private List<string> AllDesc
        {
            get
            {
                List<string> allDesc = new List<string>()
                {
                    Description,
                };
                allDesc.AddRange(AltDescription);
                return allDesc;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="desc">0 mean the default desc</param>
        /// <param name="redacted"></param>
        /// <returns></returns>
        private static string ShowText()
        {
            StringBuilder builder = StringBuilderPool.Pool.Get();

            builder.Append("Global Events: ");
            List<GlobalEvent> ge = ListPool<GlobalEvent>.Pool.Get(_activeGE);







            for (int i = 0; i < ge.Count(); i++)
            {
                GlobalEvent globalEvent = ge[i];




                builder.Append("<color=");
                builder.Append(ImpactToColor[globalEvent.ImpactLevel]);
                builder.Append(">");

                builder.Append(globalEvent.ImpactLevel.Shorten());



                if (globalEvent.IsRedacted())
                {
                    builder.Append("[REDACTED]");
                }
                else
                {
                    if (!Config.ActivateAltDescription || globalEvent.AltDescription == null)
                    {
                        builder.Append(globalEvent.Description);
                    }
                    else
                    {
                        builder.Append(globalEvent.AllDesc.GetRandomValue());
                    }
                }

                builder.AppendLine("</color>");
                if (ge.Count() > 1 && i < ge.Count() - 1)
                {
                    builder.Append(", ");
                }


            }
            ListPool<GlobalEvent>.Pool.Return(ge);


            return StringBuilderPool.Pool.ToStringReturn(builder);
        }


        private bool IsRedacted()
        {
            float chanceRedacted;

            if(this is IChanceRedactable force)
            {
                chanceRedacted = force.ChanceRedacted;
            }
            else
            {
                chanceRedacted = Config.ChanceRedacted;
            }

            chanceRedacted = Mathf.Clamp(chanceRedacted, 0, 100);

            return Random.Range(0f, 100f) <= chanceRedacted;


        }

    }
}
