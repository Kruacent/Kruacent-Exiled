using Exiled.API.Features;
using System;
using RueI.Extensions;
using RueI.Displays;

namespace KE.Utils
{
    public static class KEHint
    {

        public static void ShowHint(this Player player, RueIHint rueIHint)
        {
            ShowHint(player, rueIHint.Hint, rueIHint.Position);
        }

        public static void ShowHint(this Player player, Hint hint, float position)
        {
            ShowHint(player, hint.Content, position, TimeSpan.FromSeconds(hint.Duration));
        }
        public static void ShowHint(this Player p, string message, float position, TimeSpan timeSpan)
        {
            ShowHint(p.ReferenceHub, message, position, timeSpan);
        }

        public static void ShowHint(ReferenceHub hub, string message, float position, TimeSpan timeSpan)
        {
            DisplayCore c = DisplayCore.Get(hub);

            c.SetElemTemp(message, position, timeSpan, new RueI.Displays.Scheduling.TimedElemRef<RueI.Elements.SetElement>());
        }

    }
}
