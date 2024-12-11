using Exiled.API.Features;
using MEC;
using GEFExiled.GEFE.API.Features;
using PlayerHandler = Exiled.Events.Handlers.Player;
using Exiled.Events.EventArgs.Player;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;


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
            this.players = Player.List.ToList().Where(p => !p.IsNPC).ToList();
            this.players.ShuffleList();
            pos = new List<Vector3>(players.Count);
            for (int i = 0; i < players.Count; i++)
            {
                pos.Add(Vector3.zero); 
            }
            Log.Debug($"before while");
            while (!Round.IsEnded)
            {
                
                Log.Debug($"waiting for {GetType().Name}");
                yield return Timing.WaitForSeconds(UnityEngine.Random.Range(120, 240)); //120 240
                for (int i = 0; i < this.players.Count; i++)
                {
                    Log.Debug($"old position of player {this.players[i]} : {this.players[i].Position}");
                    var player = this.players[i];
                    pos[i] = player.Position;
                    Log.Debug("------");
                }

                ShiftLeft(this.players);
                Log.Debug("shifted players");
                for (int i = 0; i < this.players.Count; i++)
                {
                    Log.Debug("before tp");
                    this.players[i].Teleport(pos[i]);
                    Log.Debug($"new position of player {this.players[i]} : {this.players[i].Position}");
                }
                Log.Debug($"tp player");
                for (int i = 0; i < players.Count; i++)
                {
                    pos.Add(Vector3.zero);
                }
                Log.Debug($"cleared");
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
            if (!ev.Player.IsNPC)
            {
                this.players.Add(ev.Player);
                this.pos.Add(ev.Player.Position);
            }
        }

        private void ShiftLeft<T>(List<T> lst)
        {
            if (lst.Count > 0)
            {
                T firstElement = lst[0];
                for (int i = 1; i < lst.Count; i++)
                {
                    lst[i - 1] = lst[i];
                }
                lst[lst.Count - 1] = firstElement;
            }
        }


    }
}
