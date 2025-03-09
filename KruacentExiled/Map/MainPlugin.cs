
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Map.GamblingZone;

namespace Map
{
    public class MainPlugin : Plugin<Config>
    {
        public override void OnEnabled()
        {
            Exiled.Events.Handlers.Map.Generated += OnGenerated;
            
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Map.Generated -= OnGenerated;
            
        }


        private void OnGenerated()
        {
            var l = new LootTable();
            var g = new GamblingRoom(Room.Get(Exiled.API.Enums.RoomType.Lcz173),l);

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
        public bool IsEnabled { get; set; }
        public bool Debug { get; set; }
    }
}
