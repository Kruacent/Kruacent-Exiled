using Exiled.API.Features;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using KE.Utils.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features.LobbyHints
{
    internal class WrongAspectRatioWarningLobbyHint : LobbyHintBase
    {

        public const string WrongAspectRatioTranslation = "WrongAspectRatioTranslation";
        public static Dictionary<string, Dictionary<string, string>> LangToKeyToTranslation = new Dictionary<string, Dictionary<string, string>>()
        {
            ["en"] = new Dictionary<string, string>()
            {
                [WrongAspectRatioTranslation] = "<color=red>Warning wrong aspect ratio : %CurrAspect% instead of 16:9</color>",
            },
            ["fr"] = new Dictionary<string, string>()
            {
                [WrongAspectRatioTranslation] = "<color=red>Attention mauvaise résolution d'écran : %CurrAspect% au lieu de 16:9</color>",
            },
        };

        private readonly HintPosition HintPosition = new WrongAspectRatioHintPosition();

        public override void Init(Player player)
        {
            DisplayHandler.Instance.CreateAuto(player, (args) => GetWarning(player), HintPosition.HintPlacement,HintServiceMeow.Core.Enum.HintSyncSpeed.Slow);
        }


        public override void Destroy()
        {
            foreach(Player player in Player.List)
            {
                DisplayHandler.Instance.RemoveHint(player, HintPosition.HintPlacement);
            }
        }



        private string GetWarning(Player player)
        {
            if (IsNotLobby()) return " ";
            if (CheckAspectRatio(player)) return " ";

            string translation = MainPlugin.GetTranslation(player, WrongAspectRatioTranslation);


            return translation.Replace("%CurrAspect%",player.AspectRatio.GetTranslation());

        }


        public bool CheckAspectRatio(Player player)
        {
            return player.AspectRatio == Exiled.API.Enums.AspectRatioType.Ratio16_9;
        }
    }


    
}
