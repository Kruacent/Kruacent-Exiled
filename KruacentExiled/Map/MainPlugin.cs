
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using KE.Map.Quality;
using Map.GamblingZone;

namespace KE.Map
{
    public class MainPlugin : Plugin<Config>
    {
        public static MainPlugin Instance { get; private set; }
        public override void OnEnabled()
        {
            Exiled.Events.Handlers.Map.Generated += OnGenerated;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
            Exiled.Events.Handlers.Server.RoundStarted += SendFakePrimitives.Join;

            Instance = this;
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Map.Generated -= OnGenerated;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Server.RoundStarted -= SendFakePrimitives.Join;

            Instance = null;
        }


        private void OnGenerated()
        {
            var l = new LootTable();
            var g = new GamblingRoom(Room.Get(Exiled.API.Enums.RoomType.Lcz173), l);
            g.SubscribeEvents();


        }

        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            foreach (var g in GamblingRoom.List)
                g.UnsubscribeEvents();
        }
    }


    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = true;
    }
}
