using System.Collections.Generic;

namespace KruacentExiled.Misc.Features.ScpProximityChat
{
    public static class ScpProximityChatTranslations
    {
        public const string Enabled = "ScpProximityChatEnabled";
        public const string Disabled = "ScpProximityChatDisabled";

        public static Dictionary<string, Dictionary<string, string>> LangToKeyToTranslation = new Dictionary<string, Dictionary<string, string>>()
        {
            ["en"] = new Dictionary<string, string>()
            {
                [Enabled] = "<b>Proximity chat <color=green>enabled</color></b>",
                [Disabled] = "<b>Proximity chat <color=red>disabled</color></b>",
            },
            ["fr"] = new Dictionary<string, string>()
            {
                [Enabled] = "<b>Chat de proximité <color=green>activé</color></b>",
                [Disabled] = "<b>Chat de proximité <color=red>désactivé</color></b>",
            },
        };
    }
}
