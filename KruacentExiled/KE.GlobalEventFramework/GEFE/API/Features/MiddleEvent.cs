using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Server;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using KE.Utils.API;
using KE.Utils.API.Displays.DisplayMeow;
using MEC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KE.GlobalEventFramework.GEFE.API.Features
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
                Log.Debug("registering middle event");
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
                Deactivate();
            }
            private void OnRestartingRound()
            {
                Log.Debug("restarting");
                Deactivate();
            }
            private void OnRoundStarted()
            {
                Timing.RunCoroutine(Timer());
            }

            private IEnumerator<float> Timer()
            {
                if (UnityEngine.Random.Range(0f, 100f) < 37)
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
        public static TimeSpan TimeToActivate = new(0, 15, 0);

        private static HashSet<MiddleEvent> _activeEv = new();
        private static MiddleEventHandler _handler = new();




        protected sealed override void SubscribeEvents()
        {
            _handler.SubscribeEvents();
        }
        protected sealed override void UnsubscribeEvents()
        {
            _handler.UnsubscribeEvents();
            Deactivate();
        }

        protected virtual void SubscribeEvent()
        {

        }

        protected virtual void UnsubscribeEvent()
        {

        }





        /// <summary>
        /// Get a random <see cref="MiddleEvent"/> and activate it
        /// </summary>
        public static bool Activate()
        {
            if (_activeEv.Count > 0) return false;

            _activeEv = GetRandomEvent<MiddleEvent>().ToHashSet();
            foreach(MiddleEvent ev in _activeEv)
            {
                if (ev is IStart start)
                    ev.coroutineHandles.Add(Timing.RunCoroutine(start.Start()));
                ev.SubscribeEvent();
                _activeEvents.Add(ev);
            }
            Show();

            return true;
        }

        public static void Deactivate(MiddleEvent m)
        {
            if (!_activeEv.Contains(m)) throw new ArgumentException("middleevent cannot be deactivate : not activated");
            m.UnsubscribeEvent();
            if (m is IReversible r)
                r.OnDisable();
            _activeEv.Remove(m);
            _activeEvents.Remove(m);
        }

        public static void Deactivate()
        {
            foreach (MiddleEvent ev in _activeEv)
            {
                ev.UnsubscribeEvent();
                foreach(CoroutineHandle handle in ev.coroutineHandles)
                {
                    Timing.KillCoroutines(handle);
                    if (ev is IReversible revert)
                        revert.OnDisable();
                }
            }
            _activeEv.Clear();
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
