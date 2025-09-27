using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Interfaces;

namespace KE.CustomRoles
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = true;

        public int HeaderId { get; set; } = 1000;
        public int ScpPreferenceHeaderId { get; set; } = 10000;
    }
}
