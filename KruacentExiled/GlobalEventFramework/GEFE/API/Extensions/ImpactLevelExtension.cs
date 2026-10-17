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
        /// <summary>
        /// Convert the <see cref="ImpactLevel"/> to a short human readable form
        /// </summary>
        /// <param name="impact"></param>
        /// <returns></returns>
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
