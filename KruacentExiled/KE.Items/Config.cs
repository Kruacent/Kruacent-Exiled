using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Items
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public float LightRefreshRate { get; set; } = .01f;
        public float ModelRefreshRate { get; set; } = .1f;
        public string SoundLocation { get; set; } = "C:\\Users\\Patrique\\AppData\\Roaming\\EXILED\\Plugins\\audio";
        public int Position { get; set; } = 300;
    }
}
