using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Test.Attributes
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple = true)]
    sealed class TestCaseAttribute : Attribute
    {

        public object[] Arguments { get; }

        public TestCaseAttribute(params object[] arguments)
        {
            Arguments = arguments;
        }

    }
}
