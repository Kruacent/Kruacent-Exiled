using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Misc.Features.LastHuman
{
    public static class LastHumanTranslations
    {


        public static Dictionary<string, Dictionary<string, string>> LangToKeyToTranslation = new Dictionary<string, Dictionary<string, string>>()
        {
            ["en"] = new Dictionary<string, string>()
            {
                ["texthuman1"] = "You feel like everyone is counting on you",
                ["texthuman2"] = "You feel suddenly very lonely",
                ["textscp"] = "<color=#FF0000><b>The last human is at %Zone%</b></color>",
            },
            ["fr"] = new Dictionary<string, string>()
            {
                ["texthuman1"] = "Tu a la sensation que tout le monde compte sur toi",
                ["texthuman2"] = "Tu te sens soudainement très seul",
                ["textscp"] = "<color=#FF0000><b>Le dernier humain est à %Zone%</b></color>",
            },
        };


    }
}
