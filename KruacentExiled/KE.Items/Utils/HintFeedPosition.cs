using Exiled.API.Features;
using HintServiceMeow.Core.Enum;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using KE.Utils.API.Features;
using System.Collections.Generic;

namespace KE.Items.Utils
{
    public sealed class HintFeedPosition : HintPosition
    {
        public const float BaseYPosition = 150;
        public const float Increments = -50;
        private float yposition = BaseYPosition;
        public override float Xposition => -350;

        public override float Yposition => yposition;

        private int index;
        public int Index => index;

        private static List<HintFeedPosition> nonalloc = new(HintFeed.MaxFeed);


        public static HintFeedPosition GetIndex(int index)
        {
            if (!nonalloc.TryGet(index, out HintFeedPosition position))
            {
                position = new HintFeedPosition()
                {
                    yposition = BaseYPosition + index * 25,
                    index = index
                };
            }
            //KELog.Debug($"get index {index} y pos=" + position.Yposition);
            return position;

        }

        public override HintAlignment HintAlignment => HintAlignment.Left;
        public override string Name => "HintFeed";


        
    }
}
