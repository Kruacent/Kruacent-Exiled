using Exiled.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features.LobbyHints
{
    public static class AspectRatioTypeExtension
    {

        public static string GetTranslation(this AspectRatioType ratio)
        {
            if (ratio == AspectRatioType.Unknown) return "Unknown";
            string result = ratio.ToString();
            return result.Replace('_', ':').Remove(0,5);
        }


    }
}
