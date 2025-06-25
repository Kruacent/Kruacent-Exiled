using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.GlobalEventFramework.GEFE.Exceptions
{
    public class FailedRegisterException(string message) : Exception(message)
    {
    }
}
