using KE.Utils.API.Interfaces;

namespace KE.Utils.API.Models
{
    public class Models : IUsingEvents
    {
        private ModelCreator _model;

        public static Models Create()
        {
            return new();
        }

        private Models()
        {
            _model = new();
        }


        public void SubscribeEvents()
        {
            _model.SubscribeEvents();
        }

        public void UnsubscribeEvents()
        {
            _model.UnsubscribeEvents();
        }
    }
}
