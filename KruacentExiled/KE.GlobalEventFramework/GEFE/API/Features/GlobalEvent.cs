using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
using MEC;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using System;
using KE.Utils.API.Displays.DisplayMeow;
using Exiled.Events.EventArgs.Server;
using KE.Utils.API.Interfaces;
using System.Text;

namespace KE.GlobalEventFramework.GEFE.API.Features
{
    public abstract class GlobalEvent : KEEvents
    {

        private class GlobalEventHandler : IUsingEvents
        {
            private bool _eventsub = false;

            public void SubscribeEvents()
            {
                if (_eventsub) return;

                Log.Debug("registering GlobalEvent");
                Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
                Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
                Exiled.Events.Handlers.Server.RoundEnded += OnEndingRound;
                Exiled.Events.Handlers.Server.RestartingRound += OnRestartingRound;

                _eventsub = true;
            }

            public void UnsubscribeEvents()
            {
                if (!_eventsub) return;

                Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
                Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
                Exiled.Events.Handlers.Server.RoundEnded -= OnEndingRound;
                Exiled.Events.Handlers.Server.RestartingRound -= OnRestartingRound;

                _eventsub = false;
            }
            private void OnWaitingForPlayers()
            {
                StopCoroutines();
            }
            private void OnEndingRound(RoundEndedEventArgs _)
            {
                Log.Debug("ending round");
                DeactivateAll();
            }
            private void OnRestartingRound()
            {
                Log.Debug("restarting");
                DeactivateAll();
            }
            private void OnRoundStarted()
            {
                SetActiveGlobalEvent();
            }


        }

        private static GlobalEventHandler _handler = new();

        private static HashSet<GlobalEvent> _activeGE = new();
        public static float ChanceRedacted = 25;


        /// <summary>
        /// A list of all registered GlobalEvents
        /// </summary>
        public static IEnumerable<GlobalEvent> GlobalEventsList => List.Where(ev => ev is GlobalEvent).Cast<GlobalEvent>();
        ///<inheritdoc/>
        public abstract string Description { get; set; }


        public bool IsActive
        {
            get 
            { 
                return _activeGE.Contains(this); 
            }
        }



        protected override void SubscribeEvents()
        {
            _handler.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            _handler.UnsubscribeEvents();
        }


        private static void DeactivateAll()
        {
            foreach (GlobalEvent ge in _activeGE)
            {
                if (ge is IEvent geEvent)
                {
                    geEvent.UnsubscribeEvent();
                }
                _activeEvents.Remove(ge);
            }
            _activeGE.Clear();

        }




        private static void SetActiveGlobalEvent()
        {
            int nbGE = UnityEngine.Random.value < .1f ? 2 : 1;
            _activeGE = GetRandomEvent<GlobalEvent>(nbGE).ToHashSet();
            ActivateAll(_activeGE);
        }


        private static void ActivateAll(IEnumerable<GlobalEvent> globalEvent)
        {
            if (globalEvent.Count() != globalEvent.Distinct().Count()) throw new ArgumentException("You can't have the same GE twice in the same round");

            foreach (GlobalEvent ge in _activeGE)
            {
                if (ge is IEvent geEvent)
                {
                    Log.Debug($"{ge.Name} implements IEvent, subscribing events");
                    geEvent.SubscribeEvent();
                }

                if (ge is IStart geStart)
                {
                    Log.Debug($"{ge.Name} implements IStart, starting");
                    CoroutineHandle a = Timing.RunCoroutine(geStart.Start());
                    ge.coroutineHandles.Add(a);
                }
                _activeEvents.Add(ge);
            }
            
            Show();
        }


        /// <summary>
        /// Stop all Coroutine from GE
        /// </summary>
        private static void StopCoroutines()
        {
            foreach(GlobalEvent ge in GlobalEventsList)
            {
                foreach(CoroutineHandle handle in ge.coroutineHandles)
                {
                    Timing.KillCoroutines(handle);
                }
            }
        }

        

        private static void Show()
        {
            var random = UnityEngine.Random.Range(0f,100f);
            Log.Debug("random="+random);
            ShowConsole();
            foreach (Player player in Player.List)
            {
                DisplayHandler.Instance.AddHint(MainPlugin.GEAnnouncement, player, ShowText(random < ChanceRedacted), 10).FontSize = 30;
            }
        }

        private static void ShowConsole()
        {
            Log.Info($"Global Event(s) ({_activeGE.Count()}): ");

            foreach(GlobalEvent ge in _activeGE)
            {
                Log.Info(ge.Name);
            }

        }

        private static string ShowText(bool redacted = false)
        {
            StringBuilder builder = new();

            builder.Append("Global Events: ");
            List<GlobalEvent> ge = _activeGE.ToList();


            for (int i = 0; i < ge.Count(); i++)
            {
                if (redacted)
                {
                    builder.Append("[REDACTED]");
                }
                else
                {

                    builder.Append(ge[i].Description);
                }

                if (ge.Count() > 1 && i < ge.Count() - 1)
                {
                    builder.Append(", ");
                }
            }


            return builder.ToString();
        }


    }


}
