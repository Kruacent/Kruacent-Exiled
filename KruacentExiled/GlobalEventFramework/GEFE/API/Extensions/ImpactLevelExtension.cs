using KruacentExiled.GlobalEventFramework.GEFE.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.GlobalEventFramework.GEFE.API.Extensions
{
    public static class ImpactLevelExtension
    {

        public static string Shorten(this ImpactLevel impact)
        {
            return impact switch
            {
                ImpactLevel.VeryLow => "[VL]",
                ImpactLevel.Low => "[L]",
                ImpactLevel.Medium => "[M]",
                ImpactLevel.High => "[H]",
                ImpactLevel.VeryHigh => "[VH]",
                ImpactLevel.Insane => "[I]",
                _ => ""
            };
        }

    }
}
