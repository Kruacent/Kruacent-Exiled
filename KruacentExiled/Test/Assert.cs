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

        public static void Catch<TException>(Action action, string messageIfFailed) where TException : Exception
        {
            try
            {
                action();
            }
            catch (TException)
            {
                return;
            }
            catch (Exception e)
            {
                throw new AssertionException($"{messageIfFailed} : waiting {typeof(TException).Name}, got {e.GetType().Name}");
            }

            throw new AssertionException($"{messageIfFailed} : waiting {typeof(TException).Name}, no exceptions");
        }
    }
}
