using HintServiceMeow.UI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.CustomItems.API.Exceptions
{
    internal class CustomItemNotFoundException : Exception
    {

        public const string Message = "Custom item : %CustomItemName% not found";

        public CustomItemNotFoundException(string nameCustomItem) : base(Message.Replace("%CustomItemName%",nameCustomItem))
        {

        }
    }
}
