using KE.Utils.API.Interfaces;
using PlayerRoles.FirstPersonControl.Thirdperson;

namespace KE.Utils.API.Models
{
    public class Models : IUsingEvents
    {
        public ModelCreator ModelCreator
        {
            get;
            private set;
        }
        private static Models _instance;


        public static Models Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new();
                return _instance;
            }
        }

        public void DestroyInstance()
        {
            _instance = null;
        }

        private Models()
        {
            ModelCreator = new();
        }


        public void SubscribeEvents()
        {
            ModelCreator.SubscribeEvents();
        }

        public void UnsubscribeEvents()
        {
            ModelCreator.UnsubscribeEvents();
        }
    }
}
