using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.API.Interfaces;
using HintServiceMeow.Core.Extension;
using HintServiceMeow.Core.Models.Hints;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KE.Items.Utils
{
    public class HintFeed
    {
        private static readonly HintFeedPosition firstPosition = new();
        private static Dictionary<Player, HintFeed> playersFeed = new();
        public const int MaxFeed = 10;

        private readonly List<Feed> _feeds;
        private Player Player { get; }
        public static float Duration { get; set; } = 10;

        private HintFeed(Player player)
        {
            Player = player;
            _feeds = new();
        }

        public static HintFeed GetOrCreate(Player player)
        {
            if(!playersFeed.TryGetValue(player,out HintFeed feed))
            {
                feed = new(player);
                playersFeed[player] = feed;
            }
            return feed;
        }

        public static Feed AddFeed(Player player,string msg)
        {
            HintFeed hint = GetOrCreate(player);
            Feed feed = new Feed(msg);

            hint.AddFeed(feed);

            return feed;
        }


        public void AddFeed(Feed feed)
        {
            _feeds.Add(feed);
            UpdateDisplay();
            Timing.CallDelayed(Duration, () =>
            {
                _feeds.Remove(feed);
                UpdateDisplay();
            });
        }


        public void UpdateDisplay()
        {
            for (int i = 0; i < MaxFeed; i++)
            {
                HintPlacement placement = HintFeedPosition.GetIndex(i).HintPlacement;
                if (_feeds.TryGet(i, out Feed fe))
                {
                    KELog.Debug(fe.RawHint);
                    KELog.Debug("at "+i);

                    DisplayHandler.Instance.CreateAuto(Player, (args) => GetUpdate(fe), placement, HintServiceMeow.Core.Enum.HintSyncSpeed.Normal);
                }
                else
                {
                    KELog.Debug("removing feed at " + i);
                    DisplayHandler.Instance.RemoveHint(Player, placement);
                }
                    
            }
        }

        private string GetUpdate(Feed feed)
        {
            StringBuilder sb = StringBuilderPool.Pool.Get();

            sb.Append(Math.Truncate(DateTime.Now.Subtract(feed.TimeCreated).TotalSeconds));

            sb.Append("s ago - ");
            sb.Append(feed.RawHint);


            return StringBuilderPool.Pool.ToStringReturn(sb);
        }
        


        

    }
}
