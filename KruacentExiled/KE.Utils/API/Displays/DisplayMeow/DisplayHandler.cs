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






        public MHint AddHint(HintPlacement hintPlacement, Player player, string text, float delay)
        {
            var dis = PlayerDisplay.Get(player);
            string id = $"{player.Id}_{hintPlacement.XCoordinate}_{hintPlacement.YCoordinate}";
            MHint hint;

            if (!dis.TryGetHint(id, out var aHint))
            {
                hint = new()
                {
                    Text = text,
                    XCoordinate = hintPlacement.XCoordinate,
                    YCoordinate = hintPlacement.YCoordinate,
                    Alignment = hintPlacement.HintAlignment,
                    Id = id

                };
                dis.AddHint(hint);

            }
            else
            {
                
                hint = (MHint)aHint;
                hint.Hide = false;
                hint.Text = text;
            }

            
            hint.HideAfter(delay);
            return hint;
        }



        

        




    }
}
