using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.GlobalEventFramework.GEFE.Exceptions
{
    public class FailedRegisterException : Exception
    {

        public FailedRegisterException(string message) : base(message)
        {

        }
    }
}
