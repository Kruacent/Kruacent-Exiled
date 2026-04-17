using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using KE.Utils.API.Displays.DisplayMeow;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KruacentExiled.GlobalEventFramework.GEFE.API.Features
{

    public abstract class MiddleEvent : KEEvents
    {
        private class MiddleEventHandler
        {
            private CoroutineHandle _handle;
            private bool _eventsub = false;
            public void SubscribeEvents()
            {
                if (_eventsub) return;
                Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
                Exiled.Events.Handlers.Server.RoundEnded += OnEndingRound;
                Exiled.Events.Handlers.Server.RestartingRound += OnRestartingRound;


                _eventsub = true;
            }

            public void UnsubscribeEvents()
            {
                if (!_eventsub) return;

                Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
                Exiled.Events.Handlers.Server.RoundEnded -= OnEndingRound;
                Exiled.Events.Handlers.Server.RestartingRound -= OnRestartingRound;

                _eventsub = false;
            }

            private void OnEndingRound(RoundEndedEventArgs _)
            {
                Log.Debug("ending round");
                DisableEvents(_activeEv);
            }
            private void OnRestartingRound()
            {
                Log.Debug("restarting");
                DisableEvents(_activeEv);
            }
            private void OnRoundStarted()
            {
                TimeToActivate = new TimeSpan(0,UnityEngine.Random.Range(MinTimeToActivate.Minutes,MaxTimeToActivate.Minutes),0);
                _handle = Timing.RunCoroutine(Timer());
            }

            private IEnumerator<float> Timer()
            {
                if (UnityEngine.Random.Range(0f, 100f) < Chance)
                {
                    while (Round.InProgress)
                    {
                        yield return Timing.WaitForSeconds(60);
                        if(Round.ElapsedTime > TimeToActivate)
                        {
                            Activate();
                        }
                    }
                } 
            }
        }

        public abstract string Description { get; set; }

        public static float Chance = 37;
        public static TimeSpan MinTimeToActivate = new TimeSpan(0, 10, 0);
        public static TimeSpan MaxTimeToActivate = new TimeSpan(0, 16, 0);
        private static TimeSpan TimeToActivate;

        private static HashSet<MiddleEvent> _activeEv = new HashSet<MiddleEvent>();
        private static MiddleEventHandler _handler = new MiddleEventHandler();


        public static IReadOnlyCollection<MiddleEvent> ActiveMiddleEvent => _activeEv;

        public bool IsActive
        {
            get
            {
                return _activeEv.Contains(this);
            }
        }


        /// <summary>
        /// A list of all registered <see cref="MiddleEvent"/>
        /// </summary>
        public static IEnumerable<MiddleEvent> MiddleEventsList => List.Where(ev => ev is MiddleEvent).Cast<MiddleEvent>();

        protected sealed override void SubscribeEvents()
        {
            _handler.SubscribeEvents();
            base.SubscribeEvents();
        }
        protected sealed override void UnsubscribeEvents()
        {
            _handler.UnsubscribeEvents();
            DisableEvents(_activeEv);
            base.UnsubscribeEvents();
        }




        /// <summary>
        /// Get a random <see cref="MiddleEvent"/> and activate it
        /// </summary>
        public static bool Activate()
        {
            if (_activeEv.Count > 0) return false;

            _activeEv = GetRandomEvent<MiddleEvent>().ToHashSet();
            EnableEvents(_activeEv);
            Show();

            return true;
        }

        protected override void Disable(KEEvents ev)
        {
            _activeEv.Remove(ev as MiddleEvent);
            base.Disable(ev);
        }

        #region Show
        private static void Show()
        {
            var random = UnityEngine.Random.Range(0f, 100f);

            ShowConsole();
            if (_activeEv.Count == 0) return;
            foreach (Player player in Player.List)
            {
                DisplayHandler.Instance.AddHint(MainPlugin.GEAnnouncement, player, ShowText(), 10).FontSize = 30;
            }
        }

        private static void ShowConsole()
        {
            Log.Info($"Middle Event(s) ({_activeEv.Count()}): ");

            foreach (MiddleEvent ge in _activeEv)
            {
                Log.Info(ge.Name);
            }

        }

        private static string ShowText()
        {
            string result = "Middle Event: ";
            List<MiddleEvent> ge = _activeEv.ToList();


            for (int i = 0; i < ge.Count(); i++)
            {
                result += ge[i].Description;

                if (ge.Count() > 1 && i < ge.Count() - 1)
                {
                    result += ", ";
                }
            }


            return result;
        }

        #endregion





    }
}
