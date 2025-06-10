using Exiled.API.Features;
using HintServiceMeow.Core.Extension;
using HintServiceMeow.Core.Models.Hints;
using HintServiceMeow.Core.Utilities;
using MEC;


//damn why the same name
using MHint = HintServiceMeow.Core.Models.Hints.Hint;

namespace KE.Utils.API.Displays.DisplayMeow
{
    public class DisplayHandler
    {
        public static DisplayHandler Instance { get; } = new();

        private DisplayHandler() { }


        public void TempHint(AbstractHint hint, Player p,float delay)
        {
            var dis = PlayerDisplay.Get(p);
            dis.AddHint(hint);
            dis.RemoveAfter(hint, delay);

        }



        public MHint AddHint(HintPlacement hintPlacement, Player player, string text, float delay)
        {
            var dis = PlayerDisplay.Get(player);
            MHint hint = new()
            {
                Text = text,
                XCoordinate = hintPlacement.XCoordinate,
                YCoordinate = hintPlacement.YCoordinate,
                Alignment = hintPlacement.HintAlignment
            };
            dis.AddHint(hint);
            Timing.CallDelayed(delay, () =>
            {
                Log.Debug("remvoving hint");
                dis.RemoveHint(hint);
            });
            return hint;
        }


        




    }
}
