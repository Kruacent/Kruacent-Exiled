using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using KE.Utils.Quality.Handlers;
using KE.Utils.Quality.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Utils.Quality
{
    [Obsolete("scrapped", true)]
    internal class QualityHandler
    {
        public QualitySettings QualitySettings { get; private set; }
        public QualityToysHandler QualityToysHandler { get; private set; }

        private static QualityHandler _instance;
        public static QualityHandler Instance
        {
            get
            {
                if(_instance == null )
                    _instance = new QualityHandler();
                return _instance;
            }
        }
        private QualityHandler()
        {
            QualitySettings = new QualitySettings(Changed);
            QualityToysHandler = new QualityToysHandler();
        }

        ~QualityHandler()
        {
            QualityToysHandler = null;
            QualitySettings = null;
        }

        public void Changed(Player p, SettingBase _)
        {
            QualityToysHandler.Sync(p);
        }

        public void Register()
        {
            QualitySettings.Register();
        }

        public void Unregister()
        {
            QualitySettings.Unregister();
        }

        public static void Sync()
        {
            Instance.QualityToysHandler.Sync();
        }
    }
}
