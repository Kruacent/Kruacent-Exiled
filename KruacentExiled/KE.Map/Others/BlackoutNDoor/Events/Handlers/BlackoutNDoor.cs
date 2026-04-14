using Exiled.Events.Features;
using KE.Map.Others.BlackoutNDoor.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Map.Others.BlackoutNDoor.Events.Handlers
{
    public static class BlackoutNDoor
    {


        public static Event<PreEventEventArgs> PreEvent { get; set; } = new Event<PreEventEventArgs>();
        public static Event<PostEventEventArgs> PostEvent { get; set; } = new Event<PostEventEventArgs>();

        public static Event<ChoseMapEventEventArgs> ChoseMapEvent { get; set; } = new();
        public static Event<ChoseZoneEventArgs> ChoseZoneEvent { get; set; } = new();
        public static void OnPreEvent(PreEventEventArgs ev)
        {
            PreEvent.InvokeSafely(ev);
        }
        public static void OnPostEvent(PostEventEventArgs ev)
        {
            PostEvent.InvokeSafely(ev);
        }

        public static void OnChoseMapEvent(ChoseMapEventEventArgs ev)
        {
            ChoseMapEvent.InvokeSafely(ev);
        }

        public static void OnChoseZoneEvent(ChoseZoneEventArgs ev)
        {
            ChoseZoneEvent.InvokeSafely(ev);
        }

    }
}
