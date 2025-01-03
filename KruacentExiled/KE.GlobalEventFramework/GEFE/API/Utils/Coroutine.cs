﻿using MEC;
using System.Collections.Generic;

namespace KE.GlobalEventFramework.GEFE.API.Utils
{
	public static class Coroutine
	{
		public static readonly List<CoroutineHandle> _coroutine = new List<CoroutineHandle>();

        //CoroutineHandle coroutine
        public static CoroutineHandle LaunchCoroutine(IEnumerator<float> coroutine)
		{
            CoroutineHandle a = Timing.RunCoroutine(coroutine);

            _coroutine.Add(a);
			return a;
		}
	}
}
