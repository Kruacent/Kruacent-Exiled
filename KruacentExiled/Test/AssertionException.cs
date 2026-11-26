using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Test
{

	[Serializable]
	public class AssertionException : Exception
	{

		public AssertionException(string message) : base(message)
		{

		}
    }
}
