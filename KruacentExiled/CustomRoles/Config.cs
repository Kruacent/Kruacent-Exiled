using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Interfaces;

namespace KruacentExiled.CustomRoles
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = true;

        public int HeaderId { get; set; } = 1000;

        [Description("the minimum id the custom scp preference will use (Note: the header will always be the first)")]
        public int CustomScpSliderRangeMin { get; set; } = 10000;
        public int CustomScpSliderRangeMax { get; set; } = 10500;
    }
}
