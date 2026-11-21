using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Test
{
    public static class Assert
    {
        public static void IsTrue(bool condition,string messageIfFailed)
        {
            if (!condition)
            {
                throw new AssertionException(messageIfFailed);
            }
                
        }

        public static void IsFalse(bool condition, string messageIfFailed)
        {
            if (condition)
            {
                throw new AssertionException(messageIfFailed);
            }  
        }

        public static void Fail(string message)
        {
            throw new AssertionException(message);
        }
    }
}
