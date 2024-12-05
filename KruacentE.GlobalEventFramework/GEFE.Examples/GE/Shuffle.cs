using System.Collections.Generic;
using Exiled.API.Features;
using MEC;
using GEFExiled.GEFE.API.Features;
using PlayerHandler = Exiled.Events.Handlers.Player;
using Exiled.Events.EventArgs.Player;
using UnityEngine;

namespace GEFExiled.GEFE.Examples.GE
{
    public class Shuffle : GlobalEvent
    {
        ///<inheritdoc/>
        public override int Id { get; set; } = 31;
        ///<inheritdoc/>
        public override string Name { get; set; } = "Shuffle";
        ///<inheritdoc/>
        public override string Description { get; set; } = "et ça fait roomba café dans le scp";
        ///<inheritdoc/>
        public override double Weight { get; set; } = 0;
        private List<Player> players;
        private List<Vector3> pos;
        ///<inheritdoc/>
        public override IEnumerator<float> Start()
        {
            this.players.ShuffleList();
            pos = new List<Vector3>();
            while (!Round.IsEnded)
            {
                yield return Timing.WaitForSeconds(Random.Range(120, 240));
                for (int i = 0; i < this.players.Count; i++)
                {
                    pos[i] = this.players[i].Position;
                }
                ShiftLeft(this.players);
                for (int i = 0; i < this.players.Count; i++)
                {
                    this.players[i].Position = pos[i];
                }
                pos.Clear();
            }
        }

        public override void SubscribeEvent()
        {
            PlayerHandler.Joined += OnJoined;
        }

        public override void UnsubscribeEvent()
        {
            PlayerHandler.Joined -= OnJoined;
        }


        private void OnJoined(JoinedEventArgs ev)
        {
            this.players.Add(ev.Player);
        }

        private void ShiftLeft<T>(List<T> lst)
        {
            for (int i = 1; i < lst.Count; i++)
            {
                lst[i - 1] = lst[i];
            }

            for (int i = lst.Count - 1; i < lst.Count; i++)
            {
                lst[i] = default;
            }
        }

    }
}
